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
    int baseMonsterCount = 5;

    public override void _Ready()
    {
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
        for (int i = 0; i < gmle.mapLevel * (gmle.mapLevel + 1.25f); i++)
        {
            //The maximum level of the monster for this level depth is selected 
            int levelOfMonsterToSpawn = monsterScenes.Count / gmle.maxLevels;
            //Create a new node for the monster
            Node monsterToSpawn = new Node();
            //We prepare to spawn in the randomly selected monster
            monsterToSpawn = monsterScenes[0].Instance();
            //We add the monster to the main monster list
            monsterList.Add(monsterToSpawn);
            //Get a random floor tile to spawn monster on
            GetRandomFloorTileEvent grfte = new GetRandomFloorTileEvent();
            grfte.callerClass = "MonsterManager - OnSpawnMonstersEvent";
            grfte.FireEvent();
            //Change the name of the monster
            monsterToSpawn.Name = "Monster" + monsterList.Count;
            //Set the monsters position to the tile position in the world
            ((Node2D)monsterToSpawn).Position = (grfte.tilePos * 16) + (Vector2.One * 8);
            //Add the monster node as a child of the monster manager 
            AddChild(monsterToSpawn);
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

    private void OnRemoveMonsterEvent(RemoveMonsterEvent rme)
    {
        int itemToRemove = -1;
        //Loop through the monster list an remove the one sent in the message 
        for (int i = 0; i < monsterList.Count; i++)
        {
            //Lopp throught the list of monsters spawned
            if (rme.monsterID == monsterList[i].GetInstanceId())
            {
                //Get hte index to the monster to remove from the list
                itemToRemove = i;
            }
        }
        //Remove the monster with the id that has died if the id is not -1 (meaning the monster was not found in the list)
        if (itemToRemove != -1) monsterList.RemoveAt(itemToRemove);
    }
}

