using Godot;
using System;
using EventCallback;
public class CameraManager : Camera2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetCameraAsChildEvent.RegisterListener(OnSetCameraAsChildEvent);

    }

    private void OnSetCameraAsChildEvent(SetCameraAsChildEvent scace)
    {
        //Remove the camera from the its current parent
        GetParent().RemoveChild(this);
        //Set the sender of the message as the parent of the camera
        scace.newParent.AddChild(this);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
