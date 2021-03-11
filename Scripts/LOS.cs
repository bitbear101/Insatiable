using Godot;
using System;
using EventCallback;
public class LOS : CanvasModulate
{
    public override void _Ready()
    {
        SetLOSAsChildEvent.RegisterListener(OnSetLOSAsChildEvent);
    }

    private void OnSetLOSAsChildEvent(SetLOSAsChildEvent slosace)
    {
        //Remove the camera from the its current parent
        GetParent().RemoveChild(this);
        //Set the sender of the message as the parent of the camera
        slosace.newParent.AddChild(this);
    }
}
