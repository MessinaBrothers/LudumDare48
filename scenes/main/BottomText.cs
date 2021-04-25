using Godot;
using System;

public class BottomText : RichTextLabel {

    [Export]
    public float PrintCharTime = 1.0f;

    private bool _isOn = false;
    private float _timer = 0f;

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _ExitTree() {
        EventController.CommandEvent -= HandleCommand;
    }

    public override void _Process(float delta) {
        if (_isOn == false) return;

        if (Input.IsActionJustPressed("confirm") && VisibleCharacters > 4) {
            VisibleCharacters = BbcodeText.Length;
        }

        _timer += delta;
        if (_timer >= PrintCharTime) {
            _timer -= PrintCharTime;

            VisibleCharacters += 1;

            if (VisibleCharacters >= BbcodeText.Length) {
                _isOn = false;
                EventController.Send("done_bottom_text");
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
                if (s.Contains("\"")) {
                    
                }
                string[] split = s.Split('\"');
                if (split.Length == 1) {
                    BbcodeText = s;
                } else {
                    BbcodeText = split[0] + "[i]\" " + split[1] + " \"[/i]" + split[2];
                }
                VisibleCharacters = 0;
            }
        } else if ("start_bottom_text".Equals(args[0])) {
            _isOn = true;
            _timer = PrintCharTime;
        }
    }

}