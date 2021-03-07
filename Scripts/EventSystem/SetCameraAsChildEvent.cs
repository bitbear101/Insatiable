using Godot;
using System;
namespace EventCallback
{
    public class SetCameraAsChildEvent : Event<SetCameraAsChildEvent>
    {
        //The new parent of the camera
        public Node newParent;
    }
}
