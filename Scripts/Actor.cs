using Godot;
using System;

public class Actor
{
    public Actor()
    {
        //Run the init method when the actor is created
        Init();
    }
    /*
    Strength
    Dexterity
    Wisdom
    Intelegence

    Weapon
    Armour
    */
    //The level of the actor
    int level = 7;
    //The health of hte actor
    Health health;
    private void Init()
    {
        //Create the new helth for the ctor and fill the constructor
        health = new Health(10);
    }
    public int GetLevel()
    {
        return level;
    }
}
