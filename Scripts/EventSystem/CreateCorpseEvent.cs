

using Godot;
using System;
namespace EventCallback
{
    public class CreateCorpseEvent : Event<CreateCorpseEvent>
    {
        //The monster id for the corpse to be created
        public ulong monsterID;
    }
}