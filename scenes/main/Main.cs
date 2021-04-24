using Godot;
using System;
using System.Collections.Generic;

public class Main : Node2D {

    private List<string> _prayAnswers = new List<string>() {
        "Damn", "Deny",
        "Grant",
        "Ignite", "Ignore",
        "Lightning bolt to the face", "Lightning bolt to the groin",
        "Never", "No",
        "Okay",
        "Smite", "Sure",
        "Yeah", "Yes", "Yup"
    };

    private List<string> _validAnswers;
    private AnimationPlayer _animPlayer;

    private string _input = "";
    private int _index = 0;
    private uint _lastChar = 0;

    [Export]
    public Color PrimaryColor = new Color("123456");

    public enum State {
        NONE, INTRO, RESPOND_PROMPT, RESPOND,
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
        EventController.Send("toggle_intro_text", false);

        _validAnswers = new List<string>() { "The" };
        EventController.UpdateValidInputs(_validAnswers);

        EventController.Send("update_player_input", "");

        _state = State.INTRO;
    }

    public override void _Process(float delta) {
        switch (_state) {
            case State.NONE:
                break;
            case State.INTRO:
                break;
            case State.RESPOND_PROMPT:
                if (Input.IsActionJustPressed("confirm")) {
                    ResetInput();
                    _animPlayer.Play("Respond");
                    _validAnswers = _prayAnswers;
                    EventController.UpdateValidInputs(_validAnswers);
                    _state = State.RESPOND;
                }
                break;
        }
    }

    public override void _UnhandledKeyInput(InputEventKey key) {
        bool HandleInput() {
            if (key.Scancode == (uint)KeyList.Backspace && key.Pressed == false && _input.Length > 0) {
                _input = _input.Left(_input.Length - 1);
                _index -= 1;
                EventController.Send("update_player_input", _input);
            } else if (key.Scancode == _lastChar && key.Pressed == false) {
                _lastChar = 0;
            } else if (key.Pressed == true && key.Scancode != _lastChar) {
                //GD.Print(key.Scancode + " should be " + (int)_validAnswers[0][_index]);
                for (int i = 0; i < _validAnswers.Count; i++) {
                    if (_validAnswers[i].StartsWith(_input) == false) continue;

                    if (key.Scancode == Char.ToUpper(_validAnswers[i][_index])) {
                        _lastChar = key.Scancode;
                        _input += _validAnswers[i][_index++];
                        EventController.Send("update_player_input", _input);

                        if (_index >= _validAnswers[i].Length) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        switch (_state) {
            case State.NONE:
                break;
            case State.INTRO:
                if (HandleInput() == true) {
                    EventController.Send("update_bottom_text", "Please help my son George tonight.");
                    _animPlayer.Play("Distraction");
                    _state = State.NONE;
                }
                break;
            case State.RESPOND_PROMPT:
                break;
            case State.RESPOND:
                if (HandleInput() == true) {
                    _state = State.NONE;
                }
                break;
        }
    }

    public void AnimDistractionDone() {
        EventController.Send("start_bottom_text");
    }

    private void ResetInput() {
        _input = "";
        _index = 0;
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("state_respond".Equals(args[0])) {
            _state = State.RESPOND_PROMPT;
        }
    }
}