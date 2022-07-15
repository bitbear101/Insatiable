using Godot;
using System;

public class Hit : Node
{
    Area2D hitbox;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Get the kinematic body of hte monster
        hitbox = GetNode<Area2D>("../HitBox");
        hitbox.Connect("area_entered", this, nameof(OnAreaEntered));
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area.GetParent().IsInGroup("PlayerAttacks"))
        {
            area.GetParent().QueueFree();
            GetParent().QueueFree();
        }
    }
}
