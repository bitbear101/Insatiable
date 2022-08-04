using Godot;
using System;

namespace EventCallback
{
    public class SoundFinishedEvent : Event<SoundFinishedEvent>
    {
        //The id of the sound that finnished plying
        public ulong soundID;
    }
}
