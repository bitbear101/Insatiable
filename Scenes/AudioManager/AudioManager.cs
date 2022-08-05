using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EventCallback;

public enum MusicList
{
    MENU,
    GAME,
    BATTLE,
    WIN,
    LOSE
};

public enum SFXList
{

};

public class AudioManager : Node2D
{
    AudioStreamPlayer musicPlayer;
    //The music volume once it has been lurped
    float lerpedMusicVolume;
    float musicVolume, soundVolume;
    bool fadeInMusic, fadeOutMusic, fadeOutMusicDone, changeMusicTrack;
    //Use the ID of the objects to keep track if it is in the openor closed list 
    List<ulong> openSFXPlayers = new List<ulong>();
    List<ulong> closedSFXPlayers = new List<ulong>();
    Dictionary<String, AudioStreamSample> musicList = new Dictionary<String, AudioStreamSample>();
    Dictionary<String, AudioStreamSample> soundsList = new Dictionary<String, AudioStreamSample>();

    String nextTrack;

    public override void _Ready()
    {
        //The audio stream players
        musicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");
        //Add 20 SFXPlayers to the scene
        for (int i = 0; i < 20; i++)
        {
            //Call the Asp2D function
            CreateSFXPlayers();
        }
        //Set the musicPlayer volume to the lowest setting
        musicPlayer.VolumeDb = -80;

        GetVolumeEvent gvei = new GetVolumeEvent();
        gvei.callerClass = "SoundManager - _Ready()";
        gvei.FireEvent();

        soundVolume = gvei.soundVolume;
        musicVolume = gvei.musicVolume;
        musicPlayer.VolumeDb = ConvertToDB(gvei.soundVolume);

        GetSoundFiles();

        //Register listeners for this class
        ChangeVolumeEvent.RegisterListener(OnChangeVolumeEvent);
        PlayMusicEvent.RegisterListener(OnPlayMusicEvent);
        PlaySoundEvent.RegisterListener(OnPlaySoundEvent);
    }
    public override void _Process(float delta)
    {
        if (fadeInMusic) FadeInMusic();
        if (fadeOutMusic) FadeOutMusic();

        if (changeMusicTrack)
        {
            if (fadeOutMusicDone)
            {
                musicPlayer.Stop();
                musicPlayer.Stream = musicList[nextTrack];
                musicPlayer.Play();
                fadeOutMusicDone = false;
                changeMusicTrack = false;
                fadeInMusic = true;
            }
        }
        //Check hte closed list of sound effects to check if they have finished playing
        foreach (ulong SFXPlayer in closedSFXPlayers)
        {
            if (!((AudioStreamPlayer)GD.InstanceFromId(SFXPlayer)).Playing)
            {
                //Copy the player id to the open list becuse it is done playing
                openSFXPlayers.Add(SFXPlayer);
                //Remove the players Id from the closed list as it is done playing
                closedSFXPlayers.Remove(SFXPlayer);
            }
        }
    }
    private void OnChangeVolumeEvent(ChangeVolumeEvent cvei)
    {
        musicPlayer.VolumeDb = ConvertToDB(cvei.musicVolume);
        musicVolume = cvei.musicVolume;
        soundVolume = cvei.soundVolume;
    }
    private void OnPlayMusicEvent(PlayMusicEvent pmei)
    {
        if (musicList.ContainsKey(pmei.music.ToString()))
        {
            if (musicPlayer.Playing)
            {
                fadeOutMusic = true;
                changeMusicTrack = true;
                fadeOutMusicDone = false;
                lerpedMusicVolume = musicVolume;
                nextTrack = pmei.music.ToString();
            }
            else
            {
                fadeInMusic = true;
                lerpedMusicVolume = 0;
                musicPlayer.Stream = musicList[pmei.music.ToString()];
                musicPlayer.Play();
            }
        }
        else
        {
            GD.Print("SoundManager - OnPlayMusicEvent = " + pmei.music.ToString() + " musicList key was NOT found");
        }

        //Fade out old music and fade in new music
    }
    private void FadeInMusic()
    {
        lerpedMusicVolume = Mathf.Lerp(lerpedMusicVolume, musicVolume, 0.4f);
        musicPlayer.VolumeDb = ConvertToDB(lerpedMusicVolume);

        if (lerpedMusicVolume > (musicVolume - 2.0f))
        {
            fadeInMusic = false;
            musicPlayer.VolumeDb = ConvertToDB(musicVolume);
        }
    }
    private void FadeOutMusic()
    {

        lerpedMusicVolume = Mathf.Lerp(lerpedMusicVolume, 0, 0.1f);
        musicPlayer.VolumeDb = ConvertToDB(lerpedMusicVolume);

        if (lerpedMusicVolume < 2.0f)
        {
            fadeOutMusic = false;
            fadeOutMusicDone = true;
            musicPlayer.VolumeDb = ConvertToDB(0);
        }

        //musicPlayer.VolumeDb = Mathf.Lerp(musicPlayer.VolumeDb, -80.0f, 0.5f);

        GD.Print("SoundManager - FadeOutMusic(): musicPlayer.VolumeDb = " + musicPlayer.VolumeDb);
        GD.Print("SoundManager - FadeOutMusic(): musicVolume = " + musicVolume);

        if (musicPlayer.VolumeDb < -78.0f)
        {
            fadeOutMusic = false;
            fadeOutMusicDone = true;
            musicPlayer.VolumeDb = -80;
        }
    }
    private void OnPlaySoundEvent(PlaySoundEvent psei)
    {
        //Check if the SFXPlayer list is empty, zero or less
        if (openSFXPlayers.Count <= 0)
        {
            //Create a new Audiostreamplayer if no more exist inside the openSoundPlayers list
            CreateSFXPlayers();
        }
        //Add the last entry in the openSounPlayers list to the closedSoundPlayer list
        closedSFXPlayers.Add(openSFXPlayers.Last());
        //Remove the link to the Asp2D struct fron the opeSoundPlayer list 
        openSFXPlayers.Remove(openSFXPlayers.Last());

        ((AudioStreamPlayer)GD.InstanceFromId(closedSFXPlayers[closedSFXPlayers.Count])).Stream = soundsList[psei.sound.ToString()];
        ((AudioStreamPlayer)GD.InstanceFromId(closedSFXPlayers[closedSFXPlayers.Count])).Play();
    }
    private void CreateSFXPlayers()
    {
        //Create a temporary Asp2D struct
        AudioStreamPlayer tempSFXPlayer = new AudioStreamPlayer();
        //Add a AudioStreamPlayer object Id to the list of open audio stream players list
        openSFXPlayers.Add(tempSFXPlayer.GetInstanceId());
        //Changed newly created AudioStreamPlayer2D names
        tempSFXPlayer.Name = "SFXPlayer" + openSFXPlayers.Count;
        //Set the volume of the AudioStreamPlayer2D
        tempSFXPlayer.VolumeDb = -80;
        //Add the AudioStreamPlayer2D as children in the stream
        AddChild(tempSFXPlayer);
    }
    private float ConvertToDB(float value)
    {
        return ((100 - value) * -80) / 100;
    }
    private void GetSoundFiles()
    {
        Directory dir = new Directory();

        if (dir.Open("res://Audio/Music/") == Error.Ok)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                if (fileName != "" && fileName != "." && fileName != ".." && !fileName.Contains(".import"))
                {
                    String name = fileName.Remove(fileName.Find(".wav"), fileName.Length - fileName.Find(".wav"));
                    AudioStreamSample tempAudio = ResourceLoader.Load<AudioStreamSample>("res://Sounds/Music/" + fileName) as AudioStreamSample;
                    musicList.Add(name, tempAudio);
                }
                fileName = dir.GetNext();
            }
        }

        if (dir.Open("res://Audio/SFX/ActorSounds/") == Error.Ok)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                if (fileName != "" && fileName != "." && fileName != ".." && !fileName.Contains(".import"))
                {
                    String name = fileName.Remove(fileName.Find(".wav"), fileName.Length - fileName.Find(".wav"));
                    AudioStreamSample tempAudio = ResourceLoader.Load<AudioStreamSample>("res://Sounds/ActorSounds/" + fileName) as AudioStreamSample;
                    soundsList.Add(name, tempAudio);
                }
                fileName = dir.GetNext();
            }
        }

        if (dir.Open("res://Audio/SFX/ObjectSounds/") == Error.Ok)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                if (fileName != "" && fileName != "." && fileName != ".." && !fileName.Contains(".import"))
                {
                    String name = fileName.Remove(fileName.Find(".wav"), fileName.Length - fileName.Find(".wav"));
                    AudioStreamSample tempAudio = ResourceLoader.Load<AudioStreamSample>("res://Sounds/ObjectSounds/" + fileName) as AudioStreamSample;
                    soundsList.Add(name, tempAudio);
                }
                fileName = dir.GetNext();
            }
        }
    }
    public override void _ExitTree()
    {
        ChangeVolumeEvent.UnregisterListener(OnChangeVolumeEvent);
        PlayMusicEvent.UnregisterListener(OnPlayMusicEvent);
        PlaySoundEvent.UnregisterListener(OnPlaySoundEvent);
    }
}