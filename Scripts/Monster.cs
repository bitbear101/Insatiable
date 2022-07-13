using Godot;
using System;
using EventCallback;

//The type of the monster
public enum MonsterTypes
{
    BEHOLDER,
    FIEND,
    SLIME,
    FLAMING_SKULL,
    GOBLIN,
    SKELETON
};
//The state of the enemy
public enum EnemyState
{
    MOVE,
    ATTACK
};

public class Monster : KinematicBody2D
{
    //The state of the enemy currently
    EnemyState currentState;
    //The type of the monster
    MonsterTypes type;

    //If it is the monsters turn
    bool myTurn = false;
    public override void _Ready()
    {
        //Set the enemies state
        SetEnemyStateEvent.RegisterListener(OnSetEnemyStateEvent);
        //Set the monsters type
        SetMonsterTypeEvent.RegisterListener(OnSetMonsterTypeEvent);
        //The listener for the get monster type event
        GetMonsterTypeEvent.RegisterListener(OnGetMonsterTypeEvent);
        //Set the start up state to move for the enemy
        currentState = EnemyState.MOVE;
    }

    //Will be used by the move and attack classes of the monster to change its internal states
    private void OnSetEnemyStateEvent(SetEnemyStateEvent sese)
    {
        if (sese.enemyID == GetInstanceId())
        {
            GD.Print("Monster - OnSetEnemyStateEvent = Id is the same changin state");
            //Set hte new state of the enemies behaviour
            currentState = sese.newState;
        }
    }

    private void OnSetMonsterTypeEvent(SetMonsterTypeEvent smte)
    {
        if (smte.monsterID == GetInstanceId())
        {
            type = smte.monsterType;
        }
    }

        private void OnGetMonsterTypeEvent(GetMonsterTypeEvent gmte)
    {
        if (gmte.monsterID == GetInstanceId())
        {
            gmte.monsterType = type;
        }
    }

    public void Process()
    {
        if (currentState == EnemyState.MOVE)
        {
            EnemyMoveEvent eme = new EnemyMoveEvent();
            eme.callerClass = "Monster - _Process(float delta)";
            eme.enemyID = GetInstanceId();
            eme.FireEvent();
        }
    }
    public override void _ExitTree()
    {
                //Set the enemies state
        SetEnemyStateEvent.UnregisterListener(OnSetEnemyStateEvent);
        //Set the monsters type
        SetMonsterTypeEvent.UnregisterListener(OnSetMonsterTypeEvent);
        //The listener for the get monster type event
        GetMonsterTypeEvent.UnregisterListener(OnGetMonsterTypeEvent);
    }
}
