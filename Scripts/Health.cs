using Godot;
using System;
using EventCallback;
public class Health : Node
{
	//The base health of the actor
	[Export] int health = 10;
	public override void _Ready()
	{
		//The listener for the hit event
		HitEvent.RegisterListener(OnHitEvent);
	}

	private void OnHitEvent(HitEvent he)
	{
		if (he.targetID == GetParent().GetInstanceId())
		{
			//Calculate the damage for the actor
			CalculateDamageEvent cde = new CalculateDamageEvent();
			cde.callerClass = "Health - OnHitEvent()";
			cde.attackerID = he.attackerID;
			cde.targetID = he.targetID;
			cde.FireEvent();
			//Inject the calculated damage into the take damage method
			TakeDamage(cde.damage);
		}
	}

	private void TakeDamage(int damage)
	{
		health -= damage;
		 GD.Print("Health - OnHitEvent(): health left " + health);
		//Send the event message for the floating text
		FloatingTextEvent fte = new FloatingTextEvent();
		fte.position = ((Node2D)GetParent()).Position;
		fte.textToDisplay = damage.ToString();
		fte.FireEvent();

		//If there is no more health
		if (health <= 0)
		{
			DeathEvent de = new DeathEvent();
			de.callerClass = "Health - TakeDamage";
			de.targetID = GetParent().GetInstanceId();
			de.FireEvent();
		}
	}

	public override void _ExitTree()
	{
		//The listener for the hit event
		HitEvent.UnregisterListener(OnHitEvent);
	}
}
