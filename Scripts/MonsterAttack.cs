using Godot;
using System;
using EventCallback;
public class MonsterAttack : Node
{
    RayCast2D dirRay;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Listen for the attack event message
        EnemyAttackEvent.RegisterListener(OnEnemyAttackEvent);
        //Get the raycast node to use in the script
        dirRay = GetNode<RayCast2D>("../HitBox/DirectionRay");
    }

    private void OnEnemyAttackEvent(EnemyAttackEvent eae)
    {
        CheckRange(eae.target);
    }
    private void CheckRange(Node2D target)
    {
        //Cast the ray towards the direction of movement
        dirRay.CastTo = target.Position - ((Node2D)GetParent()).Position;
        GD.Print("MonsterAttack - CheckRange : direction = " + (target.Position - ((Node2D)GetParent()).Position) / 16);
        //Enable the ray to detect collisions
        dirRay.Enabled = true;
        //Forces the raycast to update and detect the collision with the building object
        dirRay.ForceRaycastUpdate();
        //Check for collisions
        if (dirRay.IsColliding())
        {
            //Get the node that the ray collided with
            Node2D hitNode = dirRay.GetCollider() as Node2D;
            //If the ray cast hit returns a object beloning to the group play
            if (hitNode.IsInGroup("Player"))
            {
                //Call the hit event
                GD.Print("MonsterAttack - CheckRange : Calling hit event");
            }
        }
        //Disable hte ray as all detection should be done
        dirRay.Enabled = false;
    }
}
