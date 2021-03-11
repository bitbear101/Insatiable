using Godot;
using System;
namespace EventCallback
{
    public class SetLOSAsChildEvent : Event<SetLOSAsChildEvent>
    {
        public Node2D newParent;
    }
}