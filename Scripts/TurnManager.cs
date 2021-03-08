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
    TurnStates state;

    public override void _Ready()
    {
        //Set hte defualt state for the turn manager to the players turn
        state = TurnStates.PLAYER_TURN;
    }

    public void OnChangeState(TurnStates newState)
    {
        //Set the new state
        state = newState;
        //Broadcasts the new state to all listeners =================
        BroadcastTurnEvent bte = new BroadcastTurnEvent();
        bte.callerClass = "TurnManager - OnChangeState()";
        bte.states = state;
        bte.FireEvent();
        //===========================================================
    }
}
