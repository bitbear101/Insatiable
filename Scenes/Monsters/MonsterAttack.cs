using Godot;
using System;
using EventCallback;
public class MonsterAttack : Node
{
    //The timer to check if the monster can attack
    Timer attacktimer;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Listen for the attack event message
        EnemyAttackEvent.RegisterListener(OnEnemyAttackEvent);
        attacktimer = GetNode<Timer>("../AttackTimer");
    }

    private void OnEnemyAttackEvent(EnemyAttackEvent eae)
    {
        if (attacktimer.IsStopped())
        {
            attacktimer.Start();
            HitEvent he = new HitEvent();
            he.callerClass = "MonsterAttack - OnEnemyAttackEvent";
            he.attackerID = eae.attackerID;
            he.targetID = eae.targetID;
            he.FireEvent();
        }
    }

    public override void _ExitTree()
    {
        //Listen for the attack event message
        EnemyAttackEvent.UnregisterListener(OnEnemyAttackEvent);
    }
}
