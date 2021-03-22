using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EventCallback;
public class MonsterManager : Node2D
{

    //The external list of monster scenes to instantiate when the game initiates for the first time
    [Export]
    private List<PackedScene> monsterScenes = new List<PackedScene>();
    //The list of nodes that will hold the pre loaded scenes
    List<Node> monsterTypesList = new List<Node>();
    //The list of monsters spawned on the level
    List<Node> monsterList = new List<Node>();
    //The base amount of monsters to spawn
    int baseMonsterCount = 10;

    public override void _Ready()
    {
        //Set the list of monster scenes to a list, workaround for Godot not to crash with c# lists as export
        monsterScenes = monsterScenes.ToList();
        //The listener for the turn managers state
        BroadcastTurnEvent.RegisterListener(OnBroadcastTurnEvent);
        //The event listenenr for the monster spawner event
        SpawnMonstersEvent.RegisterListener(OnSpawnMonstersEvent);
        //Check if the main scenes list is not zero 
        if (monsterScenes.Count > 0)
        {
            //Loop through all the scenes in the list
            foreach (PackedScene monsterScene in monsterScenes)
            {
                //Add the node of the scenes
                monsterTypesList.Add(monsterScene.Instance());
            }
        }
    }

    private void OnSpawnMonstersEvent(SpawnMonstersEvent sme)
    {
        //Clear the list of monsters already spawned for the prevoius level
        ClearMonsterList();

        //Get how deep we are in the dungeon (The dungeon level)
        GetMapLevelEvent gmle = new GetMapLevelEvent();
        gmle.callerClass = "MonsterManager - OnSpawnMonsterEvent";
        gmle.FireEvent();
        //Create a random number geneator
        RandomNumberGenerator rng = new RandomNumberGenerator();
        //Loop through the base amount of montsters and spawn as we go
        for (int i = 0; i < baseMonsterCount + (gmle.mapLevel * 1.25f); i++)
        {
            if (gmle.mapLevel < 6)
            {
                //The level of monster is selected 
                int levelOfMonsterToSpawn = monsterTypesList.Count / gmle.maxLevels;
                //If the levelOfMonsterToSpawn is zero we set it to the max monsterTypesList's count
                //if(levelOfMonsterToSpawn == 0) levelOfMonsterToSpawn = monsterTypesList.Count;
                //We prepare to spawn in the randomly selected monster
                Node monsterToSpawn = monsterTypesList[levelOfMonsterToSpawn];
                //We add the monster to the main monster list
                monsterList.Add(monsterToSpawn);

                GetRandomFloorTileEvent grfte = new GetRandomFloorTileEvent();
                grfte.callerClass = "MonsterManager - OnSpawnMonstersEvent";
                grfte.FireEvent();

                //We set the position of the newly created monster to a randpom floor tile event and then multiply the position 
                //by the size of the tile sprite to place it correctly in the world
                ((Node2D)monsterToSpawn).Position = grfte.tilePos * 16;
                //BEHOLDER, FIEND, SLIME, FLAMING_SKULL, GOBLIN, SKELETON
                SetMonsterTypeEvent smte = new SetMonsterTypeEvent();
                smte.callerClass = "MonsterManager - OnSpawnMonsterEvent";
                smte.monsterID = monsterToSpawn.GetInstanceId();
                smte.monsterType = (MonsterTypes)levelOfMonsterToSpawn;
                smte.FireEvent();

                DamageType damageType = DamageType.SLASH;

                switch ((MonsterTypes)levelOfMonsterToSpawn)
                {
                    case MonsterTypes.BEHOLDER:
                        damageType = DamageType.ELECTRIC;
                        break;
                    case MonsterTypes.FIEND:
                        damageType = DamageType.SLASH;
                        break;
                    case MonsterTypes.SLIME:
                        damageType = DamageType.PIERCE;
                        break;
                    case MonsterTypes.FLAMING_SKULL:
                        damageType = DamageType.FIRE;
                        break;
                    case MonsterTypes.GOBLIN:
                        damageType = DamageType.SLASH;
                        break;
                    case MonsterTypes.SKELETON:
                        damageType = DamageType.PIERCE;
                        break;

                }

                //Set the monsters damage type
                //Set the monsters damage type
                SetDamageTypeEvent sdte = new SetDamageTypeEvent();
                sdte.callerClass = "MonsterManager - OnSpawnMonsterEvent";
                sdte.actorID = monsterToSpawn.GetInstanceId();
                sdte.damageType = damageType;//Get the type of monster and set the damage type acording
                sdte.FireEvent();
            }
        }

    }

    private void ClearMonsterList()
    {
        //Loop through the list and delete the monster nodes
        foreach (Node monster in monsterList)
        {
            monster.QueueFree();
        }
        //After the nodes have been deleted we clear the list
        monsterList.Clear();
    }

    private void OnBroadcastTurnEvent(BroadcastTurnEvent bte)
    {
        //If the state manager is not on the enemies turn we return out of the method
        if (bte.states != TurnStates.ENEMY_TURN) return;

        foreach (Node monster in monsterList)
        {
            ((Monster)monster).Process();
        }

        //At the end of the monsters turn we cycle the turn
        CycleTurnEvent cte = new CycleTurnEvent();
        cte.callerClass = "Monster - _Process(float delta)";
        cte.FireEvent();
    }
}

