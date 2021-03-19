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
{
    //The external list of scenes to instantiate when the game initiates for the first time
    [Export]
    private List<PackedScene> mainScenes = new List<PackedScene>();
    //The external list of scenes to instantiate when the game is started from the main menu
    [Export]
    private List<PackedScene> gameScenes = new List<PackedScene>();
    //The external list of scenes to instantiate when the game menu is opened
    [Export]
    private List<PackedScene> menuScenes = new List<PackedScene>();
    //The list of nodes that will hold the pre loaded scenes
    List<Node> mainNodes = new List<Node>();
    //The list of nodes that will hold the pre loaded scenes
    List<Node> gameNodes = new List<Node>();
    //The list of nodes that will hold the pre loaded scenes
    List<Node> menuNodes = new List<Node>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Using a workaround not to crash Godot when exporting lists to use in the inspector
        mainScenes = mainScenes.ToList();
        gameScenes = gameScenes.ToList();
        menuScenes = menuScenes.ToList();
        //Check if the main scenes list is not zero 
        if (mainScenes.Count > 0)
        {
            //Loop through all the scenes in the list
            foreach (PackedScene scene in mainScenes)
            {
                //Add the node of the scenes
                mainNodes.Add(scene.Instance());
            }
            //Loop through the list of scene nodes and add them to the current scene as children
            foreach (Node node in mainNodes)
            {
                AddChild(node);
            }
        }

        //Check if the game scenes list is not zero 
        if (gameScenes.Count > 0)
        {
            //Loop through all the scenes in the list
            foreach (PackedScene scene in gameScenes)
            {
                //Add the node of the scenes
                gameNodes.Add(scene.Instance());
            }
        }

        //Check if the menu scenes list is not zero 
        if (menuScenes.Count > 0)
        {
            //Loop through all the scenes in the list
            foreach (PackedScene scene in menuScenes)
            {
                //Add the node of the scenes
                menuNodes.Add(scene.Instance());
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

    private void OnStartGameEvent(StartGameEvent sge)
    {
        //Loop through the list of scene nodes and add them to the current scene as children
        foreach (Node node in gameNodes)
        {
            AddChild(node);
        }
        //Send the message event to generate the new event
        GenerateLevelEvent gme = new GenerateLevelEvent();
        gme.callerClass = "Main - OnStartGameEvent";
        gme.FireEvent();
        //Sebnd the event message to spawn the m,onsters from the monster manager
        SpawnMonstersEvent sme = new SpawnMonstersEvent();
        sme.callerClass = "Main - OnStartGameEvent";
        sme.FireEvent();
    }
}
