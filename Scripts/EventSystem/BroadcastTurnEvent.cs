using Godot;
using System;
namespace EventCallback
{
    public class BroadcastTurnEvent : Event<BroadcastTurnEvent>
    {
        public TurnStates states;
    }
}