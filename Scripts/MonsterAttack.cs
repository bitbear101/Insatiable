using Godot;
using System;
using EventCallback;
public class MonsterAttack : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        EnemyAttackEvent.RegisterListener(OnEnemyAttackEvent);

    }

    private void OnEnemyAttackEvent(EnemyAttackEvent eae)
    {
//Check if the player is still in range, if he is stil in range
    }
}
