using Godot;
using System;
using EventCallback;
public class Movement : Node
{
	KinematicBody2D body;
	float speed = 150;
	float accel = 1500;
	float deccel = 1500;

	Vector2 vector = Vector2.Zero;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		body = GetNode<KinematicBody2D>("../../Player");
	}

	private Vector2 GetDirection()
	{
		Vector2 dir = Vector2.Zero;

		dir.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		dir.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

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
		Vector2 dir = GetDirection();

		if (dir == Vector2.Zero) ApplyFriction(deccel * delta);
		else ApplyForce(dir * accel * delta);

		vector = body.MoveAndSlide(vector);
	}
}
