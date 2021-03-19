using Godot;
using System;
using EventCallback;
public class DeathManager : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //The death listener for the on death event
        DeathEvent.RegisterListener(OnDeathEvent);

    }

    private void OnDeathEvent(DeathEvent de)
    {
        //If the returned object from the id is in the group monster
        if (((Node)GD.InstanceFromId(de.targetID)).IsInGroup("Monster"))
        {
            //Send a message out for the creation of a corpse
            CreateCorpseEvent cce = new CreateCorpseEvent();
            cce.callerClass = "DeathManager - OnDeathEvent()";
            cce.monsterID = de.targetID;
            cce.FireEvent();
        }
        //Free the node of the monster object that has died
        ((Node)GD.InstanceFromId(de.targetID)).QueueFree();
        
    }
}
