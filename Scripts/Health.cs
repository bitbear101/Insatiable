using Godot;
using System;

public class Health
{
    public Health(int _health)
    {
        //Set the new health when created for the actor
        health = _health;
    }
    int health;
    private void TakeDamage(int damage)
    {
        health -= damage;
    }
}
