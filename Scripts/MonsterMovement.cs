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
        path.RemoveAt(0);
    }

    private int GetID(Vector2 point)
    {
        int a = (int)point.x;
        int b = (int)point.y;
        return (a + b) * (a + b + 1) / 2 + b;
    }

    private void CheckDirection(Vector2 dir)
    {
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
            if (hitNode.IsInGroup("Corps"))
            {
                //Send the needed event messages 
            }
            if (hitNode.IsInGroup("Monster"))
            {
                HitEvent he = new HitEvent();
                he.callerClass = "Movement - CheckDirection";
                he.target = (Node2D)hitNode.GetParent();
                he.FireEvent();
            }
        }
        //Disable hte ray as all detection should be done
        dirRay.Enabled = false;
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
        bool inLOS = false;
        //Cast the ray towards the direction of movement
        dirRay.CastTo = target.Position - ((Node2D)GetParent()).Position;
        //Enable the ray to detect collisions
        dirRay.Enabled = true;
        //Forces the raycast to update and detect the collision with the building object
        dirRay.ForceRaycastUpdate();
        //Check for collisions
        if (dirRay.IsColliding())
        {
            //Get the node that the ray collided with
            Node2D hitNode = dirRay.GetCollider() as Node2D;

            GD.Print("MonsterMovement - CheckLOS : hitnode.name = " + hitNode.Name);
            if (hitNode.IsInGroup("Player"))
            {
                inLOS = true;
            }
        }
        //Disable hte ray as all detection should be done
        dirRay.Enabled = false;
        return inLOS;
    }

    private void OnEnemyMoveEvent(EnemyMoveEvent eme)
    {
        if (eme.enemyID != GetParent().GetInstanceId()) return;
        if (!isInRange) return;
        //If the target is not in line of sight we exit out of the function without doing anything
        if (!CheckLOS()) return;
        GetPath(((Node2D)GetParent()).Position, target.Position);

        //Check if there are any path vectors left in the list
        if (path.Count > 1)
        {
            //CheckDirection(path[0] - (((Node2D)GetParent()).Position / 16));

            ((Node2D)GetParent()).Position = path[0] * 16;
            path.RemoveAt(0);
        }
        else
        {
            //If none are left we change to to the attack state
            SetEnemyStateEvent sese = new SetEnemyStateEvent();
            sese.callerClass = "MonsterMovement - OnEnemyMoveEvent";
            sese.enemyID = GetParent().GetInstanceId();
            sese.newState = EnemyState.ATTACK;
            sese.FireEvent();
        }
        //}

        //Check distance from player and if close enough send a message to the enemy script to attack next round

    }
}
