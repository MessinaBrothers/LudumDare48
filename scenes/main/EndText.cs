using Godot;
using System;

public class EndText : Label {

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _ExitTree() {
        EventController.CommandEvent -= HandleCommand;
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("update_final_answer".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is string s) {
                Text = "You figured it out. The Meaning of Life.\n\n" + s;
            }
        }
    }
}