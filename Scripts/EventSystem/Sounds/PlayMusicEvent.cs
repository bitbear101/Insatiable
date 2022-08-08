using Godot;
using System;

namespace EventCallback
{
    public class PlayMusicEvent : Event<PlayMusicEvent>
    {
        public int music;
    }
}