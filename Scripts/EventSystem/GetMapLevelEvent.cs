using Godot;
using System;
namespace EventCallback
{
    public class GetMapLevelEvent : Event<GetMapLevelEvent>
    {
        //The level of the map to be returned
        public int mapLevel;
//The max levels the game has that was requested
public int maxLevels;

    }
}
