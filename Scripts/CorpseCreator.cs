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
                scse.strength = 0;
                scse.dexterity = 1;
                scse.intelligence = 5;
                scse.corruption = 5;
                break;
            case MonsterTypes.FIEND:
                scse.strength = 1;
                scse.dexterity = 1;
                scse.intelligence = 1;
                scse.corruption = 3;
                break;
            case MonsterTypes.SLIME:
                scse.strength = 2;
                scse.dexterity = 1;
                scse.intelligence = 0;
                scse.corruption = 2;
                break;
            case MonsterTypes.FLAMING_SKULL:
                scse.strength = 2;
                scse.dexterity = 2;
                scse.intelligence = 0;
                scse.corruption = 4;
                break;
            case MonsterTypes.GOBLIN:
                scse.strength = 1;
                scse.dexterity = 2;
                scse.intelligence = 0;
                scse.corruption = 3;
                break;
            case MonsterTypes.SKELETON:
                scse.strength = 2;
                scse.dexterity = 0;
                scse.intelligence = 0;
                scse.corruption = 2;
                break;
        }

        scse.FireEvent();
    }

}
