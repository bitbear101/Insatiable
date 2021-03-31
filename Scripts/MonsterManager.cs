using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EventCallback;
public class MonsterManager : Node2D
{

    //The external list of monster scenes to instantiate when the game initiates for the first time
    [Export] List<PackedScene> monsterScenes = new List<PackedScene>();
    //The list of monsters spawned on the level
    List<Node> monsterList = new List<Node>();
    //The base amount of monsters to spawn
    int baseMonsterCount = 1;

    public override void _Ready()
    {
        //Set the list of monster scenes to a list, workaround for Godot not to crash with c# lists as export
        monsterScenes = monsterScenes.ToList();
        monsterList = monsterList.ToList();
        //The listener for the turn managers state
        BroadcastTurnEvent.RegisterListener(OnBroadcastTurnEvent);
        //The event listenenr for the monster spawner event
        SpawnMonstersEvent.RegisterListener(OnSpawnMonstersEvent);
        //Removes the monster form the list when sent
        RemoveMonsterEvent.RegisterListener(OnRemoveMonsterEvent);
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
            //The maximum level of the monster for this level depth is selected 
            int levelOfMonsterToSpawn = monsterScenes.Count / gmle.maxLevels;
            //Create a new node for the monster
            Node monsterToSpawn = new Node();
            //We prepare to spawn in the randomly selected monster
            monsterToSpawn = monsterScenes[levelOfMonsterToSpawn].Instance();
            //We add the monster to the main monster list
            monsterList.Add(monsterToSpawn);
            //Get a random floor tile to spawn monster on
            GetRandomFloorTileEvent grfte = new GetRandomFloorTileEvent();
            grfte.callerClass = "MonsterManager - OnSpawnMonstersEvent";
            grfte.FireEvent();
            //Set the monsters position to the tile position in the world
            ((Node2D)monsterToSpawn).Position = grfte.tilePos * 16;
            //Add the monster node as a child of the monster manager 
            AddChild(monsterToSpawn);
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
            SetDamageTypeEvent sdte = new SetDamageTypeEvent();
            sdte.callerClass = "MonsterManager - OnSpawnMonsterEvent";
            sdte.actorID = monsterToSpawn.GetInstanceId();
            sdte.damageType = damageType;//Get the type of monster and set the damage type acording
            sdte.FireEvent();
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

    private void OnRemoveMonsterEvent(RemoveMonsterEvent rme)
    {
        GD.Print("MonsterManager - OnRemoveMonsterEvent : Called");
        //Loop through the monster list an remove the one sent in the message 
        for (int i = 0; i < monsterList.Count; i++)
        {
            //Lopp throught the list of monsters spawned
            if (rme.monsterID == monsterList[i].GetInstanceId())
            {
                GD.Print("MonsterManager - OnRemoveMonsterEvent : rme.monsterID == monsterList[i].GetInstanceId() = " + i + ": " + rme.monsterID + " == " + monsterList[i].GetInstanceId());
                GD.Print("MonsterManager - OnRemoveMonsterEvent : monsterList.Count Before = " + monsterList.Count);
                //Remove the monster with the id that has died
                monsterList.RemoveAt(i);
                GD.Print("MonsterManager - OnRemoveMonsterEvent : monsterList.Count After = " + monsterList.Count);
                //Return out of the loop to save resources, ya I know uneeded optiization but I want to do it ok!
                return;
            }
        }
    }
}

