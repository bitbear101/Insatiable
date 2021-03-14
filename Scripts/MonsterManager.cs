using Godot;
using System;
using System.Collections.Generic;
using EventCallback;
public class MonsterManager : Node2D
{

    //The external list of monster scenes to instantiate when the game initiates for the first time
    [Export]
    List<PackedScene> monsterScenes = new List<PackedScene>();
    //The list of nodes that will hold the pre loaded scenes
    List<Node> monsterTypes = new List<Node>();
    //The list of monsters spawned on the level
    List<Node> monsterList = new List<Node>();
    //The base amount of monsters to spawn
    int baseMonsterCount = 10;

    public override void _Ready()
    {
        //The listener for the turn managers state
        BroadcastTurnEvent.RegisterListener(OnBroadcastTurnEvent);
        //The event listenenr for the monster spawner event
        SpawnMonstersEvent.RegisterEventListener(OnSpawnMonstersEvent);
        //Check if the main scenes list is not zero 
        if (monsterScenes.Count > 0)
        {
            //Loop through all the scenes in the list
            foreach (PackedScene scene in monsterScenes)
            {
                //Add the node of the scenes
                monsterTypes.Add(scene.Instance());
            }
            //Loop through the list of scene nodes and add them to the current scene as children
            // foreach (Node monster in monsterNodes)
            // { 
            //     ((Node2D)monster).Position = new Vector2(16,16);
            //     AddChild(monster);
            // }
            Node2D tempMonster1 = ((Node2D)monsterTypes[0]);
            tempMonster1.Position = new Vector2(16, 16);
            Node2D tempMonster2 = ((Node2D)monsterTypes[1]);
            tempMonster2.Position = new Vector2(32, 16);
            AddChild(tempMonster1);
            //AddChild(tempMonster2);
        }
    }

    private void OnSpawnMonstersEvent(SpawnMonstersEvent sme)
    { 
        //Clear the list of monsters already spawned for the prevoius level
    ClearMonsterList();

    GetMapLevelEvent gmle = new GetMapLevelEvent();
    gmle.callerClass = "MonsterManager - OnSpawnMonsterEvent";
    gmle.FireEvent();
    //Loop through the base amount of montsters and spawn as we go
    for (int i = 0; i < baseMonsterCount + (gmle.mapLevel * 1.25f); i++)
    {
        //Create a random number geneator
        RandomNumberGenerator rng = new RandomNumberGenerator();
        //Randomize the random number generator every time we spawn monsters for the level
        rng.Randomaize();
        if(gmle.mapLevel < 6)
        {
            int monsterInList = rng.RandiRange(0, gmle.mapLevel);
            Node monsterToSpawn = new monsterTypes[monsterInList];
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
            smte.monsterID = monsterToSpawn.GetInstanceID();
            smte.monsterType = (MonsterTypes)monsterInList;
            smte.FireEvent();

            DamageType damageType;

            switch (MonsterTypes)
            {
                case ((MonsterTypes)monsterInList).BEHOLDER:
                damageType = DamageType.ELECTRIC;
                break;
                case ((MonsterTypes)monsterInList).FIEND:
                damageType = DamageType.SLASH;
                break;
                case ((MonsterTypes)monsterInList).SLIME:
                damageType = DamageType.PIERCE;
                break;
                case ((MonsterTypes)monsterInList).FLAMING_SKULL:
                damageType = DamageType.FIRE;
                break;
                case ((MonsterTypes)monsterInList).GOBLIN:
                damageType = DamageType.SLASH;
                break;
                case ((MonsterTypes)monsterInList).SKELETON:
                damageType = DamageType.PIERCE;
                break;

            }

            //Set the monsters damage type
            //Set the monsters damage type
            SetDamageTypeEvent sdte = new SetDamageTypeEvent();
            sdte.callerClass = "MonsterManager - OnSpawnMonsterEvent";
            sdte.actorID = monsterToSpawn.GetInstanceID();
            sdte.damageType = damageType;//Get the type of monster and set the damage type acording
            sdte.FireEvent()
        }
    } 

    }

    private void ClearMonsterList()
    {
        //Loop through the list and delete the monster nodes
        foreach (Node monster in monsterList)
        {
            monster.queue_free();
        }
//After the nodes have been deleted we clear the list
        monsterList.Clear();
    }

    private void OnBroadcastTurnEvent(BroadcastTurnEvent bte)
    {
        //If the state manager is not on the enemies turn we return out of the method
        if (bte.states != TurnStates.ENEMY_TURN) return;

        foreach (Node monster in monsterNodes)
        {
            ((Monster)monster).Process();
        }

        //At the end of the monsters turn we cycle the turn
        CycleTurnEvent cte = new CycleTurnEvent();
        cte.callerClass = "Monster - _Process(float delta)";
        cte.FireEvent();
    }
}

