using Godot;
using System;
using EventCallback;

public class FloatingText : Node2D
{
    Vector2 velocity = new Vector2(50, -100);
    Vector2 gravity = new Vector2(0, 1);
    float mass = 100;

    Tween myTween;

    Label label;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        FloatingTextEvent.RegisterListener(OnFloatingTextEvent);
        myTween = GetNode<Tween>("Tween");
        label = GetNode<Label>("Label");

        myTween.InterpolateProperty(this,
                                    "modulate",
                                    new Color(Modulate.r, Modulate.g, Modulate.b, Modulate.a),
                                    new Color(Modulate.r, Modulate.g, Modulate.b, 0.0f),
                                    0.3f,
                                    Tween.TransitionType.Linear,
                                    Tween.EaseType.Out,
                                    0.7f);

        myTween.InterpolateProperty(this,
                                    "scale",
                                    new Vector2(0, 0),
                                    new Vector2(1, 1),
                                    0.3f,
                                    Tween.TransitionType.Linear,
                                    Tween.EaseType.Out);

        myTween.InterpolateProperty(this,
                                    "scale",
                                    new Vector2(1, 1),
                                    new Vector2(0.4f, 0.4f),
                                    0.3f,
                                    Tween.TransitionType.Linear,
                                    Tween.EaseType.Out,
                                    0.6f);

        myTween.InterpolateCallback(this, 1.0f, "destroy");


        myTween.Start();
    }

    private void OnFloatingTextEvent(FloatingTextEvent fte)
    {
        //The spawn position of the floating text effect
        Position = fte.position;
        //The text to display
        SetText(fte.textToDisplay);
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