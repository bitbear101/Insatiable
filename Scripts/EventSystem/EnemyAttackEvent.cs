using Godot;
using System;
namespace EventCallback
{
    public class EnemyAttackEvent : Event<EnemyAttackEvent>
    {
        //The target tot attack
        public ulong target;
        //The id to identify which enemy needs to attack
        public ulong enemyID;
    }
}
