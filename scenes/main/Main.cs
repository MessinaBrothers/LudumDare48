using Godot;
using System;
using System.Collections.Generic;

public class Main : Node2D {

    private readonly uint SCORE_MAX = 1;

    struct Answer {
        public Answer(string response, string result, string texture) {
            this.response = response;
            this.result = result;
            this.texture = texture;
        }
        public string response;
        public string result;
        public string texture;
    }

    private Dictionary<string, Answer> _meaningOfLifeAnswer = new Dictionary<string, Answer>() {
        //{ "The Meaning of Life is", new Answer("", "", "") },
        { "The", new Answer("", "", "") },
    };
    private Dictionary<string, Answer> _prayAnswers = new Dictionary<string, Answer>() {
        { "Damn", new Answer("You damn them to hell. \"NNNNOOOOOooooooo\"", "Your other followers are frightened, and more eager to please you.", "test.png") },
        { "Deny", new Answer("You deny their request. \"Well, thanks anyway!\"", "They seem unperturbed.", "test.png") },
        { "Grant", new Answer("You grant their request. \"Praise be God!\"", "They run to spread your Word.", "test.png") },
        { "Ignite", new Answer("They spontaneously combust. \"Aaaarrrrgggghhhhhh\"", "Passersby witness this, and convert immediately.", "test.png") },
        { "Ignore", new Answer("You ignore them. \"Hello? Are you there?\"", "Your followers are dismayed, and convert to a religion with a more responsive deity.", "test.png") },
        { "Lightning", new Answer("You cast a lightning bolt to their face. \"Aaaarrrrgggghhhhhh\"", "Passersby witness this, and convert immediately.", "test.png") },
        { "Never", new Answer("Deny", "", "") },
        { "No", new Answer("Deny", "", "") },
        { "Okay", new Answer("Grant", "", "") },
        { "Smite", new Answer("Ignite", "", "") },
        { "Sure", new Answer("Grant", "", "") },
        { "Yeah", new Answer("Grant", "", "") },
        { "Yes", new Answer("Grant", "", "") },
        { "Yup", new Answer("Grant", "", "") },
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
    private Dictionary<Dictionary<string, Answer>, string> _correctAnswers;
    private List<string> _validKeys;
    private AnimationPlayer _animPlayer;

    private string _input = "";
    private int _index = 0;
    private uint _lastChar = 0;
    private uint _score = 0;

    [Export]
    public Color PrimaryColor = new Color("123456");

    public enum State {
        WRITE_MOL,
        DISTRACTION_WAIT, DISTRACTION_PROMPT,
        WRITE_REPLY,
        RESPONSE_WAIT, RESPONSE_PROMPT,
        RESULT_WAIT, RESULT_PROMPT,
        WRITE_ENDING,
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

        _correctAnswers = new Dictionary<Dictionary<string, Answer>, string>() {
            { _prayAnswers, "Ignore" }
        };

        EventController.Send("update_color", PrimaryColor);
        EventController.Send("toggle_intro_text", false);

        UpdateAnswers(_meaningOfLifeAnswer);

        EventController.Send("update_result_text", "");
        EventController.Send("update_player_input", "");

        _state = State.WRITE_MOL;
    }

    public override void _Process(float delta) {
        if (Input.IsActionJustPressed("confirm")) {
            switch (_state) {
                case State.DISTRACTION_PROMPT:
                    ResetInput();
                    _animPlayer.Play("WriteReply");
                    UpdateAnswers(_prayAnswers);
                    _state = State.WRITE_REPLY;
                    EventController.Send("show_arrow", false);
                    break;
                case State.RESPONSE_PROMPT:
                    string result = _validAnswers[_input].result;
                    if (_validAnswers.ContainsKey(result)) result = _validAnswers[result].result;
                    EventController.Send("update_result_text", result);
                    EventController.Send("start_result_text");
                    EventController.Send("show_arrow", false);
                    _state = State.RESULT_WAIT;
                    break;
                case State.RESULT_PROMPT:
                    // score for correctness
                    if (_input == _correctAnswers[_validAnswers]) {
                        _score += 1;
                        EventController.Send("update_score", _score, SCORE_MAX);

                    }
                    ResetInput();
                    _animPlayer.Play("DistractionDisappear");
                    UpdateAnswers(_meaningOfLifeAnswer);
                    EventController.Send("show_arrow", false);
                    if (_score == SCORE_MAX) {
                        _state = State.WRITE_ENDING;
                    } else {
                        _state = State.WRITE_MOL;
                    }
                    break;
            }
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
            case State.WRITE_MOL:
                if (HandleInput() == true) {
                    EventController.Send("update_result_text", "");
                    EventController.Send("update_bottom_text", "A follower says \"Please help my son George tonight.\"");
                    _animPlayer.Play("Distraction");
                    _state = State.DISTRACTION_WAIT;
                }
                break;
            case State.WRITE_REPLY:
                if (HandleInput() == true) {
                    string response = _validAnswers[_input].response;
                    if (_validAnswers.ContainsKey(response)) response = _validAnswers[response].response;
                    EventController.Send("update_bottom_text", response);
                    _animPlayer.Play("Response");
                    _state = State.RESPONSE_WAIT;
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

        if ("done_bottom_text".Equals(args[0])) {
            if (_state == State.DISTRACTION_WAIT) {
                _state = State.DISTRACTION_PROMPT;
            } else if (_state == State.RESPONSE_WAIT) {
                _state = State.RESPONSE_PROMPT;
            }
        } else if ("done_result_text".Equals(args[0])) {
            _state = State.RESULT_PROMPT;
        }
    }
}