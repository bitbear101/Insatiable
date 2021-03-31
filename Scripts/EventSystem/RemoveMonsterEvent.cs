using Godot;
using System;
namespace EventCallback
{
    public class RemoveMonsterEvent : Event<RemoveMonsterEvent>
    {
        public ulong monsterID;
    }
}