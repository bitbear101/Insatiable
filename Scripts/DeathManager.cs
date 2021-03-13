using Godot;
using System;
using EventCallback;
public class DeathManager : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        DeathEvent.RegisterListener(OnDeathEvent);
    }

    private void OnDeathEvent(DeathEvent de)
    {
        //Get the id of hte actor dying and if its a monster generate corpse
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
