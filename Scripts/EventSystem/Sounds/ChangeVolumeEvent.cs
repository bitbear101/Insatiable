
using Godot;
using System;

namespace EventCallback
{
    public class ChangeVolumeEvent : Event<ChangeVolumeEvent>
    {
        public float soundVolume = -1;
        public float musicVolume = -1;
    }
}