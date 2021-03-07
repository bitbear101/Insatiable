using Godot;
using System;
using EventCallback;
public class Player : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Set hte camera as the child of the player
        SetCameraAsChildEvent scace = new SetCameraAsChildEvent();
        scace.newParent = this;
        scace.FireEvent();
        //Get the position where the player must spawn in
        GetPlayerSpawnPointEvent gpspe = new GetPlayerSpawnPointEvent();
        gpspe.callerClass = "Player - _Ready()";
        gpspe.FireEvent();
        Position = gpspe.spawnPos;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
