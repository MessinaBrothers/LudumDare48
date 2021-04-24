using Godot;
using System;
using System.Collections.Generic;

public class Main : Node2D {

    private List<string> _validAnswers;
    private AnimationPlayer _animPlayer;

    private string _input = "";
    private int _index = 0;
    private uint _lastChar = 0;

    [Export]
    public Color PrimaryColor = new Color("123456");

    public enum State {
        NONE, INTRO, RESPOND_PROMPT
    }
    public State _state;

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _ExitTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _Ready() {
        _animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        EventController.Send("update_color", PrimaryColor);
        EventController.Send("toggle_intro_text", true);
        EventController.Send("update_player_input", "");

        _validAnswers = new List<string>() { "The" };
        EventController.UpdateValidInputs(_validAnswers);

        _state = State.INTRO;
    }

    public override void _UnhandledKeyInput(InputEventKey key) {
        switch (_state) {
            case State.NONE:
                break;
            case State.INTRO:
                if (key.Scancode == _lastChar && key.Pressed == false) {
                    _lastChar = 0;
                } else if (key.Pressed == true && key.Scancode != _lastChar) {
                    //GD.Print(key.Scancode + " should be " + (int)_validAnswers[0][_index]);
                    if (key.Scancode == Char.ToUpper(_validAnswers[0][_index])) {
                        _lastChar = key.Scancode;
                        _input += _validAnswers[0][_index++];
                        EventController.Send("update_player_input", _input);

                        if (_index >= _validAnswers[0].Length) {
                            EventController.Send("update_bottom_text", "Please help my son George tonight.");
                            _animPlayer.Play("Distraction");
                            _state = State.NONE;
                        }
                    }
                }       
                break;
            case State.RESPOND_PROMPT:
                if (key.Scancode == (uint)KeyList.Enter && key.Pressed == true) {
                    _animPlayer.Play("Respond");
                    _state = State.NONE;
                }
                break;
        }
    }

    public void AnimDistractionDone() {
        EventController.Send("start_bottom_text");
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("state_respond".Equals(args[0])) {
            _state = State.RESPOND_PROMPT;
        }
    }
}