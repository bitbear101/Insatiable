using Godot;
using System;
using EventCallback;
public class Menu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PlayMusicEvent pme = new PlayMusicEvent();
        pme.callerClass = "Menu - _Ready()";
        pme.music = (int)MusicList.MENU;
        pme.FireEvent();
    }
    public void OnStartButtonPressed()
    {
        //Play the button click sound
        PlaySoundEvent pse = new PlaySoundEvent();
        pse.callerClass = "Menu - _Ready()";
        pse.sound = (int)SFXList.BUTTON_CLICK;
        pse.FireEvent();
        //Play the in  game music
        PlayMusicEvent pme = new PlayMusicEvent();
        pme.callerClass = "Menu - OnStartButtonPressed()";
        pme.music = (int)MusicList.DARK_CAVE_AMBIENT;
        pme.FireEvent();
        //Send the start game event message
        StartGameEvent sge = new StartGameEvent();
        sge.callerClass = "Menu - OnStartGamePressed()";
        sge.FireEvent();
        //Set the menu to invisible
        Visible = false;
    }
    public void OnOptionsButtonPressed()
    {
        GetNode<Control>("../Options").Visible = !GetNode<Control>("../Options").Visible;
        //Play the button click sound
        PlaySoundEvent pse = new PlaySoundEvent();
        pse.callerClass = "Menu - _Ready()";
        pse.sound = (int)SFXList.BUTTON_CLICK;
        pse.FireEvent();
    }
    public void OnExitButtonPressed()
    {
        //Play the button click sound
        PlaySoundEvent pse = new PlaySoundEvent();
        pse.callerClass = "Menu - _Ready()";
        pse.sound = (int)SFXList.BUTTON_CLICK;
        pse.FireEvent();
        GetTree().Quit();
    }
}
