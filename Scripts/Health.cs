using Godot;
using System;
using EventCallback;
public class Health : Node
{
    public Health(int _health)
    {
        //The listener for the hit event
        HitEvent.RegisterListener(OnHitEvent);
    }
    int health = 5;

    private void OnHitEvent(HitEvent he)
    {
        if (he.target.GetInstanceId() == GetParent().GetInstanceId())
        {

        }
    }
    private void TakeDamage(int damage)
    {
        health -= damage;
    }
}
