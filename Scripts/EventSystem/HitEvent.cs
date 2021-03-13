using Godot;
using System;
namespace EventCallback
{
    public class HitEvent : Event<HitEvent>
    {
        //The actor doing the attacking
        public ulong attackerID;
        //The target that is hit
        public ulong targetID;
    }
}