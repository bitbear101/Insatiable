using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EventCallback;

public class CorpseCreator : Node
{
    //The list of packed scenes for the monster corpses
    [Export] List<PackedScene> corpseScenes = new List<PackedScene>();
    //The list of nodes that will hold the pre loaded scenes
    List<Node> corpseNodes = new List<Node>();

    public override void _Ready()
    {
        corpseScenes = corpseScenes.ToList();
        corpseNodes = corpseNodes.ToList();

        //Check if the corpse scenes list is not zero 
        if (corpseScenes.Count > 0)
        {
            //Loop through all the scenes in the list
            foreach (PackedScene scene in corpseScenes)
            {
                //Add the instanced scenes to the node list
                corpseNodes.Add(scene.Instance());
            }
        }
        //The create corpse listener for the on death event
        CreateCorpseEvent.RegisterListener(OnCreateCorpseEvent);
    }

    private void OnCreateCorpseEvent(CreateCorpseEvent cce)
    {
        GetMonsterTypeEvent gmte = new GetMonsterTypeEvent();
        gmte.monsterID = cce.monsterID;
        gmte.FireEvent();

        SetCorpseStatsEvent scse = new SetCorpseStatsEvent();
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
