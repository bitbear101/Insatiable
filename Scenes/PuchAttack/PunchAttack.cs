using Godot;
using System;

public class PunchAttack : Node2D
{
    Vector2 target;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Get the parents global positioning before setting it a stop level
        Vector2 parentPos = ((Node2D)GetParent()).GlobalPosition;
        //Set the punch as a top level as not to move with player anymore
        SetAsToplevel(true);
        //Set the position of the punch again as it is reset with the set as top level
        GlobalPosition = parentPos;
        // Set the direction for the hit
        LookAt(GetGlobalMousePosition());
        // Set the taret for the hit on creation
        target = GlobalPosition + ((GetGlobalMousePosition() - GlobalPosition).Normalized() * 50);
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
}
