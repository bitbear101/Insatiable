using Godot;
using System;
using EventCallback;
public class Health : Node
{
    //The base health of the actor
    int health = 5;
    public override void _Ready()
    {
        //The listener for the hit event
        HitEvent.RegisterListener(OnHitEvent);
        //Get the stats of the actor to work out its full health
        GetStatsEvent gse = new GetStatsEvent();
        gse.actorID = GetParent().GetInstanceID();
        gse.FireEvent();
        //Set the new health of the new actor
        health += ((sge.strength * 0.1f) + (sge.level * 0.2f)); 
    }

    private void OnHitEvent(HitEvent he)
    {
        if (he.target.GetInstanceId() == GetParent().GetInstanceId())
        {
            CalculateDamageEvent cde = new CalculateDamageEvent();
            cde.attackerID = he.attackerID;
            cde.targetID = he.targetID.
            cde.FireEvent();
            TakeDamage(cde.damage);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        //If there is no more health
        if (health <= 0)
        {
            DeathEvent de = new DeathEvent();
            de.callerClass = "Health - TakeDamage";
            de.targetID = GetParent().GetInstanceId();
            de.FireEvent();
        }
    }
}
