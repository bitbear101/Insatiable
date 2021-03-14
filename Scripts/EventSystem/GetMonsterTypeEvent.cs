using Godot;
using System;
namespace EventCallback
{
    public class GetMonsterTypeEvent : Event<GetMonsterTypeEvent>
    {
        //The id of the monster that the tpe must be set of
        public ulong monsterID;
        //The new type of the monster 
        public MonsterTypes monsterType;
    }

}