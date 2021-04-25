using Godot;
using System;

public class ScoreText : RichTextLabel {

    private uint _score, _max;

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _ExitTree() {
        EventController.CommandEvent -= HandleCommand;
    }

    public override void _Ready() {
        BbcodeText = "";
    }

    public override void _Process(float delta) {
        
    }

    public void SetScore() {
        BbcodeText = string.Format(
                    "Score: {0} [i]/[/i] {1}",
                    _score,
                    _max);
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("update_score".Equals(args[0])) {
            if (args.Length < 2) return;
            if (args[1] is uint score && args[2] is uint max) {
                _score = score;
                _max = max;
                GetNode<AnimationPlayer>("AnimationPlayer").Play("OnScore");
            }
        }
    }

}