using Godot;
using System;
namespace EventCallback
{
    public class GetCorpseStatsEvent : Event<GetCorpseStatsEvent>
    {
        public ulong corpseID;
        public int strength;
        public int dexterity;
        public int intelligence;
        public int corruption;
    }

}
