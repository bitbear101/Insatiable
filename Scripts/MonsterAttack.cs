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
        if (eae.enemyID != GetParent().GetInstanceId()) return;
        CheckRange(eae.target);
    }
    private void CheckRange(Node2D target)
    {
        //Get the normalized direction to the target
        Vector2 rayDir = (target.Position - ((Node2D)GetParent()).Position).Normalized();
        //Floor the dir vector for the y and x axises so as to exclude the diagonal checks
        rayDir.x = Mathf.Floor(rayDir.x);
        rayDir.y = Mathf.Floor(rayDir.y);
        //Cast the ray towards the direction of movement
        dirRay.CastTo = rayDir * 16;
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
                HitEvent he = new HitEvent();
                he.callerClass = "MoonsterAttack - CheckRange";
                he.attackerID = GetParent().GetInstanceId();
                he.targetID = hitNode.GetParent().GetInstanceId();
                he.FireEvent();
            }
            else
            {
                //If the collision was not the player then switch to the move state again
                SetEnemyStateEvent sese = new SetEnemyStateEvent();
                sese.callerClass = "MonsterAttack";
                sese.enemyID = GetParent().GetInstanceId();
                sese.newState = EnemyState.MOVE;
                sese.FireEvent();
            }
        }
        //Disable hte ray as all detection should be done
        dirRay.Enabled = false;
    }
    public override void _ExitTree()
    {
        //Listen for the attack event message
        EnemyAttackEvent.UnregisterListener(OnEnemyAttackEvent);
    }
}
