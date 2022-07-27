using Godot;
using System;
using EventCallback;
public class PunchAttack : Node2D
{
	//The travel target for the attack
	Vector2 target;
	//The aatacks parent ID
	ulong parentID;
	//The hit box of the attack
	Area2D hitArea;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Get the ID of the parent
		parentID = GetParent().GetInstanceId();
		//Get the parents global positioning before setting it a stop level
		Vector2 parentPos = ((Node2D)GetParent()).GlobalPosition;
		//Set the punch as a top level as not to move with player anymore
		SetAsToplevel(true);
		//Set the hit area in script to the on instanced in the scene
		hitArea = GetNode<Area2D>("HitArea");
		//Set the connection for collision detection
		hitArea.Connect("area_entered", this, nameof(OnAreaEntered));
		//Set the position of the punch again as it is reset with the set as top level
		GlobalPosition = parentPos;
		//Get the modified mouse pos for the viewport
		Vector2 mouse_pos = ((Viewport)GD.InstanceFromId((ulong)(GetBBData((int)BBKey.VIEWPORT)))).GetMousePosition() / ((float)GetBBData((int)BBKey.WINDOW_SCALE)) - (((Vector2)GetBBData((int)BBKey.GAME_SIZE)) / 2) + GlobalPosition;
		// Set the direction for the hit
		LookAt(mouse_pos);
		// Set the taret for the hit on creation
		target = GlobalPosition + ((mouse_pos - GlobalPosition).Normalized() * 50);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		GlobalPosition = GlobalPosition.LinearInterpolate(target, .1f);

		if (GlobalPosition.DistanceTo(target) < 1)
		{
			QueueFree();
		}
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area.GetParent().IsInGroup("Monster"))
		{
			HitEvent he = new HitEvent();
			he.callerClass = "PunchAttack - OnAreaEntered()";
			he.attackerID = parentID;
			he.targetID = area.GetParent().GetInstanceId();
			he.FireEvent();

		}
		//Free the punch attack scene
		QueueFree();
	}

	private object GetBBData(int key)
	{
		GetBBDataEvent gbbde = new GetBBDataEvent();
		gbbde.callerClass = "CameraManager - GetBBData()";
		gbbde.key = key;
		gbbde.FireEvent();
		return gbbde.data;
	}

	private void SetBBdata(int key, object data)
	{
		SetBBDataEvent sbbde = new SetBBDataEvent();
		sbbde.callerClass = "CameraManager - GetBBData()";
		sbbde.key = key;
		sbbde.data = data;
		sbbde.FireEvent();
	}
}
