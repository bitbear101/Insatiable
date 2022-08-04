using Godot;
using System;
using EventCallback;
public class StatsWindow : Control
{
    ulong playerID;
    bool hasPlayerID = false;

    Label lvlLabel, strLabel, dexLabel, intLabel, corLabel;
    // When the stats window is called we update the stats by calling stats from the player 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ToggleStatsWindowEvent.RegisterListener(OnToggleStatsWindowEvent);
        lvlLabel = (Label)GetNode("VBoxContainer/NinePatchRect/VBoxContainer/LevelHBox/LabelLevelAmount");
        strLabel = (Label)GetNode("VBoxContainer/NinePatchRect/VBoxContainer/StrengthHBox/LabelStrAmount");
        dexLabel = (Label)GetNode("VBoxContainer/NinePatchRect/VBoxContainer/DexterityHBox/LabelDexAmount");
        intLabel = (Label)GetNode("VBoxContainer/NinePatchRect/VBoxContainer/IntelligenceHBox/LabelIntAmount");
        corLabel = (Label)GetNode("VBoxContainer/NinePatchRect/VBoxContainer/CorruptionHBox/LabelCorruptionAmount");
    }
    private void OnToggleStatsWindowEvent(ToggleStatsWindowEvent tswe)
    {
        if (hasPlayerID == false)
        {
            playerID = GetTree().Root.FindNode("Player", true, false).GetInstanceId();
            hasPlayerID = true;
        }

        if (hasPlayerID == false) return;

        //Toggle the visibility of the stast window
        Visible = !Visible;
        GetStatsEvent gse = new GetStatsEvent();
        gse.callerClass = "StatsWindow - OnToggleStatsWindow()";
        gse.actorID = playerID;
        gse.FireEvent();

        lvlLabel.Text = (gse.level).ToString();
        strLabel.Text = (gse.strength).ToString();
        dexLabel.Text = (gse.dexterity).ToString();
        intLabel.Text = (gse.intelligence).ToString();
        corLabel.Text = (gse.corruption).ToString();
    }
    public override void _ExitTree()
    {
        ToggleStatsWindowEvent.UnregisterListener(OnToggleStatsWindowEvent);
    }
}
