using Godot;
using System;

public class MainMenu : Node2D {

    public override void _Ready() {
        EventController.Send("show_arrow", true);
    }

    public override void _Process(float delta) {
        if (Input.IsActionJustPressed("confirm")) {
            GetTree().ChangeScene("res://scenes/main/Main.tscn");
        }
    }
}