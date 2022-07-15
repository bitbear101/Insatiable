using Godot;
using System;
using EventCallback;

public class TweenText : Node2D
{
     Vector2 velocity;
    Vector2 gravity = new Vector2(0, 1);
    float mass = 200;

    //The random number generator
    RandomNumberGenerator rng = new RandomNumberGenerator();
    Tween myTween;
    //The label for the floating text
    Label label;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        myTween = GetNode<Tween>("Tween");
        label = GetNode<Label>("Label");  
        
        //Get the random hop direction for the text
        velocity = new Vector2((float)rng.RandiRange(-50, 50), -100);

        myTween.InterpolateProperty(this,
                                    "modulate",
                                    new Color(Modulate.r, 0.2f, 0.2f, 1.0f),
                                    new Color(0.2f, 0.2f, 0.2f, 0.0f),
                                    0.3f,
                                    Tween.TransitionType.Linear,
                                    Tween.EaseType.Out,
                                    0.0f);
        myTween.Start();
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        velocity += gravity * mass * delta;
        Position += velocity * delta;
    }
    public string GetText()
    {
        return label.Text;
    }
    public void SetText(string textInput)
    {
        label.Text = textInput;
    }
}
