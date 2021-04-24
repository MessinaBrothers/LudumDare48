using Godot;
using System;
using System.Collections.Generic;

public class Main : Node2D {

    struct Answer {
        public Answer(string r, string t) {
            response = r;
            texture = t;
        }
        public string response;
        public string texture;
    }

    private Dictionary<string, Answer> _prayAnswers = new Dictionary<string, Answer>() {
        { "Damn", new Answer("You damn them to hell. \"NNNNOOOOOooooooo\"", "test.png") },
        { "Deny", new Answer("You deny their request. \"Well, thanks anyway!\"", "test.png") },
        { "Grant", new Answer("You grant their request. \"Praise be God!\"", "test.png") },
        { "Ignite", new Answer("They spontaneously combust. \"Aaaarrrrgggghhhhhh\"", "test.png") },
        { "Ignore", new Answer("You ignore them. \"Hello? Are you there?\"", "test.png") },
        { "Lightning", new Answer("You cast a lightning bolt to their face. \"Aaaarrrrgggghhhhhh\"", "test.png") },
        { "Never", new Answer("Deny", "") },
        { "No", new Answer("Deny", "") },
        { "Okay", new Answer("Grant", "") },
        { "Smite", new Answer("Ignite", "") },
        { "Sure", new Answer("Grant", "") },
        { "Yeah", new Answer("Grant", "") },
        { "Yes", new Answer("Grant", "") },
        { "Yup", new Answer("Grant", "") },
    };

    // private List<Answer> _prayAnswers = new List<Answer>() {
    //     new Answer("Damn", "You damn them to hell. \"NNNNOOOOOooooooo\"", "test.png"),
    //     new Answer("Deny", "You deny their request. \"Well, thanks anyway!\"", "test.png"),
    //     new Answer("Grant", "You grant their request. \"Praise be God!\"", "test.png"),
    //     new Answer("Ignite", "They spontaneously combust. \"Aaaarrrrgggghhhhhh\"", "test.png"),
    //     new Answer("Ignore", "You ignore them. \"Hello? Are you there?\"", "test.png"),
    //     new Answer("Lightning", "You cast a lightning bolt to their face. \"Aaaarrrrgggghhhhhh\"", "test.png"),
    //     new Answer("Never", "Deny", ""),
    //     new Answer("No", "Deny", ""),
    //     new Answer("Okay", "Grant", ""),
    //     new Answer("Smite", "Ignite", ""),
    //     new Answer("Sure", "Grant", ""),
    //     new Answer("Yeah", "Grant", ""),
    //     new Answer("Yes", "Grant", ""),
    //     new Answer("Yup", "Grant", ""),
    // };

    // private List<string> _prayAnswers = new List<string>() {
    //     "Damn", "Deny",
    //     "Grant",
    //     "Ignite", "Ignore",
    //     "Lightning bolt to the face", "Lightning bolt to the groin",
    //     "Never", "No",
    //     "Okay",
    //     "Smite", "Sure",
    //     "Yeah", "Yes", "Yup"
    // };

    private Dictionary<string, Answer> _validAnswers;
    private List<string> _validKeys;
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

        _validAnswers = new Dictionary<string, Answer>() {
            { "The", new Answer("", "") } };
        UpdateAnswers(_validAnswers);

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
                    UpdateAnswers(_prayAnswers);
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
                for (int i = 0; i < _validKeys.Count; i++) {
                    if (_validKeys[i].StartsWith(_input) == false) continue;

                    if (key.Scancode == Char.ToUpper(_validKeys[i][_index])) {
                        _lastChar = key.Scancode;
                        _input += _validKeys[i][_index++];
                        EventController.Send("update_player_input", _input);

                        if (_index >= _validKeys[i].Length) {
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
                    EventController.Send("update_bottom_text", "A followers says \"Please help my son George tonight.\"");
                    _animPlayer.Play("Distraction");
                    _state = State.NONE;
                }
                break;
            case State.RESPOND_PROMPT:
                break;
            case State.RESPOND:
                if (HandleInput() == true) {
                    string response = _validAnswers[_input].response;
                    if (_validAnswers.ContainsKey(response)) response = _validAnswers[response].response;
                    EventController.Send("update_bottom_text", response);
                    EventController.Send("start_bottom_text");
                    _state = State.NONE;
                }
                break;
        }
    }

    public void AnimDistractionDone() {
        EventController.Send("start_bottom_text");
    }

    // private List<string> GetKeyList(Dictionary<string, Answer> answers) {
    //     List<string> l = new List<string>();
    //     l = new List<string>(answers.Keys);
    //     for (int i = 0; i < answers.Count; i++) {
    //         l.Add(answers[i].key);
    //     }
    //     return l;
    // }

    private void ResetInput() {
        _input = "";
        _index = 0;
    }

    private void UpdateAnswers(Dictionary<string, Answer> answers) {
        _validAnswers = answers;
        _validKeys = new List<string>(answers.Keys);
        EventController.UpdateValidInputs(_validKeys);
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("state_respond".Equals(args[0])) {
            _state = State.RESPOND_PROMPT;
        }
    }
}