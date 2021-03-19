using Godot;
using System;
namespace EventCallback
{
    public class SetStatsEvent : Event<SetStatsEvent>
    {
        //The id of the en
        public ulong actorID;
        //The strength of the actor
        public int strength;
        //The dexterity of the actor
        public int dexterity;
        //The intelegence of the actor
        public int intelligence;
        //The level of the actor
        public int level;
        //The damage type of the actor
        public DamageType damageType;
        //The corruption of the actor
        public int corruption;
    }
}