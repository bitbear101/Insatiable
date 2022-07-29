using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EventCallback;

//The states for the game main loop
public enum GameStates
{
    MAIN,
    GAME,
    MENU
};

public class Main : Node2D
{    // //The external list of scenes to instantiate when the game menu is opened
    [Export] List<PackedScene> menuScenes = new List<PackedScene>();
    //The external list of scenes to instantiate when the game initiates after selectin play from the menu
    [Export] List<PackedScene> gameScenes = new List<PackedScene>();
    // //The external list of scenes to instantiate when the game is started from the main menu
    // [Export] List<PackedScene> gameScenes = new List<PackedScene>();

    //The list of nodes that will hold the pre loaded scenes
    List<Node> gameNodes = new List<Node>();
    // //The list of nodes that will hold the pre loaded scenes
    // List<Node> gameNodes = new List<Node>();
    // //The list of nodes that will hold the pre loaded scenes
    List<Node> menuNodes = new List<Node>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Check if the menu scenes list is not zero 
        if (menuScenes.Count > 0)
        {
            //Loop through all the scenes in the list
            foreach (PackedScene scene in menuScenes)
            {
                if (scene == null)
                {
                    GD.PrintErr("One of the scenes in the game scenes list are not set to an instance, check list");
                }
                else
                {
                    //Add the node of the scenes
                    menuNodes.Add(scene.Instance());
                }
            }
            //Loop through the list of scene nodes and add them to the current scene as children
            foreach (Node node in menuNodes)
            {
                AddChild(node);
            }
        }
        //The event message listner for the game start event message
        StartGameEvent.RegisterListener(OnStartGameEvent);
    }
    public override void _Process(float delta)
    {
        if (Input.IsActionJustReleased("ui_statsWindow"))
        {
            ToggleStatsWindowEvent tswe = new ToggleStatsWindowEvent();
            tswe.FireEvent();
        }
        if (Input.IsActionJustReleased("ui_escape"))
        {
            GetTree().Quit();
        }
    }
    private void OnStartGameEvent(StartGameEvent sge)
    {
        //Check if the main scenes list is not zero 
        if (gameScenes.Count > 0)
        {
            //Loop through all the scenes in the list
            foreach (PackedScene scene in gameScenes)
            {
                if (scene == null)
                {
                    GD.PrintErr("One of the scenes in the game scenes list are not set to an instance, check list");
                }
                else
                {
                    //Add the node of the scenes
                    gameNodes.Add(scene.Instance());
                }

            }
            //Loop through the list of scene nodes and add them to the current scene as children
            foreach (Node node in gameNodes)
            {
                GetNode<Viewport>("./ViewportContainer/Viewport").AddChild(node);
            }
        }
        //Send the event message to spawn the m,onsters from the monster manager
        SpawnMonstersEvent sme = new SpawnMonstersEvent();
        sme.callerClass = "Main - OnStartGameEvent";
        sme.FireEvent();
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


    public override void _ExitTree()
    {
        //The event message listner for the game start event message
        StartGameEvent.UnregisterListener(OnStartGameEvent);
    }
}
