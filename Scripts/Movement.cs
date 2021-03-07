using Godot;
using System;
using EventCallback;
public class Movement : Node
{
    //The directional ray
    RayCast2D dirRay;
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
        dirRay = GetNode<RayCast2D>("../DirectionRay");
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
        bool canMoveMap = false, canMoveRay = true;

        //Cast the ray towards the direction of movement
        dirRay.CastTo = dir * 16;
        //Forces the raycast to update and detect the collision with the building object
        //touchRay.ForceRaycastUpdate();
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
                //Call the needed event messages
            }
        }
        //2. check the direction from the move direction to the map
        GetTileEvent gte = new GetTileEvent();
        gte.callerClass = "Movement - CheckDirection()";
        gte.pos = new Vector2(((Node2D)GetParent()).Position / 16 + dir);
        gte.FireEvent();
        GD.Print("tile in move direction = " + gte.tile);
        if (gte.tile == TileType.FLOOR)
        {
            canMoveMap = true;
        }
        else if (gte.tile == TileType.STONE)
        {
            //We wont move but we can call the 
            canMoveMap = false;
        }
        else
        {
            canMoveMap = false;
        }

        //Return if we can move or not here by comparint if the ray or map is stopping it
        return (canMoveMap && canMoveRay);
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
