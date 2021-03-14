using Godot;
using System;
namespace EventCallback
{
    public class GetStatsEvent : Node<GetStatsEvent>
    {
        //The id of the en
        public ulong corpseID;
        //The strength of the actor
        public int strenght;
        //The dexterity of the actor
        public int dexterity;
        //The intelegence of the actor
        public int intelligence;
        //The level of the actor
        public int level;
        //The damage type of the actor
        public DamageType damageType;
    }
}