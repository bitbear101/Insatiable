using Godot;
using System;
using EventCallback;
public class PunchAttack : Node2D
{
    //The travel target for the attack
    Vector2 target;
    //The aatacks parent ID
    ulong parentID;
    //The hit box of the attack
    Area2D hitArea;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Get the ID of the parent
        parentID = GetParent().GetInstanceId();
        //Get the parents global positioning before setting it a stop level
        Vector2 parentPos = ((Node2D)GetParent()).GlobalPosition;
        //Set the punch as a top level as not to move with player anymore
        SetAsToplevel(true);
        //Set the hit area in script to the on instanced in the scene
        hitArea = GetNode<Area2D>("HitArea");
        //Set the connection for collision detection
        hitArea.Connect("area_entered", this, nameof(OnAreaEntered));
        //Set the connection for collision detection
        hitArea.Connect("body_entered", this, nameof(OnBodyEntered));
        //Set the position of the punch again as it is reset with the set as top level
        GlobalPosition = parentPos;
        //Get the modified mouse pos for the viewport
        Vector2 mouse_pos = ((Viewport)GD.InstanceFromId((ulong)(GetBBData((int)BBKey.VIEWPORT)))).GetMousePosition() / ((float)GetBBData((int)BBKey.WINDOW_SCALE)) - (((Vector2)GetBBData((int)BBKey.GAME_SIZE)) / 2) + GlobalPosition;
        // Set the direction for the hit
        LookAt(mouse_pos);
        // Set the taret for the hit on creation
        target = GlobalPosition + ((mouse_pos - GlobalPosition).Normalized() * 50);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GlobalPosition = GlobalPosition.LinearInterpolate(target, .1f);

        if (GlobalPosition.DistanceTo(target) < 1)
        {
            QueueFree();
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area.GetParent().IsInGroup("Monster"))
        {
            HitEvent he = new HitEvent();
            he.callerClass = "PunchAttack - OnAreaEntered()";
            he.attackerID = parentID;
            he.targetID = area.GetParent().GetInstanceId();
            he.FireEvent();

        }

        if (area.GetParent().IsInGroup("Corpse"))
        {
            SetStatsEvent sse = new SetStatsEvent();
            GetStatsEvent gse = new GetStatsEvent();
            sse.callerClass = "PunchAttack - OnAreaEntered()";
            gse.callerClass = "PunchAttack - OnAreaEntered()";
            //Pass in the corpse id to get the stats from
            gse.actorID = area.GetParent().GetInstanceId();
            gse.FireEvent();
            //The strength of the actor
            sse.strength = gse.strength * 0.025f;
            //The dexterity of the actor
            sse.dexterity = gse.dexterity * 0.025f;
            //The intelegence of the actor
            sse.intelligence = gse.intelligence * 0.025f;
            //The level of the actor
            sse.experience = gse.level * 0.25f;
            //The corruption of the actor
            sse.corruption = gse.corruption * 0.25f;
            sse.actorID = parentID;
            sse.FireEvent();

            DeathEvent de = new DeathEvent();
            de.callerClass = "PunchAttack - OnAreaEntered()";
            de.targetID = area.GetParent().GetInstanceId();
            de.FireEvent();
        }
        //Free the punch attack scene
        QueueFree();
    }
    private void OnBodyEntered(StaticBody2D body)
    {
        if (body.GetParent().IsInGroup("Map"))
        {
            Polygon2D poly = new Polygon2D();
            Vector2[] shape = { new Vector2(10, 10), new Vector2(10, 20), new Vector2(20, 20), new Vector2(20, 10) };
            poly.Polygon = shape;



            Vector2 globPos = GlobalPosition.LinearInterpolate(target, .2f);
            Vector2 newPos;

            newPos.x = Mathf.Round(globPos.x / 16) - globPos.x % 16;
            newPos.y = Mathf.Round(globPos.y) - globPos.y % 16;

    poly.GlobalPosition = GlobalPosition.LinearInterpolate(target, .1f);;
        
            // newPos.x = globPos.x - globPos.x % 16;
            // newPos.y = globPos.y - globPos.y % 16;
            // newPos.x = GlobalPosition.x - GlobalPosition.x % 16;
            // newPos.y = GlobalPosition.y - GlobalPosition.y % 16;
            // StoneToFloorEvent stfe = new StoneToFloorEvent();
            // stfe.callerClass = "PunchAttack - OnBodyEntered()";
            // stfe.TileToChange = newPos / 16;
            // stfe.FireEvent();
            GetParent().GetParent().AddChild(poly);
            //Free the punch attack scene
            QueueFree();
        }
    }

    private object GetBBData(int key)
    {
        GetBBDataEvent gbbde = new GetBBDataEvent();
        gbbde.callerClass = "CameraManager - GetBBData()";
        gbbde.key = key;
        gbbde.FireEvent();
        return gbbde.data;
    }

    private void SetBBdata(int key, object data)
    {
        SetBBDataEvent sbbde = new SetBBDataEvent();
        sbbde.callerClass = "CameraManager - GetBBData()";
        sbbde.key = key;
        sbbde.data = data;
        sbbde.FireEvent();
    }
}
