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
        //The listener for the turn managers state
        BroadcastTurnEvent.RegisterListener(OnBroadcastTurnEvent);
        //Set the enemies state
        SetEnemyStateEvent.RegisterListener(OnSetEnemyStateEvent);
        //Set the start up state to move for the enemy
        currentState = EnemyState.MOVE;
    }

    private void OnBroadcastTurnEvent(BroadcastTurnEvent bte)
    {
        //If the state manager is on the enemies turn we set the local myTrun bool to true to preform a move
        if (bte.states == TurnStates.ENEMY_TURN) myTurn = true;
    }
    //Will be used by the move and attack classes of the monster to change its internal states
    private void OnSetEnemyStateEvent(SetEnemyStateEvent sese)
    {
        if (sese.enemyID == GetParent().GetInstanceId())
        {
            //Set hte new state of the enemies behaviour
            currentState = sese.newState;
        }
    }

    public override void _Process(float delta)
    {
        //If the my turn is false we just return out of the process method
        if (!myTurn) return;

        if (currentState == EnemyState.MOVE)
        {
            EnemyMoveEvent eme = new EnemyMoveEvent();
            eme.callerClass = "Monster - _Process(float delta)";
            eme.enemyID = GetParent().GetInstanceId();
            eme.FireEvent();
        }
        else if (currentState == EnemyState.ATTACK)
        {
            EnemyAttackEvent eae = new EnemyAttackEvent();
            eae.callerClass = "Monster - _Process(float delta)";
            eae.enemyID = GetParent().GetInstanceId();
            eae.FireEvent();
        }
        //At the end of the monsters turn we set its myTurn to false or else it will continue to move in the loop
        myTurn = false;

        //At the end of the monsters turn we cycle the turn
        CycleTurnEvent cte = new CycleTurnEvent();
        cte.callerClass = "Monster - _Process(float delta)";
        cte.FireEvent();
    }
}
