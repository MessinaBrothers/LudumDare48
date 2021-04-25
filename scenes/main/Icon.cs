using Godot;
using System;

public class Icon : Sprite {

    [Export]
    public float Time = 1f;

    private float _timer = 0f;
    private int _index;
    private int _nextX, _nextY;

    public override void _EnterTree() {
        EventController.UpdateIconEvent += HandleUpdate;
        EventController.CommandEvent += HandleCommand;
    }

    public override void _ExitTree() {
        EventController.UpdateIconEvent -= HandleUpdate;
        EventController.CommandEvent -= HandleCommand;
    }

    public override void _Process(float delta) {
        _timer += delta;
        if (_timer >= Time) {
            _timer -= Time;
            _index = (_index + 1) % 2;
            Frame = _index;
        }
    }

    private void HandleUpdate(int x, int y) {
        _nextX = x;
        _nextY = y;
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("update_icon".Equals(args[0])) {
            RegionRect = new Rect2(_nextX * 16, _nextY * 16, 32f, 16f);
            _timer = 0f;
            _index = 0;
            Frame = 0;
        }
    }
}