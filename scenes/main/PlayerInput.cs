using Godot;
using System;
using System.Collections.Generic;

public class PlayerInput : RichTextLabel {

    [Export]
    public string ColorFaded = "393939";

    private static readonly Color COLOR_DUMMY = new Color(0.31f, 0.32f, 0.33f);

    private List<string> _validAnswers;

    private Color _colorOrig = COLOR_DUMMY;
    private Color _colorGlow = new Color(1f, 1f, 1f);

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
        EventController.ValidInputEvent += HandleValidInputs;
    }

    public override void _ExitTree() {
        EventController.CommandEvent -= HandleCommand;
        EventController.ValidInputEvent -= HandleValidInputs;
    }

    // public override void _Process(float delta) {
    //     char c = GetPlayerInput();
    //     if (c != '0') {

    //     }
    // }

    // private char GetPlayerInput() {
    //     return '0';
    // }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("update_player_input".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is string s) {
                FormatInput(s);
            }
        } else if ("update_player_input_force".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is string s) {
                BbcodeText = "[color=#ffffff]" + s;
            }
        } else if ("update_glow".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is float f) {
                if (_colorOrig == COLOR_DUMMY) _colorOrig = SelfModulate;
                SelfModulate = _colorOrig.LinearInterpolate(_colorGlow, f);
            }
        }
    }

    private void HandleValidInputs(List<string> validInputs) {
        _validAnswers = validInputs;

        //FormatInput("");
    }

    private void FormatInput(string input) {
        string s = string.Format("[color=#{0}]", ColorFaded);
        List<string> answers = new List<string>();
        for (int i = 0; i < _validAnswers.Count; i++) {
            if (input == _validAnswers[i]) {
                s = input;
                break;
            } else if (input == "" || _validAnswers[i].StartsWith(input)) {
                char c = _validAnswers[i][input.Length];
                if (answers.Contains(input + c) == false) {
                    answers.Add(input + c);
                    s += string.Format(
                        "[color=#{0}]{1}[/color]{2}\n",
                        answers.Count == 1 ? "ffffff" : "000000",
                        input,
                        c);
                }
            }
        }
        BbcodeText = s;
    }

}