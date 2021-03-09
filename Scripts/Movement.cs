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
        dirRay = GetNode<RayCast2D>("../HitBox/DirectionRay");
    }

    private void OnMoveDirectionEvent(MoveDirectionEvent mde)
    {
        //Check if the the direction of movement is valid
        if (CheckDirection(mde.dir))
        {
            //If hte direction of movement is valid we move in that direction
            Move(mde.dir);
        }

        //At the end of the players turn we cycle the turn
        CycleTurnEvent cte = new CycleTurnEvent();
        cte.callerClass = "Movement - OnMoveDirectionEvent()";
        cte.FireEvent();
    }

    private bool CheckDirection(Vector2 dir)
    {
        //If the actor can move we set it true
        bool canMoveMap = false, canMoveRay = true;

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
                canMoveRay = false;
                //Send the needed event messages 
            }
            if (hitNode.IsInGroup("Monster"))
            {
                canMoveRay = false;
                HitEvent he = new HitEvent();
                he.callerClass = "Movement - CheckDirection";
                he.target = (Node2D)hitNode.GetParent();
                he.FireEvent();

            }
        }
        //Disable hte ray as all detection should be done
        dirRay.Enabled = false;
        //2. check the direction from the move direction to the map
        GetTileEvent gte = new GetTileEvent();
        gte.callerClass = "Movement - CheckDirection()";
        gte.pos = new Vector2(((Node2D)GetParent()).Position / 16 + dir);
        gte.FireEvent();
        if (gte.tile == TileType.FLOOR)
        {
            canMoveMap = true;
        }
        else if (gte.tile == TileType.STONE)
        {
            //We wont move but we can call the 
            canMoveMap = false;
            //If the player collides with a stone tile we change it to a floor tile
            StoneToFloorEvent stfe = new StoneToFloorEvent();
            stfe.callerClass = "Movement - CheckDirection()";
            stfe.TileToChange = new Vector2(((Node2D)GetParent()).Position / 16 + dir);
            stfe.FireEvent();
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
}
