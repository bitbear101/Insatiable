using Godot;
using System;
namespace EventCallback
{
    public class SetCorpseStatsEvent : Event<SetCorpseStatsEvent>
    {
        public int strength;
        public int dexterity;
        public int intelligence;
        public int corruption;
    }

}
