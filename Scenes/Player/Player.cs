using Godot;
using System;
using EventCallback;
public class Player : KinematicBody2D
{
	[Export] PackedScene punchScene = new PackedScene();
	Node punch = new Node();
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
