using Godot;
using System;
using EventCallback;
public class Options : Control
{
    Slider musicSlider;
    Slider soundSlider;
    public override void _Ready()
    {
        //Connect the mgodot messaging for the change of values on the sliders in the options menu 
        musicSlider = GetNode<Slider>("VBoxContainer/NinePatchRect/Options/Controls/MusicVolume");
        soundSlider = GetNode<Slider>("VBoxContainer/NinePatchRect/Options/Controls/SoundVolume");

        musicSlider.Connect("value_changed", this, nameof(OnMusicVolumeValueChanged));

        soundSlider.Connect("value_changed", this, nameof(OnSoundVolumeValueChanged));

        Connect("visibility_changed", this, nameof(OnVisibilityChanged));
    }
    public void OnMusicVolumeValueChanged(float value)
    {
        ChangeVolumeEvent cvei = new ChangeVolumeEvent();
        cvei.callerClass = "OptionsScreen: OnMusicVolumeValueChanged()";
        cvei.musicVolume = value;
        cvei.soundVolume = -1;
        cvei.FireEvent();
    }

    public void OnSoundVolumeValueChanged(float value)
    {
        ChangeVolumeEvent cvei = new ChangeVolumeEvent();
        cvei.callerClass = "OptionsScreen: OnSoundVolumeValueChanged()";
        cvei.musicVolume = -1;
        cvei.soundVolume = value;
        cvei.FireEvent();
    }
    private void OnVisibilityChanged()
    {
        if (!Visible) return;
        GetVolumeEvent gvei = new GetVolumeEvent();
        gvei.callerClass = "Options - OnVisibilityChanged()";
        gvei.FireEvent();
        musicSlider.Value = gvei.musicVolume;
        soundSlider.Value = gvei.soundVolume;
    }
}
