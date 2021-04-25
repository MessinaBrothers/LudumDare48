using Godot;
using System;

public class ResultText : RichTextLabel {

    [Export]
    public float PrintCharTime = 1.0f;
    
    private static readonly Color COLOR_DUMMY = new Color(0.31f, 0.32f, 0.33f);
    private Color _colorOrig = COLOR_DUMMY;
    private Color _colorGlow = new Color(1f, 1f, 1f);

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
            EventController.Send("play_instrument");

            if (VisibleCharacters >= BbcodeText.Length) {
                _isOn = false;
                EventController.Send("done_result_text");
            }
        }
    }

    // private char GetPlayerInput() {
    //     return '0';
    // }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("update_result_text".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is string s) {
                if (s.Contains("\"")) {
                    
                }
                string[] split = s.Split('\"');
                if (split.Length == 1) {
                    BbcodeText = "[center]" + s;
                } else {
                    BbcodeText = "[center]" + split[0] + "[i]\" " + split[1] + " \"[/i]" + split[2];
                }
                VisibleCharacters = 0;
            }
        } else if ("start_result_text".Equals(args[0])) {
            _isOn = true;
            _timer = PrintCharTime;
        } else if ("update_result_glow".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is bool b) {
                if (_colorOrig == COLOR_DUMMY) _colorOrig = SelfModulate;
                SelfModulate = b ? _colorGlow : _colorOrig;
            }
        }
    }

}