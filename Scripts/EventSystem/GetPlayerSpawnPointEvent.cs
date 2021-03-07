using Godot;
using System;
namespace EventCallback
{
    public class GetPlayerSpawnPointEvent : Event<GetPlayerSpawnPointEvent>
    {
        public Vector2 spawnPos;
    }

}
