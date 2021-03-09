using Godot;
using System;
namespace EventCallback
{
    public class EnemyMoveEvent : Event<EnemyMoveEvent>
    {
        //The id to identify which enemy needs to move
        public ulong enemyID;
    }
}