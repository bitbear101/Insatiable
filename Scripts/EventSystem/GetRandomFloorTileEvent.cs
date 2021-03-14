using Godot;
using System;
namespace EventCallback
{
    public class GetRandomFloorTileEvent : Event<GetRandomFloorTileEvent>
    {
        //The open floor tile from the map
        public Vector2 tilePos;
    }
}
