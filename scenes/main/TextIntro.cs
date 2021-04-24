using Godot;
using System;

public class TextIntro : Label {

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _ExitTree() {
        EventController.CommandEvent += HandleCommand;
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("toggle_intro_text".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is bool b) {
                Visible = b;
            }
        }
    }
}