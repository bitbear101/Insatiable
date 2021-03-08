using Godot;
using System;
using EventCallback;
public class Health : Node
{
    int health = 5;

    public override void _Ready()
    {
        //The listener for the hit event
        HitEvent.RegisterListener(OnHitEvent);
    }

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
