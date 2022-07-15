using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EventCallback;
public class MonsterMovement : Node
{
	//The directional ray
	RayCast2D LOSRay;
	//The view radius of the monster
	Area2D viewRadius;
	KinematicBody2D body;
	KinematicBody2D target = null;
	float speed = 250;
	float accel = 1000;
	float deccel = 1500;
	Vector2 vector = Vector2.Zero;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Get the kinematic body of hte monster
		body = GetNode<KinematicBody2D>("../../Slime");
		//Get the LOS ray for the monster
		LOSRay = GetNode<RayCast2D>("../LOSRay");
		//Get the view radius of the monster
		viewRadius = GetNode<Area2D>("../ViewRadius");
		//Connect the doby entered check;
		viewRadius.Connect("body_entered", this, nameof(OnBodyEntered));
		//Connect the doby entered check;
		viewRadius.Connect("area_entered", this, nameof(OnAreaEntered));
	}

	private void OnBodyEntered(KinematicBody2D body)
	{
		if (body.IsInGroup("Player")) target = body;
	}

	private void OnAreaEntered(Area2D area)
	{

	}

	private bool TargetInLOS()
	{
		//Add the los checks for the line of sight  
		if (target == null) return false;
		LOSRay.CastTo = target.GlobalPosition - body.GlobalPosition;
		LOSRay.ForceRaycastUpdate();
		if(LOSRay.IsColliding())
		{
			//If the ray does not collide with the player false is returned so we know we cannot see he player
			if(!((Node2D)LOSRay.GetCollider()).IsInGroup("Player")) return false;
		}
		return true;
	}

	private bool InRange()
	{
		if(body.GlobalPosition.DistanceTo(target.GlobalPosition) <= 25)
		{
			return true;
		}
		return false;
	}

	private Vector2 GetDirection()
	{
		//Set the target to zero
		Vector2 dir = Vector2.Zero;
		//Get the direction of the target if it is not null
		dir = target.GlobalPosition - body.GlobalPosition;
		//Return the normalized vector for the direction ot the target
		return dir.Normalized();
	}

	private void ApplyFriction(float amount)
	{
		if (vector.Length() > amount) vector -= vector.Normalized() * amount;
		else vector = Vector2.Zero;
	}

	private void ApplyForce(Vector2 amount)
	{
		vector += amount;
		vector = vector.LimitLength(speed);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		//If we don't have a target then return out of the function
		if (!TargetInLOS() || InRange()) return;

		Vector2 dir = GetDirection();

		if (dir == Vector2.Zero) ApplyFriction(deccel * delta);
		else ApplyForce(dir * accel * delta);

		vector = body.MoveAndSlide(vector);
	}
}
