using Godot;
using System;
namespace EventCallback
{
    public class GetVolumeEvent : Event<GetVolumeEvent>
    {
        public float soundVolume;
        public float musicVolume;
    }
}