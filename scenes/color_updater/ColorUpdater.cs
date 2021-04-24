using Godot;
using System;

public class ColorUpdater : Node {

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _ExitTree() {
        EventController.CommandEvent += HandleCommand;
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("update_color".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is Color color) {
                if (GetParent() is Node2D node) {
                    //GD.Print("changing color for node " + node.Name);
                    node.SelfModulate = color;
                } else if (GetParent() is Control control) {
                    //GD.Print("changing color for control " + control.Name);
                    control.SelfModulate = color;
                }
            }
        }
    }
}