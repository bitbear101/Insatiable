using Godot;
using System;
using EventCallback;

public class Stats : Node
{
    int strength;
    int dexterity;
    int intelegince;
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    private void OnGetDefenceEvent()
    {
//Check the id of the actor to get the defence of
    }

    private void OnGetAttackEvent()
    {
//Check the id of the attacker to calculate the damage of
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
