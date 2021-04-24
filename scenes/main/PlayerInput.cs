using Godot;
using System;
using System.Collections.Generic;

public class PlayerInput : RichTextLabel {

    private List<string> _validAnswers;

    private string _input;
    private int _index = 0;
    private uint _lastChar = 0;

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
        EventController.ValidInputEvent += HandleValidInputs;
    }

    public override void _ExitTree() {
        EventController.CommandEvent += HandleCommand;
        EventController.ValidInputEvent -= HandleValidInputs;
    }

    public override void _Process(float delta) {
        char c = GetPlayerInput();
        if (c != '0') {

        }
    }

    private char GetPlayerInput() {
        return '0';
    }

    public override void _UnhandledKeyInput(InputEventKey key) {
        if (key.Scancode == _lastChar && key.Pressed == false) {
            _lastChar = 0;
        } else if (key.Pressed == true && key.Scancode != _lastChar) {
            //GD.Print(key.Scancode + " should be " + (int)_validAnswers[0][_index]);
            if (key.Scancode == Char.ToUpper(_validAnswers[0][_index])) {
                _lastChar = key.Scancode;
                _input += _validAnswers[0][_index++];
                BbcodeText = _input;
            }
        }
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("update_player_input".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is string s) {
                BbcodeText = s;
            }
        }
    }

    private void HandleValidInputs(List<string> validInputs) {
        _validAnswers = validInputs;
        foreach (string s in validInputs) GD.Print("answer: " + s);
    }

}