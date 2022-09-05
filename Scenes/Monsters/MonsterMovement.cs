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
    KinematicBody2D myBody;
    KinematicBody2D target = null;
    float speed = 100;
    float accel = 1000;
    float deccel = 1500;
    Vector2 vector = Vector2.Zero;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
        //Get the kinematic body of hte monster
        myBody = GetTree().Root.GetNode("Main").GetNode<Node2D>("ViewportContainer/Viewport/MonsterManager/" + GetParent().Name) as KinematicBody2D;
        // myBody = GetNode<KinematicBody2D>("../../../../" + GetParent().Name);
        // GD.Print("../../../" + GetParent().Name);
        // myBody = GetNode<KinematicBody2D>("../../../Body");
        //Get the LOS ray for the monster
        LOSRay = GetNode<RayCast2D>("../LOSRay");
        //Get the view radius of the monster
        viewRadius = GetNode<Area2D>("../ViewRadius");
        //Connect the doby entered check;
        viewRadius.Connect("body_entered", this, nameof(OnBodyEntered));
        //Connect the doby entered check;
        viewRadius.Connect("body_exited", this, nameof(OnBodyExited));
        //Connect the doby entered check;
        viewRadius.Connect("area_entered", this, nameof(OnAreaEntered));
        //Connect the doby entered check;
        viewRadius.Connect("area_exited", this, nameof(OnAreaExited));
    }

    private void OnBodyEntered(KinematicBody2D body)
    {
        if (body.IsInGroup("Player")) target = body;
    }

    private void OnBodyExited(KinematicBody2D body)
    {
        if (body.IsInGroup("Player")) target = null;
    }

    private void OnAreaEntered(Area2D area)
    {

    }
    private void OnAreaExited(Area2D area)
    {

    }

    private bool TargetInLOS()
    {
        //Add the los checks for the line of sight  
        if (target == null) return false;
       LOSRay.CastTo = target.GlobalPosition - myBody.GlobalPosition; 
        LOSRay.ForceRaycastUpdate();
        if (LOSRay.IsColliding())
        {
            //If the ray does not collide with the player false is returned so we know we cannot see he player
            if (!((Node2D)LOSRay.GetCollider()).IsInGroup("Player")) return false;
        }
        return true;
    }

    private bool InRange()
    {
        if (myBody.GlobalPosition.DistanceTo(target.GlobalPosition) <= 25)
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
        dir = target.GlobalPosition - myBody.GlobalPosition;
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
        if (!TargetInLOS()) return;

        if (InRange())
        {
            EnemyAttackEvent eae = new EnemyAttackEvent();
            eae.callerClass = "MonsterMovement - _PhysicsProcess()";
            eae.attackerID = GetParent().GetInstanceId();
            eae.targetID = target.GetInstanceId();
            eae.FireEvent();
            return;
        }

        Vector2 dir = GetDirection();

        if (dir == Vector2.Zero) ApplyFriction(deccel * delta);
        else ApplyForce(dir * accel * delta);

        vector = myBody.MoveAndSlide(vector);
    }
}
