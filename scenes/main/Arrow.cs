using Godot;
using System;

public class Arrow : Sprite {

    [Export]
    public float Time = 1f;

    private bool _isOn = false;
    private float _timer = 0f;

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _ExitTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _Ready() {
        Visible = false;
    }

    public override void _Process(float delta) {
        if (_isOn == true) {
            _timer += delta;
            if (_timer >= Time) {
                _timer -= Time;
                Visible = !Visible;
            }
        }
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        void Start() {
            _isOn = true;
            _timer = 0;
            Visible = true;
        }

        if ("show_arrow".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is bool b) {
                if (b == false) {
                    _isOn = false;
                    Visible = false;
                } else {
                    Start();
                }
            }
        }
    }

}