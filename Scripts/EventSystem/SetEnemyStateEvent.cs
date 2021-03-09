using Godot;
using System;
namespace EventCallback
{
    public class SetEnemyStateEvent : Event<SetEnemyStateEvent>
    {
        //The id of the enemy which the state needs to change
        public ulong enemyID;
        //The new state of the enemy
        public EnemyState newState;
    }
}