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
    //Is the target in Line Of Sight LOS
    bool inLOS = false;
    //The target of the monster
    Node2D target;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        dirRay = GetNode<RayCast2D>("../HitBox/DirectionRay");
    }

    private void AddMapPoints()
    {
        GetUsedCellsEvent guce = new GetUsedCellsEvent();
        guce.callerClass = "MonsterMovement";
        guce.FireEvent();

        GD.Print("MonsterMovement - AddMapPoints : cells count = " + guce.cells.Count);
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

    private bool CheckDirection(Vector2 dir)
    {
        //If the actor can move we set it true
        bool canMoveRay = true;

        //Cast the ray towards the direction of movement
        dirRay.CastTo = dir * 16;
        //Enable the ray to detect collisions
        dirRay.Enabled = true;
        //Forces the raycast to update and detect the collision with the building object
        dirRay.ForceRaycastUpdate();
        //Check for collisions
        if (dirRay.IsColliding())
        {
            canMoveRay = false;
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

        //Return if we can move or not here by comparint if the ray or map is stopping it
        return (canMoveRay);
    }

    private void OnRangeAreaEntered(Area2D area)
    {
        if (area.IsInGroup("Player"))
        {
            AddMapPoints();
            ConnectPoints();
            isInRange = true;
            target = (Node2D)area.GetParent();
        }
    }
    private void OnRangeAreaExited(Area2D area)
    {
        isInRange = true;
        target = null;
    }
    //Check the line of sight of the target
    private void CheckLOS()
    {

    }

    private void Move()
    {
        //If the target is not in line of sight we exit out of the function without doing anything
        //if (!inLOS) return;

        if (target == null) return;
        GetPath(((Node2D)GetParent()).Position, target.Position);
        //if (CheckDirection())
        //{
        //Move to the first 
        ((Node2D)GetParent()).Position = path[0] * 16;
        path.RemoveAt(0);
        //}

    }

    public override void _Process(float delta)
    {
        Move();
    }
}
