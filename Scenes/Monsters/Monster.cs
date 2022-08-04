using Godot;
using System;
using EventCallback;


public class Monster : KinematicBody2D
{
    [Export] PackedScene corpsScene = new PackedScene();
    Node corpsNode = new Node();
    public override void _Ready()
    {
        corpsNode = corpsScene.Instance();
    }
    public override void _ExitTree()
    {
        ((Node2D)corpsNode).GlobalPosition = GlobalPosition;
        GetParent().AddChild(corpsNode);
        // GetParent().CallDeferred("AddChild", corpsNode);
    }
}
