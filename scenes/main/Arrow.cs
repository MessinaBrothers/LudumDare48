using Godot;
using System;

public class Arrow : Sprite {

    [Export]
    public float Time = 1f;

    private bool _isOn = false;
    private float _timer = 0f;

    public override void _Ready() {
        Visible = false;
    }

    public void Start() {
        _isOn = true;
        _timer = 0;
        Visible = true;
    }

    public void Stop() {
        _isOn = false;
    }

    public override void _Process(float delta) {
        if (_isOn == true) {
            _timer += delta;
            if (_timer >= Time) {
                _timer -= Time;
                Visible = !Visible;
            }

            if (Input.IsActionJustPressed("confirm")) {
                _isOn = false;
                Visible = false;
            }
        }
    }

}