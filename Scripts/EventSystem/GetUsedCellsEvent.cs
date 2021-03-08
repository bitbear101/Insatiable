using Godot;
using System;
using System.Collections.Generic;
namespace EventCallback
{
    public class GetUsedCellsEvent : Event<GetUsedCellsEvent>
    {
        //The cells (tiles) from the map
        public List<Vector2> cells = new List<Vector2>();

    }
}