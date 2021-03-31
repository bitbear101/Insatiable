using Godot;
using System;
using EventCallback;
public class Menu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void OnStartButtonPressed()
    {
        //Send the start game event message
        StartGameEvent sge = new StartGameEvent();
        sge.callerClass = "Menu - OnStartGamePressed()";
        sge.FireEvent();
        //Set the menu to invisible
        Visible = false;
    }

    public void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}
