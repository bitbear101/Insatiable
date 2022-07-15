using Godot;
using System;
using System.Collections.Generic;
using EventCallback;
public enum BBKey
{
    MOUSE_POSITION,
    VIEWPORT,
    VIEWPORT_CONTAINER,
    WINDOW_SCALE,
    GAME_SIZE
};
public class Blackboard : Node
{
    Dictionary<BBKey, object> BBData = new Dictionary<BBKey, object>();

    public override void _Ready()
    {
        GetBBDataEvent.RegisterListener(OnGetBBDataEvent);
    }

    private void OnSetBBDataEvent(SetBBDataEvent sbbde)
    {
        BBData.Add((BBKey)sbbde.key, sbbde.data);
    }
    private void OnGetBBDataEvent(GetBBDataEvent gbbde)
    {
        gbbde.data = BBData[(BBKey)gbbde.key];
    }
}
