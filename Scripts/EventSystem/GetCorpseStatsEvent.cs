using Godot;
using System;
namespace EventCallback
{
    public class GetCorpseStatsEvent : Event<GetCorpseStatsEvent>
    {
        public int strength;
        public int dexterity;
        public int intelligence;
        public int corruption;
    }

}
