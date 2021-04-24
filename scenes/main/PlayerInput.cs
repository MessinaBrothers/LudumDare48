using Godot;
using System;
using System.Collections.Generic;

public class PlayerInput : RichTextLabel {

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
        EventController.ValidInputEvent += HandleValidInputs;
    }

    public override void _ExitTree() {
        EventController.CommandEvent += HandleCommand;
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
                BbcodeText = s;
            }
        }
    }

    private void HandleValidInputs(List<string> validInputs) {
        // _validAnswers = validInputs;
        // foreach (string s in validInputs) GD.Print("answer: " + s);
    }

}