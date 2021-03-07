using Godot;
using System;
using EventCallback;

public enum MonsterType
{
    ORC,
    CYCLOPS,
    SKELETON,
    SLIME

};

public class Monster : Actor
{
    //The defualt constructor for the monster class
    public Monster()
    {

    }
    //The type of the monster
    MonsterType type;
//Returns the type of monster
    public MonsterType Type
    {
        get { return type; }
    }

}
