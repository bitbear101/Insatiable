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
        agse.corpseID = cde.attackerID;
        agse.FireEvent();
        //Get the targets stats
        GetStatsEvent tgse = new GetStatsEvent();
        tgse.callerClass = "CalculateDamage - OnCalculatDamageEvent";
        tgse.corpseID = cde.targetID;
        tgse.FireEvent();

        //The initial dadge chance is gotten by sutracting on actors dexterity with the other actors
        int tempDodgeChance = (tgse.dexterity - agse.dexterity);
        //If the temp dodge chance is greater than zero we work out the percentage chance for a dodge
        if (tempDodgeChance > 0) dodgeChance = (tempDodgeChance * 100) / tempDodgeChance;
        //If the random number is greater then the dodge chance
        if (rng.RandiRange(0, 100) > dodgeChance)
        {
            //We set the damage taken to the strength added to the level then multiplied to return only 25% of the damage
            cde.damage = (int)((float)(agse.strength + agse.level) * 0.25f);
        }
        else
        {
            //The actor dodged he attack and takes zero damage
            cde.damage = 0;
        }
    }

}