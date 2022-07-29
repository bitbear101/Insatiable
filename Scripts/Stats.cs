using Godot;
using System;
using EventCallback;

public enum DamageType
{
	FIRE,
	ELECTRIC,
	ACID,
	CRUSHING,
	SLASH,
	PIERCE
};
public class Stats : Node
{
	//The damage type the  does
	[Export] DamageType damageType;
	//The level of the actor
	[Export] int level;
	//The experience of the actor
	[Export] float experience;
	//The strength of the actor
	[Export] float strength;
	//The dexterity of the actor
	[Export] float dexterity;
	//The intelligence of the actor
	[Export] float intelligence;
	//The corruption of the actor
	[Export] float corruption;

	//Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//The listener for the Get stats listener 
		GetStatsEvent.RegisterListener(OnGetStatsEvent);
		//The listener for hte set stats listener 
		SetStatsEvent.RegisterListener(OnSetStatsEvent);
	}

	private void OnGetStatsEvent(GetStatsEvent gse)
	{
		//Check the id of the actor to get the defence o
		if (gse.actorID == GetParent().GetInstanceId())
		{
			gse.strength = strength;
			gse.dexterity = dexterity;
			gse.intelligence = intelligence;
			gse.corruption = corruption;
			gse.level = level;
		}
	}

	private void OnSetStatsEvent(SetStatsEvent sse)
	{
		//Check the id of the actor to get the defence o
		if (sse.actorID == GetParent().GetInstanceId())
		{
			experience += sse.experience;
			strength += sse.strength;
			dexterity += sse.dexterity;
			intelligence += sse.intelligence;
			corruption += sse.corruption;
		}
	}

	public override void _ExitTree()
	{
		//The listener for the Get stats listener 
		GetStatsEvent.UnregisterListener(OnGetStatsEvent);
		//The listener for hte set stats listener 
		SetStatsEvent.UnregisterListener(OnSetStatsEvent);
	}
}
