using Godot;
using System;

namespace EventCallback
{
    public class SetBBDataEvent : Event<SetBBDataEvent>
    {
        //The key for the data to be stored is the dictionary
        public int key;
        //The data tot be saved to the dictionary
        public object data;
    }
}
