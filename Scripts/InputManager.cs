using Godot;
using System;
using EventCallback;

public class InputManager : Node
{
    public override void _Ready()
    {
    }
    public override void _UnhandledInput(Godot.InputEvent @event)
    {
        if (@event is InputEventKey keyPress)
        {
            if (keyPress.Pressed)
            {
                //The move direction event messaging
                MoveDirectionEvent mde = new MoveDirectionEvent();
                mde.callerClass = "InputManager - _UnhandledInput()";
                if (keyPress.Scancode == (uint)KeyList.W || keyPress.Scancode == (uint)KeyList.Up)
                {
                    mde.dir = Vector2.Up;
                    mde.FireEvent();
                }
                else if (keyPress.Scancode == (uint)KeyList.A || keyPress.Scancode == (uint)KeyList.Left)
                {
                    mde.dir = Vector2.Left;
                    mde.FireEvent();
                }
                else if (keyPress.Scancode == (uint)KeyList.S || keyPress.Scancode == (uint)KeyList.Down)
                {
                    mde.dir = Vector2.Down;
                    mde.FireEvent();
                }
                else if (keyPress.Scancode == (uint)KeyList.D || keyPress.Scancode == (uint)KeyList.Right)
                {
                    mde.dir = Vector2.Right;
                    mde.FireEvent();
                }
                if (keyPress.Scancode == (uint)KeyList.E)
                {
                    ToggleStatsWindowEvent tswe = new ToggleStatsWindowEvent();
                    tswe.callerClass = "InputManager - _UnhandledInput()";
                    tswe.FireEvent();
                }
                if (keyPress.Scancode == (uint)KeyList.Escape)
                {
                    GetTree().Quit();
                }
            }
        }
    }
}
