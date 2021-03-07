using Godot;
using System;
using EventCallback;

//The types of adventurer
public enum AdventurerType
{
    WIZARD,
    SORCERER,
    FIGHTER,
    BARBARIAN,
    CLERIC,
    PALADIN,
    RANGER
};
public class Adventurer : Actor
{
    //The default constructor for the adventurer class
    public Adventurer()
    {
    }
    //The type of adventurer
    AdventurerType type;
    //The public accesor for the type value but no set function
    public AdventurerType Type
    {
        //Returns the type of adventurer
        get { return type; }
    }
}
