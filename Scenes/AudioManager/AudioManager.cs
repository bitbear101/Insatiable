using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EventCallback;

public enum MusicList
{
    MENU,
    GAME,
    WIN,
    LOSE
};

public enum EnviromentSFX
{

};

public enum ActorSFX
{
    
};

public struct Asp2D
{
    public AudioStreamPlayer2D audioStreamPlayer2D;
    public bool follow;
    public bool repeat;
    public ulong targetID;
}

public class AudioManager : Node2D
{
    //The lists of music
    [Export] List<AudioStream> menuMusic = new List<AudioStream>();
    [Export] List<AudioStream> gameMusic = new List<AudioStream>();
    //The lists of sound effects
    [Export] List<AudioStream> effect = new List<AudioStream>();

    AudioStreamPlayer musicPlayer;
    AudioStreamPlayer effectsPlayer;

        //A temporary Asp2D used to set when a sound needs to be played
    Asp2D tempAsp2D;
    //The music volume once it has been lurped
    float lerpedMusicVolume;
    float musicVolume, soundVolume;
    bool fadeInMusic, fadeOutMusic, fadeOutMusicDone, changeMusicTrack;
    List<Asp2D> openSoundPlayers = new List<Asp2D>();
    List<Asp2D> closedSoundPlayers = new List<Asp2D>();
    Dictionary<String, AudioStreamSample> musicList = new Dictionary<String, AudioStreamSample>();
    Dictionary<String, AudioStreamSample> soundsList = new Dictionary<String, AudioStreamSample>();

    String nextTrack;

    public override void _Ready()
    {
        musicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");
        effectsPlayer = GetNode<AudioStreamPlayer>("SFXPlayer");
    }

    private void OnPlayeSound(int soundID)
    {

    }

    private void PlayMusic(int musicID)
    {
        
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Register listeners for this class
        ChangeVolumeEvent.RegisterListener(OnChangeVolumeEvent);
        PlayMusicEvent.RegisterListener(OnPlayMusicEvent);
        PlaySoundEvent.RegisterListener(OnPlaySoundEvent);
        musicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");

        //Add 20 Asp2D structs to the scene
        for (int i = 0; i < 20; i++)
        {
            //Call the Asp2D function
            CreateAsp2D();
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
    }
    public override void _Process(float delta)
    {
        UpdateSoundPosition();
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
    }

    private void UpdateSoundPosition()
    {
        foreach (Asp2D audioPlayer in closedSoundPlayers)
        {
            if (audioPlayer.audioStreamPlayer2D.Playing)
            {
                audioPlayer.audioStreamPlayer2D.Position = ((Node2D)(GD.InstanceFromId(audioPlayer.targetID))).Position;
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
        //Check if the Asp2D struct list is empty, zero or less
        if (openSoundPlayers.Count <= 0)
        {
            //Create a new Asp2D if no more exist inside the openSoundPlayers list
            CreateAsp2D();
        }
        //Add the last entry in the openSounPlayers list to the closedSoundPlayer list
        closedSoundPlayers.Add(openSoundPlayers.Last());
        //Remove the link to the Asp2D struct fron the opeSoundPlayer list 
        openSoundPlayers.Remove(openSoundPlayers.Last());

        tempAsp2D = closedSoundPlayers[closedSoundPlayers.Count];
        tempAsp2D.follow = psei.follow;
        tempAsp2D.targetID = psei.targetID;
        tempAsp2D.audioStreamPlayer2D.Stream = soundsList[psei.sound.ToString()];
        closedSoundPlayers[closedSoundPlayers.Count] = tempAsp2D;

        closedSoundPlayers[closedSoundPlayers.Count].audioStreamPlayer2D.Play();
    }

    private void CreateAsp2D()
    {
        //Create a temporary Asp2D struct
        Asp2D tempAsp2D = new Asp2D();
        //Add a new AudioStreamPlayer to the tempAsp2D struct
        tempAsp2D.audioStreamPlayer2D = new AudioStreamPlayer2D();
        //Add a Asp2D struct in memory but not instanced into the scene yet
        openSoundPlayers.Add(tempAsp2D);
        //Changed newly created AudioStreamPlayer2D names
        openSoundPlayers.Last().audioStreamPlayer2D.Name = "ASP2D" + openSoundPlayers.Count;
        //Set the volume of the AudioStreamPlayer2D
        openSoundPlayers.Last().audioStreamPlayer2D.VolumeDb = -80;

        openSoundPlayers.Last().audioStreamPlayer2D.Connect("finished", this, nameof)
        
        openSoundPlayers.Last().audioStreamPlayer2D.AddChild()
        Connect()
        //Add the AudioStreamPlayer2D as children in the stream
        AddChild(openSoundPlayers.Last().audioStreamPlayer2D);
    }



    private float ConvertToDB(float value)
    {
        return ((100 - value) * -80) / 100;
    }

    private void GetSoundFiles()
    {
        Directory dir = new Directory();

        if (dir.Open("res://Sounds/Music/") == Error.Ok)
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

        if (dir.Open("res://Sounds/ActorSounds/") == Error.Ok)
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

        if (dir.Open("res://Sounds/ObjectSounds/") == Error.Ok)
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
        base._ExitTree();
        ChangeVolumeEvent.UnregisterListener(OnChangeVolumeEvent);
        PlayMusicEvent.UnregisterListener(OnPlayMusicEvent);
        PlaySoundEvent.UnregisterListener(OnPlaySoundEvent);
    }

}