using Godot;
using System;

public class BottomText : Label {

    [Export]
    public float PrintCharTime = 1.0f;

    private bool _isOn = false;
    private float _timer = 0f;

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _ExitTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _Process(float delta) {
        if (_isOn == false) return;

        _timer += delta;
        if (_timer >= PrintCharTime) {
            _timer -= PrintCharTime;

            VisibleCharacters += 1;

            if (VisibleCharacters == Text.Length) {
                _isOn = false;
                GetNode<Arrow>("Arrow").Start();
                EventController.Send("state_respond");
            }
        }
    }

    // private char GetPlayerInput() {
    //     return '0';
    // }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("update_bottom_text".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is string s) {
                Text = s;
                VisibleCharacters = 0;
            }
        } else if ("start_bottom_text".Equals(args[0])) {
            _isOn = true;
            _timer = PrintCharTime;
        }
    }

}