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
    DARK_CAVE_AMBIENT,
    LOSE
};

public enum SFXList
{
    BUTTON_CLICK,
    GRAVEL_FOOTSTEP,
    ROCK_SMASH,
    ROCK_SMASH1,
    ROCK_SMASH2,
    ROCK_SMASH3
};

public class AudioManager : Node2D
{
    AudioStreamPlayer musicPlayer;
    //The music volume once it has been lurped
    float lerpedMusicVolume;
    //The volume levels are stored in Desibels value so -80 (0% sound) to 0 (100% sound) 
    float musicVolume, soundVolume;
    bool fadeInMusic = false, fadeOutMusic = false, fadeOutMusicDone = false, changeMusicTrack = false;
    //Use the ID of the objects to keep track if it is in the openor closed list 
    List<ulong> openSFXPlayers = new List<ulong>();
    List<ulong> closedSFXPlayers = new List<ulong>();
    Dictionary<String, AudioStream> musicDic = new Dictionary<String, AudioStream>();
    Dictionary<String, AudioStream> sfxDic = new Dictionary<String, AudioStream>();

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
        //Set the sound player volume
        musicVolume = 55;
        //Set the musicPlayer volume to the lowest setting
        musicPlayer.VolumeDb = ConvertToDB(musicVolume);
        //Set the sound volume
        soundVolume = 70;
        //Get the sound files from the audio directory
        GetSoundFiles();

        //Register listeners for this class
        ChangeVolumeEvent.RegisterListener(OnChangeVolumeEvent);
        PlayMusicEvent.RegisterListener(OnPlayMusicEvent);
        PlaySoundEvent.RegisterListener(OnPlaySoundEvent);
        GetVolumeEvent.RegisterListener(OnGetVolumeEvent);
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
                musicPlayer.Stream = musicDic[nextTrack];
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
                //Break out of function so that the list enumeration does notfail due to the list size changing while still in the loop
                break;
            }
        }
    }
    private void OnGetVolumeEvent(GetVolumeEvent gvei)
    {
        gvei.musicVolume = musicVolume;
        gvei.soundVolume = soundVolume;
    }
    private void OnChangeVolumeEvent(ChangeVolumeEvent cvei)
    {
        //Update the music volume
        if (cvei.musicVolume > -1) musicVolume = cvei.musicVolume;
        //Update the music players volume
        musicPlayer.VolumeDb = ConvertToDB(musicVolume);
        //Update the sound volume
        if (cvei.soundVolume > -1) soundVolume = cvei.soundVolume;
    }
    private void OnPlayMusicEvent(PlayMusicEvent pmei)
    {
        if (musicDic.ContainsKey(((MusicList)pmei.music).ToString()))
        {
            if (musicPlayer.Playing)
            {
                fadeOutMusic = true;
                changeMusicTrack = true;
                fadeOutMusicDone = false;
                lerpedMusicVolume = musicVolume;
                nextTrack = ((MusicList)pmei.music).ToString();
            }
            else
            {
                fadeInMusic = true;
                lerpedMusicVolume = 0;
                //Set the starting volume of the player when preparing to fade music in
                musicPlayer.VolumeDb = -80;
                musicPlayer.Stream = musicDic[((MusicList)pmei.music).ToString()];
                musicPlayer.Play();
            }
        }
        else
        {
            GD.Print("SoundManager - OnPlayMusicEvent = " + ((MusicList)pmei.music).ToString() + " musicList key was NOT found");
        }

        //Fade out old music and fade in new music
    }
    private void FadeInMusic()
    {
        lerpedMusicVolume = Mathf.Lerp(lerpedMusicVolume, musicVolume, 0.01f);
        musicPlayer.VolumeDb = ConvertToDB(lerpedMusicVolume);

        if (lerpedMusicVolume > (musicVolume - 2.0f))
        {
            fadeInMusic = false;
            musicPlayer.VolumeDb = ConvertToDB(musicVolume);
        }
    }
    private void FadeOutMusic()
    {

        lerpedMusicVolume = Mathf.Lerp(lerpedMusicVolume, 0, 0.01f);
        musicPlayer.VolumeDb = ConvertToDB(lerpedMusicVolume);

        if (lerpedMusicVolume < 2.0f)
        {
            fadeOutMusic = false;
            fadeOutMusicDone = true;
            musicPlayer.VolumeDb = ConvertToDB(0);
        }
    }
    private void OnPlaySoundEvent(PlaySoundEvent psei)
    {
        if (sfxDic.ContainsKey(((SFXList)psei.sound).ToString()))
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
            //Set the volume of the player before playing the sound
            ((AudioStreamPlayer)GD.InstanceFromId(closedSFXPlayers[closedSFXPlayers.Count - 1])).VolumeDb = ConvertToDB(soundVolume);
            //Set the sound to be played
            ((AudioStreamPlayer)GD.InstanceFromId(closedSFXPlayers[closedSFXPlayers.Count - 1])).Stream = sfxDic[((SFXList)psei.sound).ToString()];
            //Play the sound      
            ((AudioStreamPlayer)GD.InstanceFromId(closedSFXPlayers[closedSFXPlayers.Count - 1])).Play();
        }
        else
        {
            GD.Print("SoundManager - OnPlaySoundEvent = " + sfxDic[((SFXList)psei.sound).ToString()] + " musicList key was NOT found");
        }
    }
    private void CreateSFXPlayers()
    {
        //Create a temporary Asp2D struct
        AudioStreamPlayer tempSFXPlayer = new AudioStreamPlayer();
        //Changed newly created AudioStreamPlayer2D names
        tempSFXPlayer.Name = "SFXPlayer" + openSFXPlayers.Count;
        //Set the volume of the AudioStreamPlayer2D
        tempSFXPlayer.VolumeDb = soundVolume;
        //Add the AudioStreamPlayer2D as children in the stream
        AddChild(tempSFXPlayer);
        //Add a AudioStreamPlayer object Id to the list of open audio stream players list
        openSFXPlayers.Add(tempSFXPlayer.GetInstanceId());
    }
    private void GetSoundFiles()
    {
        Directory dir = new Directory();

        if (dir.Open("res://Audio/Music/") == Error.Ok)
        {
            //We start looping throuugh the files in the given directory and skip all the navigation directories
            dir.ListDirBegin(true, true);
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                if (fileName != "" && !fileName.Contains(".import"))
                {
                    String name = fileName.Remove(fileName.Find(".ogg"), fileName.Length - fileName.Find(".ogg"));
                    AudioStream tempAudio = ResourceLoader.Load<AudioStream>("res://Audio/Music/" + fileName) as AudioStream;
                    musicDic.Add(name, tempAudio);
                }
                fileName = dir.GetNext();
            }
            dir.ListDirEnd();
        }

        if (dir.Open("res://Audio/SFX/ActorSounds/") == Error.Ok)
        {
            dir.ListDirBegin(true, true);
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                if (fileName != "" && !fileName.Contains(".import"))
                {
                    String name = fileName.Remove(fileName.Find(".wav"), fileName.Length - fileName.Find(".wav"));
                    AudioStream tempAudio = ResourceLoader.Load<AudioStream>("res://Audio/ActorSounds/" + fileName) as AudioStream;
                    sfxDic.Add(name, tempAudio);
                }
                fileName = dir.GetNext();
            }
            dir.ListDirEnd();
        }

        if (dir.Open("res://Audio/SFX/ObjectSounds/") == Error.Ok)
        {
            dir.ListDirBegin(true, true);
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                if (fileName != "" && !fileName.Contains(".import"))
                {
                    String name = fileName.Remove(fileName.Find(".wav"), fileName.Length - fileName.Find(".wav"));
                    AudioStream tempAudio = ResourceLoader.Load<AudioStream>("res://Audio/SFX/ObjectSounds/" + fileName) as AudioStream;
                    sfxDic.Add(name, tempAudio);
                }
                fileName = dir.GetNext();
            }
            dir.ListDirEnd();
        }
    }
    private float ConvertToDB(float value)
    {
        return ((100 - value) * -80) / 100;
    }
    public override void _ExitTree()
    {
        //Stop the music when the audiomanager is destroyed
        musicPlayer.Stop();
        ChangeVolumeEvent.UnregisterListener(OnChangeVolumeEvent);
        PlayMusicEvent.UnregisterListener(OnPlayMusicEvent);
        PlaySoundEvent.UnregisterListener(OnPlaySoundEvent);
    }
}
