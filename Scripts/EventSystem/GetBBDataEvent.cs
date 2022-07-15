using Godot;
using System;

namespace EventCallback
{
    public class GetBBDataEvent : Event<GetBBDataEvent>
    {
        //The key for the data to be retrived from the dictionary
        public int key;
        //The data to be retrived from the dictionary
        public object data;
    }
}
