using Godot;
using System;
namespace EventCallback
{
    public class SetStatsEvent : Event<SetStatsEvent>
    {
        //The id of the en
        public ulong actorID;
        //Set the experience of the actor
        public float experience;
        //The strength of the actor
        public float strength;
        //The dexterity of the actor
        public float dexterity;
        //The intelegence of the actor
        public float intelligence;
        //The corruption of the actor
        public float corruption;
        //The damage type of the actor
        public DamageType damageType;

    }
}