using Godot;
using System;
using EventCallback;

public enum DamageType
{
    FIRE,
    ELECTRIC,
    SLASH,
    PIERCE
};
public class Stats : Node
{
    //The damage type the  does
    DamageType damageType;
    //The level of the actor
    int level;
    //The experience of the actor
    float experience;
    //The strength of the actor
    int strength;
    //The dexterity of the actor
    int dexterity;
    //The intelligence of the actor
    int intelligence;
    //The corruption of the actor
    int corruption;
    
    //Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //The listener for the Get stats listener 
        GetStatsEvent.RegisterListener(OnGetStatsEvent);
        //Set the damage type
        SetDamageTypeEvent.RegisterListener(OnSetDamageTypeEvent);
        //The listener for hte set stats listener 
        SetStatsEvent.RegisterListener(OnSetStatsEvent);
    }

    private void OnGetStatsEvent(GetStatsEvent gse)
    {
        //Check the id of the actor to get the defence o
        if(gse.corpseID == GetParent().GetInstanceId())
        {
            gse.strength = strength;
            gse.dexterity = dexterity;
            gse.intelligence = intelligence;
        }
    }

        private void OnSetStatsEvent(SetStatsEvent sse)
    {
        //Check the id of the actor to get the defence o
        if(sse.actorID == GetParent().GetInstanceId())
        {
            strength = sse.strength;
            dexterity = sse.dexterity;
            intelligence = sse.intelligence;
            corruption = sse.corruption;
        }
    }
    private void OnSetDamageTypeEvent(SetDamageTypeEvent sdte)
    {
        //Set the damage type if the acto id is equal the actors parent node
        if(GetParent().GetInstanceId() == sdte.actorID) damageType = sdte.damageType;
    }
}
