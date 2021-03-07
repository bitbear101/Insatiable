using Godot;
using System;
namespace EventCallback
{
public class MoveDirectionEvent : Event<MoveDirectionEvent> 
{
    //The direction of travel
    public Vector2 dir;
}
}
