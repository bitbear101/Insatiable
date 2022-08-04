
using Godot;
using System;

namespace EventCallback
{
    public class ChangeVolumeEvent : Event<ChangeVolumeEvent>
    {
        public float soundVolume;
        public float musicVolume;
    }
}