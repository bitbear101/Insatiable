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
    List<Node> monsterNodes = new List<Node>();

    public override void _Ready()
    {
        //The listener for the turn managers state
        BroadcastTurnEvent.RegisterListener(OnBroadcastTurnEvent);
        //Check if the main scenes list is not zero 
        if (monsterScenes.Count > 0)
        {
            //Loop through all the scenes in the list
            foreach (PackedScene scene in monsterScenes)
            {
                //Add the node of the scenes
                monsterNodes.Add(scene.Instance());
                monsterNodes.Add(scene.Instance());
            }
            //Loop through the list of scene nodes and add them to the current scene as children
            // foreach (Node monster in monsterNodes)
            // { 
            //     ((Node2D)monster).Position = new Vector2(16,16);
            //     AddChild(monster);
            // }
            Node2D tempMonster1 = ((Node2D)monsterNodes[0]);
            tempMonster1.Position = new Vector2(16, 16);
            Node2D tempMonster2 = ((Node2D)monsterNodes[1]);
            tempMonster2.Position = new Vector2(32, 16);
            AddChild(tempMonster1);
            //AddChild(tempMonster2);
        }
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

