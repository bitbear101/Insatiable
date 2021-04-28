using Godot;
using System;

namespace EventCallback
{
    public class FloatingTextEvent : Event<FloatingTextEvent>
    {
        //The start position for the event
        public Vector2 position;
        //The text to display for the floating text
        public string textToDisplay;
    }
}

