using Godot;
using System;
namespace EventCallback
{
    public class PlaySoundEvent : Event<PlaySoundEvent>
    {
        //Follow the target
        public bool follow = false;
        //The sound state that needs to be loaded
        public SoundStates sound;
        //The position of the sound to be played
        public ulong targetID;
        //The pitch of the sound
        public float pitch = 0f;
    }
}

