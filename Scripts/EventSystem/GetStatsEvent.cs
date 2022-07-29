using Godot;
using System;
namespace EventCallback
{
    public class GetStatsEvent : Event<GetStatsEvent>
    {
        //The id of the en
        public ulong actorID;
        //The strength of the actor
        public float strength;
        //The dexterity of the actor
        public float dexterity;
        //The intelegence of the actor
        public float intelligence;
        //The level of the actor
        public int level;
        //The coruption of the stats
        public float corruption;
        //The damage type of the actor
        public DamageType damageType;

    }
}