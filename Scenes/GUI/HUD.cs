using Godot;
using System;
using EventCallback;
public class HUD : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        StartGameEvent.RegisterListener(OnStartGameEvent);
    }

    private void UpdateStats()
    {
        
    }

    private void OnStartGameEvent(StartGameEvent sge)
    {
        Visible = true;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
