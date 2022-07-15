using Godot;
using System;
using EventCallback;
public class CameraManager : Camera2D
{
    Node2D player;
    Vector2 game_size = new Vector2(640, 360);
    float window_scale;
    Vector2 actual_cam_pos;
    ViewportContainer viewportContainer;
    Viewport viewport;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        viewportContainer = GetNode<ViewportContainer>("../../../ViewportContainer");
        viewport = GetNode<Viewport>("../../../ViewportContainer/Viewport");
        player = GetNode<Node2D>("../../../ViewportContainer/Viewport/Player") as Node2D;
        window_scale = (OS.WindowSize / game_size).x;
        actual_cam_pos = GlobalPosition;

        SetBBdata((int)BBKey.WINDOW_SCALE, window_scale);
        SetBBdata((int)BBKey.GAME_SIZE, game_size);
        SetBBdata((int)BBKey.VIEWPORT, viewport.GetInstanceId());
        SetBBdata((int)BBKey.VIEWPORT_CONTAINER, viewportContainer.GetInstanceId());
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector2 mouse_pos = viewport.GetMousePosition() / window_scale - (game_size / 2) + player.GlobalPosition;

        Vector2 cam_pos = player.GlobalPosition.LinearInterpolate(mouse_pos, 0.7f);

        actual_cam_pos = actual_cam_pos.LinearInterpolate(cam_pos, delta * 5);

        Vector2 subpixel_position = actual_cam_pos.Round() - actual_cam_pos;

        ((ShaderMaterial)viewportContainer.Material).SetShaderParam("cam_offset", subpixel_position);

        GlobalPosition = actual_cam_pos.Round();

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
