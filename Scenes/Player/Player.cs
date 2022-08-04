using Godot;
using System;
using EventCallback;
public class Player : KinematicBody2D
{
	[Export] PackedScene punchScene = new PackedScene();
	Node punch = new Node();

	public override void _Ready()
	{
		GetPlayerSpawnPointEvent gpsp = new GetPlayerSpawnPointEvent();
		gpsp.callerClass = "Player";
		gpsp.FireEvent();
		//Set the spawn position for hte player
		GlobalPosition = gpsp.spawnPos;
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
