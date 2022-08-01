using Godot;
using System;
using System.Collections.Generic;
namespace EventCallback
{
    public class GetUsedCellsEvent : Event<GetUsedCellsEvent>
    {
        //Te array of cells specifiedto be returned
        public  List<Vector2> cells = new List<Vector2>();
    }
}
