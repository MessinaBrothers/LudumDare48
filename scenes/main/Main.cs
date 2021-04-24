using Godot;
using System;
using System.Collections.Generic;

public class Main : Node2D {

    [Export]
    public Color PrimaryColor = new Color("123456");

    public override void _Ready() {
        EventController.Send("update_color", PrimaryColor);
        EventController.Send("toggle_intro_text", true);
        EventController.Send("update_player_input", "");
        EventController.UpdateValidInputs(new List<string>() { "The Meaning of Life is" });
    }
}