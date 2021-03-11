using Godot;
using System;
using EventCallback;

//The type of the monster
public enum MonsterType
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

public class Monster : Node2D
{
    //The state of the enemy currently
    EnemyState currentState;
    //The type of the monster
    MonsterType type;
    //If it is the monsters turn
    bool myTurn = false;
    public override void _Ready()
    {
        //Set the enemies state
        SetEnemyStateEvent.RegisterListener(OnSetEnemyStateEvent);
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

    public void Process()
    {
        if (currentState == EnemyState.MOVE)
        {
            EnemyMoveEvent eme = new EnemyMoveEvent();
            eme.callerClass = "Monster - _Process(float delta)";
            eme.enemyID = GetInstanceId();
            eme.FireEvent();
        }
        else if (currentState == EnemyState.ATTACK)
        {
            GD.Print("Monster attacking");
            EnemyAttackEvent eae = new EnemyAttackEvent();
            eae.callerClass = "Monster - _Process(float delta)";
            eae.enemyID = GetInstanceId();
            eae.FireEvent();
        }
    }
}
