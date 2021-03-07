using Godot;
using System;

public enum TurnStates
{
    PLAYER_TURN,
    PARTY_MOVE,
    CONFLICT_RESOLUTION,
    CALCULATE_RESOURCES
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

    public override void _Process(float delta)
    {
        base._Process(delta);
        switch (state)
        {
            case TurnStates.PLAYER_TURN:
                break;
            case TurnStates.PARTY_MOVE:
                //Loop through all the parties and move those with the move flag status set
                break;
            case TurnStates.CONFLICT_RESOLUTION:
                break;
            case TurnStates.CALCULATE_RESOURCES:
                break;
        }
    }

    public void OnChangeState(TurnStates newState)
    {
        //Set the new state
        state = newState;
    }
}
