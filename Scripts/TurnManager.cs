using Godot;
using System;
using EventCallback;
public enum TurnStates
{
    PLAYER_TURN,
    ENEMY_TURN,
};
public class TurnManager : Node
{
    //The states for the turn manager to loop through
    TurnStates currentState;

    public override void _Ready()
    {
        //Set hte defualt state for the turn manager to the players turn
        currentState = TurnStates.PLAYER_TURN;
        //The listener for the cycling of the turn
        CycleTurnEvent.RegisterListener(OnCycleTurnEvent);
    }

    public void OnCycleTurnEvent(CycleTurnEvent cte)
    {
        //Set the new state
        currentState = (TurnStates)(((int)currentState + 1) % 2);
        //Broadcasts the new state to all listeners =================
        BroadcastTurnEvent bte = new BroadcastTurnEvent();
        bte.callerClass = "TurnManager - OnChangeState()";
        bte.states = currentState;
        bte.FireEvent();
        //===========================================================
    }
}
