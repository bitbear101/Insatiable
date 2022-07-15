using Godot;
using System;
using EventCallback;
public class Player : KinematicBody2D
{
	//The stats of the player
	Stats stats = new Stats();
	//The health for the player
	Health health = new Health();
	[Export] PackedScene punchScene = new PackedScene();
	Node punch = new Node();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Set the LOS layer as a child of the pplayer
		SetLOSAsChildEvent slosace = new SetLOSAsChildEvent();
		slosace.callerClass = "Player - _Ready()";
		slosace.newParent = this;
		slosace.FireEvent();
		//Get the position where the player must spawn in
		GetPlayerSpawnPointEvent gpspe = new GetPlayerSpawnPointEvent();
		gpspe.callerClass = "Player - _Ready()";
		gpspe.FireEvent();
		Position = gpspe.spawnPos;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("attack"))
		{
            punch = punchScene.Instance();
			AddChild(punch);
		}
	}
}
