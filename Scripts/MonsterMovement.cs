using Godot;
using System;
using EventCallback;
public class MonsterMovement : Node
{
    //The directional ray
    RayCast2D directionRay;
    //The lenght of the raay that checks the movement direction
    int rayLenght = 16;
    //

    /*
    1. Get the parents body to be able to move it
    2. Get and check the direction of movement on the map 
    3. Check the movement direction for other objects in the was by using raycasts in the direction of movement
    4. If enemy in way call hit event
    5. If nothing in the way move in that direction
    
    */

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        MoveDirectionEvent.RegisterListener(OnMoveDirectionEvent);
        directionRay = GetNode<RayCast2D>("../DirectionRay");
        directionRay.CastTo = Vector2.Up * rayLenght;
    }

    private void OnMoveDirectionEvent(MoveDirectionEvent mde)
    {
        //Check if the the direction of movement is valid
        if (CheckDirection(mde.dir))
        {
            //If hte direction of movement is valid we move in that direction
            Move(mde.dir);
        }
    }

    private bool CheckDirection(Vector2 dir)
    {
        //1. Check if we can move from the turn manager

        //If the actor can move we set it true
        bool canMove = false;
        //2. check the direction from the move direction to the map
        GetTileEvent gte = new GetTileEvent();
        gte.callerClass = "Movement - CheckDirection()";
        gte.pos = new Vector2(((Node2D)GetParent()).Position / 16 + dir);
        gte.FireEvent();
        GD.Print("tile in move direction = " + gte.tile);
        if (gte.tile == TileType.FLOOR)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }

        //Do the ray check here
        return canMove;
    }

    private void Move(Vector2 dir)
    {
        ((Node2D)GetParent()).Position += dir * 16;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
