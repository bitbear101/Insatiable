using Godot;
using System;
using EventCallback;
public class PlayerFlipSprite : Sprite
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector2 mouse_pos = ((Viewport)GD.InstanceFromId((ulong)(GetBBData((int)BBKey.VIEWPORT)))).GetMousePosition() / ((float)GetBBData((int)BBKey.WINDOW_SCALE)) - (((Vector2)GetBBData((int)BBKey.GAME_SIZE)) / 2) + GlobalPosition;
        //Flip the Image to face the the mouses left or right from the prite
        // if (GetGlobalMousePosition().x < GlobalPosition.x)
        if (mouse_pos.x < GlobalPosition.x)
        {
            FlipH = true;
        }
        else
        {
            FlipH = false;
        }
    }

    private object GetBBData(int key)
    {
        GetBBDataEvent gbbde = new GetBBDataEvent();
        gbbde.callerClass = "CameraManager - GetBBData()";
        gbbde.key = key;
        gbbde.FireEvent();
        return gbbde.data;
    }

    private void SetBBdata(int key, object data)
    {
        SetBBDataEvent sbbde = new SetBBDataEvent();
        sbbde.callerClass = "CameraManager - GetBBData()";
        sbbde.key = key;
        sbbde.data = data;
        sbbde.FireEvent();
    }
}
