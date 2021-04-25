using Godot;
using System;
using System.Collections.Generic;

public class Main : Node2D {

    private static readonly uint SCORE_MAX = 3;
    //private static readonly string MOL_PROMPT = "The Meaning of Life is";
    private static readonly string MOL_PROMPT = "The";

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

    private static Dictionary<string, Answer> _meaningOfLifeAnswer = new Dictionary<string, Answer>() {
        { MOL_PROMPT, new Answer("", "", "") },
    };
    private static Dictionary<string, Answer> _prayAnswers = new Dictionary<string, Answer>() {
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
        { "Destroy", new Answer("Ignite", "", "") },
        { "Sure", new Answer("Grant", "", "") },
        { "Yeah", new Answer("Grant", "", "") },
        { "Yes", new Answer("Grant", "", "") },
        { "Yup", new Answer("Grant", "", "") },
    };
    private static Dictionary<string, Answer> _angelAnswers = new Dictionary<string, Answer>() {
        { "Cast", new Answer("Hell", "", "test.png") },
        { "Defend", new Answer("Defend the chosen ones in their hour of conflict. \"On it, my Lord!\"", "That should keep her busy.", "test.png") },
        { "Descend", new Answer("Descend at the hour of death. \"Yes, my Lord!\"", "That should keep her busy.", "test.png") },
        { "Dream", new Answer("Appear in a dream. \"Yes, my Lord!\"", "That should keep her busy.", "test.png") },
        { "Dragon", new Answer("Slay", "", "test.png") },
        { "Dust", new Answer("Turn a city into dust. \"Yes, my Lord!\"", "That should keep her busy.", "test.png") },
        { "Expel", new Answer("Hell", "", "test.png") },
        { "Heal", new Answer("There is a plague. Go cure the people. \"Yes, my Lord!\"", "That should keep her busy.", "test.png") },
        { "Hell", new Answer("You cast your archangel into Hell. \"Oof!\"", "That should shut her up!", "test.png") },
        { "Herald", new Answer("Go and herald your replacement. \"Yes, my Lord!\"", "Your archangel retires, but another soon replaces her. Damn it.", "test.png") },
        { "Safeguard", new Answer("Safeguard against the wickedness. \"Yes, my Lord!\"", "Whatever that means.", "test.png") },
        { "Scale", new Answer("Weigh a soul on that perfect scale of yours. \"Yes, my Lord!\"", "Where did he get that thing, anyway?", "test.png") },
        { "Slay", new Answer("Go slay a dragon. \"Yes, my Lord!\"", "Later, you remember that dragons do not exist.", "test.png") },
        { "Smite", new Answer("Go and smite someone. \"Yes, my Lord!\"", "The smitten deserved it. Probably.", "test.png") },
        { "Wander", new Answer("Wander through the world for the ruin of souls. \"Yes, my Lord!\"", "So many ruins of souls in the world.", "test.png") },
        { "Weigh", new Answer("Scale", "", "test.png") },
    };
    private static Dictionary<string, Answer> _sonAnswers = new Dictionary<string, Answer>() {
        { "Agree", new Answer("You idly agree, which seems to please him. Idiot.", "He seems content. For now.", "test.png") },
        { "Crucify", new Answer("Sacrifice", "", "test.png") },
        { "Die", new Answer("Kill", "", "test.png") },
        { "Disagree", new Answer("You disagree with him, but he does not seem to listen.", "He seems content. For now.", "test.png") },
        { "Ignore", new Answer("You ignore him.", "He huffs and puffs and leaves, eventually. He will be back.", "test.png") },
        { "Kill", new Answer("You cast a lightning bolt to his face. \"Arrrgggghhh\"", "Once the smoke clears, you are dismayed to see him still standing, more annoyed than ever. Stupid immortality.", "test.png") },
        { "Lightning", new Answer("Kill", "", "test.png") },
        { "Persuade", new Answer("You try to convince him otherwise, but he does not seem to listen.", "He seems content. For now.", "test.png") },
        { "Reinforce", new Answer("You put on your dad hat and reinforce his beliefs.", "He seems content. For now.", "test.png") },
        { "Sacrifice", new Answer("Go to Earth, my son, and sacrifice yourself to save humanity.", "He seems keen to the idea. You never hear from him again.", "test.png") },
        { "Save", new Answer("Sacrifice", "", "test.png") },
        { "Soothe", new Answer("You soothe his fragile ego.", "He seems content. For now.", "test.png") },
        { "Smite", new Answer("Kill", "", "test.png") },
    };
    private Dictionary<Dictionary<string, Answer>, List<string>> _answerPrompts = new Dictionary<Dictionary<string, Answer>, List<string>>() {
        { _meaningOfLifeAnswer, new List<string>() {
            "This should never be seen!",
        } },
        { _prayAnswers, new List<string>() {
            "A follower says \"I pledge allegience, to the flag, of the United States of America. Amen!\" She seems confused.",
            "A follower asks \"Look after us on this holiday season.\"",
            "A follower requests \"Please Lord, I just want to see my daughters again.\"",
            "A follower says \"Help him, Dear Father.\" Who is him?",
            "A follower says \"Help my friend Mr. Bailey.\"",
            "A follower says \"Please help my son George tonight.\"",
            "A follower says \"Give him a break, God.\" Give who a break?",
            "A follower says \"Watch over him tonight.\" They should be more specific.",
            "A young follower says \"Please bring Daddy back.\"",
            "A distraught follower says \"Show me the way.\"",
            "A drunk follower says \"May our feet by swift.\"",
            "A boozy follower says \"May our bats be mighty.\"",
            "A handsome follower says \"May our balls be plentiful.\"",
            "A follower asks \"Please forgive me for not praying before I had a bite of my bagel.\"",
            "A follower thinks \"Send us the cure. We got the sickness already.\"",
            "A follower daydreams \"Would it be so terrible if I had a small fortune?\"",
        } },
        { _angelAnswers, new List<string>() {
            "Your archangel is back from Earth. \"What next, my Lord?\"",
            "Your archangel is back from Earth. What took her so long? \"What next, my Lord?\"",
            "Your archangel is back from Earth. Again. \"What next, my Lord?\"",
            "Your archangel is back from your latest distraction. \"What next, my Lord?\"",
            "\"Hark! It is I, your favorite archangel! How may I serve thee?\"",
        } },
        { _sonAnswers, new List<string>() {
            "\"Daaaaaaaaaaaaaaaaad!\" Ugh. What now?",
            "\"Daaaad!\" Where is his mother?",
            "\"Daaad!\" You wish he would leave you alone, but alas.",
            "\"Dad! Dad! Dad! Daaaaaaaaaad!\" You listen to his latest ramblings.",
            "\"Why is everyone so stupid, Dad?\"",
            "\"How come everyone can't be like me, Dad?\" ",
            "\"Daaaaaaaaaaad!\" You contemplate begotting another child. Being a single child is the worst.",
            "\"Daaaaaaaaad!\" Why will he not get a job already?",
        } },
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
    private Dictionary<Dictionary<string, Answer>, List<string>> _correctAnswers;
    private List<string> _validKeys;
    private AnimationPlayer _animPlayer;

    private string _input = "";
    private int _index = 0;
    private uint _lastChar = 0;
    private uint _score = 0;
    private bool _canEndGame = false;
    private int _promptsIndex = 0;

    [Export]
    public Color PrimaryColor = new Color("123456");

    public enum State {
        WRITE_MOL,
        DISTRACTION_WAIT, DISTRACTION_PROMPT,
        WRITE_REPLY,
        RESPONSE_WAIT, RESPONSE_PROMPT,
        RESULT_WAIT, RESULT_PROMPT,
        WRITE_ENDING_MOL, WRITE_ENDING_ANY,
        CONFIRM_WAIT, CONFIRM_PROMPT,
        GAME_OVER,
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

        _correctAnswers = new Dictionary<Dictionary<string, Answer>, List<string>>() {
            { _prayAnswers, new List<string>() { "Ignore" } },
            { _angelAnswers, new List<string>() { "Hell", "Cast" } },
            { _sonAnswers, new List<string>() { "Sacrifice", } },
        };

        EventController.Send("update_color", PrimaryColor);
        EventController.Send("toggle_intro_text", false);

        UpdateAnswers(_meaningOfLifeAnswer);

        EventController.Send("update_result_text", "");
        EventController.Send("update_player_input", "");

        _state = State.WRITE_MOL;
    }

    public override void _Process(float delta) {
        if (_state == State.CONFIRM_PROMPT) {
            if (Input.IsActionJustPressed("END_Y")) {
                _animPlayer.Play("GameOver");
                _state = State.GAME_OVER;
            } else if (Input.IsActionJustPressed("END_N")) {
                //EventController.Send("update_bottom_text", "Are you finished?");
                EventController.Send("show_arrow", true);
                _animPlayer.Play("ConfirmEnd", -1, -1, true);
                _state = State.WRITE_ENDING_ANY;
            }
        } else if (Input.IsActionJustPressed("confirm")) {
            switch (_state) {
                case State.DISTRACTION_PROMPT:
                    ResetInput();
                    _animPlayer.Play("WriteReply");
                    EventController.Send("update_player_input", "");
                    EventController.Send("update_glow", 0f);
                    _state = State.WRITE_REPLY;
                    EventController.Send("show_arrow", false);
                    break;
                case State.RESPONSE_PROMPT:
                    Answer answer = _validAnswers[_input];
                    if (_validAnswers.ContainsKey(answer.response)) answer = _validAnswers[answer.response];
                    string result = answer.result;
                    EventController.Send("update_result_text", result);
                    EventController.Send("start_result_text");
                    EventController.Send("show_arrow", false);
                    _state = State.RESULT_WAIT;
                    break;
                case State.RESULT_PROMPT:
                    // score for correctness
                    if (_correctAnswers[_validAnswers].Contains(_input)) {
                        _score += 1;
                        EventController.Send("update_score", _score, SCORE_MAX);

                    }
                    ResetInput();
                    _animPlayer.Play("DistractionDisappear");
                    UpdateAnswers(_meaningOfLifeAnswer);
                    EventController.Send("update_player_input", "");
                    EventController.Send("show_arrow", false);
                    if (_score == SCORE_MAX) {
                        _state = State.WRITE_ENDING_MOL;
                    } else {
                        _state = State.WRITE_MOL;
                    }
                    break;
                case State.WRITE_ENDING_ANY:
                    if (_canEndGame == true) {
                        EventController.Send("update_bottom_text", "Are you finished? Y or N");
                        EventController.Send("show_arrow", false);
                        _animPlayer.Play("ConfirmEnd");
                        _state = State.CONFIRM_WAIT;
                    }
                    break;
            }
        }
    }

    public override void _UnhandledKeyInput(InputEventKey key) {
        bool HandleInput(bool isMatch) {
            if (key.Scancode == (uint)KeyList.Backspace && key.Pressed == true) {
                _lastChar = key.Scancode;
                if (_input.Length == 0) return false;
                _input = _input.Left(--_index);
                if (_input.Contains(MOL_PROMPT)) {
                    EventController.Send("update_player_input_force", _input);
                } else {
                    EventController.Send("update_player_input", _input);
                }
            } else if (key.Scancode == _lastChar && key.Pressed == false) {
                _lastChar = 0;
            } else if (key.Pressed == true && key.Scancode != _lastChar) {
                if (isMatch == true) {
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
                } else if ((key.Scancode >= 'a' && key.Scancode <= 'z') || 
                    (key.Scancode >= 'A' && key.Scancode <= 'Z') ||
                    (key.Scancode >= '0' && key.Scancode <= '9') ||
                    key.Scancode == ' ' || key.Scancode == ',' ||
                    (key.Scancode == '1' && Input.IsKeyPressed((int)KeyList.Shift)) ||
                    (key.Scancode == '/' && Input.IsKeyPressed((int)KeyList.Shift)) ||
                    key.Scancode == '.') {
                    _lastChar = key.Scancode;
                    _index += 1;
                    char c = (char)_lastChar;
                    if (Input.IsKeyPressed((int)KeyList.Shift) == false) c = Char.ToLower(c);
                    if (key.Scancode == '1' && Input.IsKeyPressed((int)KeyList.Shift) == true) c = '!';
                    if (key.Scancode == '/' && Input.IsKeyPressed((int)KeyList.Shift) == true) c = '?';
                    _input += c;
                    EventController.Send("update_player_input_force", _input);
                }
            }
            return false;
        }


        switch (_state) {
            case State.WRITE_MOL:
                if (HandleInput(true) == true) {
                    EventController.Send("update_result_text", "");
                    // update valid answers
                    if (_score == 0) {
                        UpdateAnswers(_prayAnswers);
                    } else if (_score == 1) {
                        UpdateAnswers(_angelAnswers);
                    } else if (_score == 2) {
                        UpdateAnswers(_sonAnswers);
                    }
                    // get random prompt
                    if (_promptsIndex == _answerPrompts[_validAnswers].Count) {
                        _promptsIndex = 0;
                        List<string> prompts = _answerPrompts[_validAnswers];
                        Utility.Shuffle(prompts, new Random());
                    }
                    string prompt = _answerPrompts[_validAnswers][_promptsIndex++];
                    EventController.Send("update_bottom_text", prompt);
                    _animPlayer.Play("Distraction");
                    _state = State.DISTRACTION_WAIT;
                }
                EventController.Send("update_glow", 0.5f * _input.Length / MOL_PROMPT.Length);
                break;
            case State.WRITE_REPLY:
                if (HandleInput(true) == true) {
                    string response = _validAnswers[_input].response;
                    if (_validAnswers.ContainsKey(response)) response = _validAnswers[response].response;
                    EventController.Send("update_bottom_text", response);
                    _animPlayer.Play("Response");
                    _state = State.RESPONSE_WAIT;
                }
                break;
            case State.WRITE_ENDING_MOL:
                if (HandleInput(true) == true) {
                    _state = State.WRITE_ENDING_ANY;
                }
                EventController.Send("update_glow", 1f * _input.Length / MOL_PROMPT.Length);
                break;
            case State.WRITE_ENDING_ANY:
                HandleInput(false);

                if (_input.Contains(MOL_PROMPT) == false) {
                    _state = State.WRITE_ENDING_MOL;
                } else if (_canEndGame == false && _input.Length == MOL_PROMPT.Length + 1) {
                    _canEndGame = true;
                    EventController.Send("show_arrow", true);
                } else if (_canEndGame == true && _input.Length == MOL_PROMPT.Length) {
                    _canEndGame = false;
                    EventController.Send("show_arrow", false);
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

        _promptsIndex = _answerPrompts[_validAnswers].Count;
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("done_bottom_text".Equals(args[0])) {
            if (_state == State.DISTRACTION_WAIT) {
                EventController.Send("show_arrow", true);
                _state = State.DISTRACTION_PROMPT;
            } else if (_state == State.RESPONSE_WAIT) {
                EventController.Send("show_arrow", true);
                _state = State.RESPONSE_PROMPT;
            } else if (_state == State.CONFIRM_WAIT) {
                _state = State.CONFIRM_PROMPT;
            }
        } else if ("done_result_text".Equals(args[0])) {
            _state = State.RESULT_PROMPT;
            EventController.Send("show_arrow", true);
        }
    }
}