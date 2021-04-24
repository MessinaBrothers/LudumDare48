using Godot;
using System;

public class ScoreText : RichTextLabel {

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _ExitTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _Ready() {
        
    }

    public override void _Process(float delta) {
        
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("update_score".Equals(args[0])) {
            if (args.Length < 2) return;
            if (args[1] is uint score && args[2] is uint max) {
                BbcodeText = string.Format(
                    "[right]Score: {0} [i]/[/i] {1}",
                    score,
                    max);

            }

        }
    }

}