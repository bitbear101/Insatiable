using Godot;
using System;
using EventCallback;

public class FloatingText : Node2D
{
    //The Scene for instancing the floating text
    [Export] PackedScene tweenText;
    //The node tha is used to instance the floating text in the scene
    Node2D floatingText;

    public override void _Ready()
    {
        FloatingTextEvent.RegisterListener(OnFloatingTextEvent);
    }

    private void OnFloatingTextEvent(FloatingTextEvent fte)
    {
        //The instance of floating text scene
        floatingText = (Node2D)tweenText.Instance();
        //The spawn position of the floating text effect
        floatingText.Position = fte.position;
        //Add the Node as a child to the scene
        AddChild(floatingText);
        //The text to display
        ((TweenText)floatingText).SetText(fte.textToDisplay);
    }
}