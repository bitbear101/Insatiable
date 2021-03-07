using Godot;
using System;
namespace EventCallback
{
    public class StoneToFloorEvent : Event<StoneToFloorEvent>
    {
        //The stone tile to change to a floor tile
        public Vector2 TileToChange;
    }
}