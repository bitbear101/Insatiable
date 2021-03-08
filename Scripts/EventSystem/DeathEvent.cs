using Godot;
using System;
namespace EventCallback
{
    public class DeathEvent : Event<DeathEvent>
    {
        //The instance ID for the object
        public ulong targetID;
    }
}