using Godot;
using System;
using EventCallback;
public class Corps : Node2D
{
	// int strength;
	// int dexterity;
	// int intelligence;
	// int corruption;

	// // Called when the node enters the scene tree for the first time.
	// public override void _Ready()
	// {
	// 	//Set the corpses stats after instancing
	// 	SetCorpseStatsEvent.RegisterListener(OnSetCorpseStatsEvent);
	// 	//Get the corpses stats after instancing
	// 	GetCorpseStatsEvent.RegisterListener(OnGetCorpseStatsEvent);
	// }

	// private void OnGetCorpseStatsEvent(GetCorpseStatsEvent gcse)
	// {
	// 	GD.Print("Corpse - OnGetCorpseStatsEvent: Called");
	// 	if (gcse.corpseID == GetParent().GetInstanceId())
	// 	{
	// 		gcse.strength = strength;
	// 		gcse.dexterity = dexterity;
	// 		gcse.intelligence = intelligence;
	// 		gcse.corruption = corruption;
	// 	}
	// }
	// public override void _ExitTree()
	// {
	// 	GetCorpseStatsEvent.UnregisterListener(OnGetCorpseStatsEvent);
	// }
}
