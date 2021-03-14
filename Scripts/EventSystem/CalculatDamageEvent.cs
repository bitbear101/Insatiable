using Godot;
using System;
namespace EventCallback
{
    public class CalculatDamageEvent : Event<CalculatDamageEvent>
    {
        //The id of the attacker
        public ulong attackerID;
        //The id of the target
        public ulong targetID;
        //The calculated damage
        public int damage;
    }
}