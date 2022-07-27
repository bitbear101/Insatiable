using Godot;
using System;
using EventCallback;
public class CalculateDamage : Node
{
    //The chance the actor has of dodging
    int dodgeChance;
    public override void _Ready()
    {
        //The listener for the hit event
        CalculateDamageEvent.RegisterListener(OnCalculateDamageEvent);
    }

    private void OnCalculateDamageEvent(CalculateDamageEvent cde)
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();

        //Get the attackers stats
        GetStatsEvent agse = new GetStatsEvent();
        agse.callerClass = "CalculateDamage - OnCalculatDamageEvent";
        agse.actorID = cde.attackerID;
        agse.FireEvent();
        //Get the targets stats
        GetStatsEvent tgse = new GetStatsEvent();
        tgse.callerClass = "CalculateDamage - OnCalculatDamageEvent";
        tgse.actorID = cde.targetID;
        tgse.FireEvent();
        //The initial dadge chance is gotten here lol
        float dodgeChance = Mathf.Abs(((tgse.dexterity * tgse.level) - (agse.dexterity * agse.level)) * 100 / (tgse.dexterity * tgse.level));
        //If the temp dodge chance is greater than zero we work out the percentage chance for a dodge
        if (rng.RandiRange(0, 100) < dodgeChance)
        {
            //We set the damage taken to the strength added to the level then multiplied to return only 25% of the damage
            cde.damage = (agse.strength + agse.level);
        }
        else
        {
            //The actor dodged he attack and takes zero damage
            cde.damage = 0;
        }
    }

    public override void _ExitTree()
    {
        //The listener for the hit event
        CalculateDamageEvent.UnregisterListener(OnCalculateDamageEvent);
    }

}