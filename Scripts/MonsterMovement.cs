using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EventCallback;
public class MonsterMovement : Node
{
    //The directional ray
    RayCast2D dirRay;
    //The lenght of the raay that checks the movement direction
    int rayLenght = 16;
    //The astar path for the enemy
    AStar2D astar2d = new AStar2D();
    //The used cells from the map
    List<Vector2> cells = new List<Vector2>();
    //The path to be moved along
    List<Vector2> path = new List<Vector2>();
    //If the player is in rand start following him
    bool isInRange = false;
    //The target of the monster
    Node2D target;
    //Has the path tiles already been retrieved
    bool gotTiles = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cells = cells.ToList();
        path = path.ToList();

        //Get the raycast node to use in the script
        dirRay = GetNode<RayCast2D>("../HitBox/DirectionRay");
        //the listener for the StoneToFloorEvent
        UpdateMapCellsEvent.RegisterListener(OnUpdateMapCellsEvent);
        //The move enemy event listener
        EnemyMoveEvent.RegisterListener(OnEnemyMoveEvent);
    }
    private void OnUpdateMapCellsEvent(UpdateMapCellsEvent umce)
    {

        AddMapPoints();
        ConnectPoints();
    }
    private void AddMapPoints()
    {
        GetUsedCellsEvent guce = new GetUsedCellsEvent();
        guce.callerClass = "MonsterMovement - AddMapPoints";
        guce.FireEvent();
        //Get the cells from the tile map
        cells = guce.cells;
        //Loop through the cells
        for (int i = 0; i < cells.Count; i++)
        {
            astar2d.AddPoint(GetID(cells[i]), cells[i], 1.0f);
        }
    }

    private void ConnectPoints()
    {
        //Declare the directions of the neighbours
        Vector2[] neighbours = { Vector2.Up, Vector2.Left, Vector2.Right, Vector2.Down };
        //Loop through all the cells tha we got from the map
        for (int i = 0; i < cells.Count; i++)
        {
            //Loop through the neigbours
            for (int j = 0; j < neighbours.Length; j++)
            {
                //Get the next neighbour to check
                Vector2 nextCell = cells[i] + neighbours[j];
                if (cells.Contains(nextCell))
                {
                    astar2d.ConnectPoints(GetID(cells[i]), GetID(nextCell), false);
                }
            }
        }
    }

    private void GetPath(Vector2 start, Vector2 end)
    {
        if (!gotTiles)
        {
            //Add the map cells to the list for connection
            AddMapPoints();
            //Connect all the posible walkable tiles
            ConnectPoints();
            //Set the got tiles true after this so it doesn't get called again
            gotTiles = true;
        }
        //Set the coordinates of the nodes from world coordinates to tile map coordinates
        start /= 16;
        end /= 16;
        //Get the points on the path and cast it to a list of vector2s
        path = astar2d.GetPointPath(GetID(start), GetID(end)).Cast<Vector2>().ToList();
        //We remove the first entry in the list
        if (path.Count != 0) path.RemoveAt(0);
    }

    private int GetID(Vector2 point)
    {
        int a = (int)point.x;
        int b = (int)point.y;
        return (a + b) * (a + b + 1) / 2 + b;
    }

    private bool CheckNextTile(Vector2 dir)
    {
        bool canMove = true;
        //Cast the ray towards the direction of movement
        dirRay.CastTo = dir * 16;
        //Enable the ray to detect collisions
        dirRay.Enabled = true;
        //Forces the raycast to update and detect the collision with the building object
        dirRay.ForceRaycastUpdate();
        //Check for collisions
        if (dirRay.IsColliding())
        {
            //Get the node that the ray collided with
            Node2D hitNode = dirRay.GetCollider() as Node2D;
            //If the ray cast hit returns a object beloning to the group monster
            if (hitNode.IsInGroup("Monster"))
            {
                //We set the canMove state tot false;
                canMove = false;
            }
            //If the ray cast hit returns a object beloning to the group play
            if (hitNode.IsInGroup("Player"))
            {
                //We set the canMove state tot false;
                canMove = false;
                //If none are left we change to to the attack state
                EnemyAttackEvent eae = new EnemyAttackEvent();
                eae.callerClass = "MonsterMovement - CheckNextTile";
                eae.target = (Node2D)hitNode.GetParent();
                eae.enemyID = GetParent().GetInstanceId();
                eae.FireEvent();
            }
        }
        //Disable hte ray as all detection should be done
        dirRay.Enabled = false;
        return canMove;
    }

    private void OnRangeAreaEntered(Area2D area)
    {
        //If the area that collided with the monsters range area is the player
        if (area.IsInGroup("Player"))
        {
            //Set is in range to true
            isInRange = true;
            //Set the target to the the player
            target = (Node2D)area.GetParent();
        }
    }
    private void OnRangeAreaExited(Area2D area)
    {
        //isInRange = true;
        target = null;
        //Set is in range to true
        isInRange = false;
    }
    //Check the line of sight of the target
    private bool CheckLOS()
    {
        //The bool to indicate if the target is in line of sight
        bool inLOS = false;
        //The vectors that wil represent the corners for the target ssprite (Note brute forcing it is not a good idea but it works)
        Vector2[] corners = { new Vector2(-7, -7), new Vector2(-7, 7), new Vector2(7, -7), new Vector2(7, 7), Vector2.Zero };
        //Enable the ray to detect collisions
        dirRay.Enabled = true;
        //Loop through the corners array 
        foreach (Vector2 corner in corners)
        {

            GD.Print("MonsterMovement - CheckLOS : going through corners");
            GD.Print("MonsterMovement - CheckLOS : target.Position = " + target.Position);
            //Cast the ray to the requested corner
            dirRay.CastTo = new Vector2(target.GlobalPosition.x + corner.x, target.GlobalPosition.y + corner.y);
            //dirRay.CastTo = ((target.Position - ((Node2D)GetParent()).Position) + corner);
            GD.Print("MonsterMovement - CheckLOS : going through corner pos = " + (new Vector2(target.GlobalPosition.x + corner.x, target.GlobalPosition.y + corner.y)));
            //Forces the raycast to update and detect the collision with the building object
            dirRay.ForceRaycastUpdate();
            //Check for collisions
            if (dirRay.IsColliding())
            {
                //Get the node that the ray collided with
                Node2D hitNode = dirRay.GetCollider() as Node2D;

                if (hitNode.IsInGroup("Player"))
                {
                    GD.Print("MonsterMovement - hitNode.IsInGroup Player = true");
                    inLOS = true;
                    break;
                }
            }
        }
        //Cast the ray towards the direction of movement
        //dirRay.CastTo = target.Position - ((Node2D)GetParent()).Position;
        //Disable hte ray as all detection should be done
        dirRay.Enabled = false;
        return inLOS;
    }

    private void OnEnemyMoveEvent(EnemyMoveEvent eme)
    {
        GD.Print("MonsterMovement - OnEnemyMoveEvent : before GetParent().GetInstanceId() = " + eme.enemyID + " " + GetParent().GetInstanceId());
        //If the parent calling the move class is this scripts parent we keep running the method
        if (eme.enemyID != GetParent().GetInstanceId()) return;
        GD.Print("MonsterMovement - OnEnemyMoveEvent : after GetParent().GetInstanceId() = " + eme.enemyID + " " + GetParent().GetInstanceId());
        //If the target is in range we continue running the method
        if (!isInRange) return;
        //We get the path from the list of tiles that can be traveled
        GetPath(((Node2D)GetParent()).Position, target.Position);
        if (path.Count < 1) return;
        //If the player or enemy is in its way it exits out of the move function
        if (!CheckNextTile(path[0] - (((Node2D)GetParent()).Position / 16))) return;
        //Check if there are any path vectors left in the list
        if (path.Count > 1)
        {
            //Set the position of the monster to the next position in the path
            ((Node2D)GetParent()).Position = path[0] * 16;
            path.RemoveAt(0);
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        EnemyMoveEvent.UnregisterListener(OnEnemyMoveEvent);
    }
}
