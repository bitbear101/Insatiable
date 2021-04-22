using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EventCallback;

public class CorpseCreator : Node2D
{
    //The list of packed scenes for the monster corpses
    [Export] List<PackedScene> corpseScenes = new List<PackedScene>();

    public override void _Ready()
    {
        //The create corpse listener for the on death event
        CreateCorpseEvent.RegisterListener(OnCreateCorpseEvent);
    }

    private void OnCreateCorpseEvent(CreateCorpseEvent cce)
    {
        GetMonsterTypeEvent gmte = new GetMonsterTypeEvent();
        gmte.callerClass = "CorpseCreator - OnCreateCorpseEvent()";
        gmte.monsterID = cce.monsterID;
        gmte.FireEvent();
        //The new corpse to be created
        Node newCorpse;
        //The event message for the corpse stats setting
        SetCorpseStatsEvent scse = new SetCorpseStatsEvent();
        //Get the monster corpse according to the monster type
        newCorpse = corpseScenes[(int)gmte.monsterType].Instance();
        //Set the position of hte corpse
        ((Node2D)newCorpse).Position = ((Node2D)GD.InstanceFromId(cce.monsterID)).Position;
        //Add the new corpse to the scene as a child
        AddChild(newCorpse);

        switch (gmte.monsterType)
        {
            case MonsterTypes.BEHOLDER:
                //Create the node for the corpse
                // scse.strength;
                // scse.dexterity;
                // scse.intelligence;
                // scse.corruption;
                break;
            case MonsterTypes.FIEND:
                // scse.strength;
                // scse.dexterity;
                // scse.intelligence;
                // scse.corruption;
                break;
            case MonsterTypes.SLIME:
                // scse.strength;
                // scse.dexterity;
                // scse.intelligence;
                // scse.corruption;
                break;
            case MonsterTypes.FLAMING_SKULL:
                // scse.strength;
                // scse.dexterity;
                // scse.intelligence;
                // scse.corruption;
                break;
            case MonsterTypes.GOBLIN:
                // scse.strength;
                // scse.dexterity;
                // scse.intelligence;
                // scse.corruption;
                break;
            case MonsterTypes.SKELETON:
                // scse.strength;
                // scse.dexterity;
                // scse.intelligence;
                // scse.corruption;
                break;
        }

        scse.FireEvent();
    }

}
