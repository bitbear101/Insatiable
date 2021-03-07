using Godot;
using System;
namespace EventCallback
{
    public class HitEvent : Event<HitEvent>
    {
        //The target that is hit
        public Node2D target;
    }
}