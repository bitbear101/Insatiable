using Godot;
using System;
namespace EventCallback
{
    public class GetTileEvent : Event<GetTileEvent>
    {
        //The position of the tile requested
        public Vector2 pos;
        //The tile at the position requested
        public TileType tile;
    }

}
