using Godot;
using System;
namespace EventCallback
{
    public class SetDamageTypeEvent : Event<SetDamageTypeEvent>
    {
        //The id for the targeted actor that hte dmage type must be set
        public ulong actorID;
        //The damage type that must be set
        public DamageType damageType;
    }

}
