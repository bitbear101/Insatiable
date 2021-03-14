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
        if((InstanceFromID(de.targetID)).IsInGroup("Monster"))
        {
            CreateCorpseEvent cce = new CreateCorpseEvent();
            cce.callerClass = "DeathManager - OnDeathEvent()";
            cce.monsterID = de.targetID;
            cce.FireEvent();
        }

        (InstanceFromID(de.targetID)).FreeQeue();
        //Get the id of hte actor dying and if its a monster generate corpse
    }
}
