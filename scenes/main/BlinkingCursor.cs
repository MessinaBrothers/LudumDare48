using Godot;
using System;

public class BlinkingCursor : Label {

    private float _timer;

    public override void _Process(float delta) {
        _timer += delta;
        if (_timer >= .5f) {
            _timer -= 0.5f;
            Visible = !Visible;
        }

        if (Input.IsKeyPressed((int)KeyList.T)) {
            QueueFree();
        }
    }
}