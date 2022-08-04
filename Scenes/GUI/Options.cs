using Godot;
using System;
using EventCallback;
public class Options: Control
{
    Slider musicSlider;
    Slider soundSlider;
    public override void _Ready()
    {
        musicSlider = GetNode<Slider>("NinePatchRect/Options/Controls/MusicVolume");
        musicSlider.Connect("value_changed",this,nameof(OnMusicVolumeValueChanged));
        soundSlider = GetNode<Slider>("NinePatchRect/Options/Controls/SoundVolume");
        soundSlider.Connect("value_changed",this,nameof(OnSoundVolumeValueChanged));
        GetVolumeEvent.RegisterListener(OnGetVolumeEvent);
    }
    public void OnMusicVolumeValueChanged(float value)
    {
        ChangeVolumeEvent cvei = new ChangeVolumeEvent();
        cvei.callerClass = "OptionsScreen: OnMusicVolumeValueChanged()";
        cvei.musicVolume = value;
        cvei.FireEvent();
    }

    public void OnSoundVolumeValueChanged(float value)
    {
        ChangeVolumeEvent cvei = new ChangeVolumeEvent();
        cvei.callerClass = "OptionsScreen: OnSoundVolumeValueChanged()";
        cvei.soundVolume = value;
        cvei.FireEvent();
    }

    private void OnGetVolumeEvent(GetVolumeEvent gvei)
    {
        gvei.musicVolume = (float)(musicSlider.Value);
        gvei.soundVolume = (float)(soundSlider.Value);
    }
}
