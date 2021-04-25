using Godot;
using System;
using System.Collections.Generic;

public class Main : Node2D {

    private static readonly uint SCORE_MAX = 3;
    private static readonly int PORTRAIT_MAX_INDEX = 3;
    private static readonly string MOL_PROMPT = "The Meaning of Life is";

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
        { "Damn", new Answer("\"Noooooooooooooooooo\"", "You damn them to HECK.\nYour other followers work even harder to please you.", "0x3") },
        { "Deny", new Answer("\"Understood! It's all part of your plan.\"", "They seem undeterred.", "2x3") },
        { "Grant", new Answer("You grant their request. \"Praise be God!\"", "They run to spread your Word.", "4x3") },
        { "Ignite", new Answer("They spontaneously combust. \"Aaaarrrrgggghhhhhh\"", "Passersby witness this, and convert immediately.", "0x3") },
        { "Ignore", new Answer("You ignore them. \"Hello? Are you there?\"", "Your followers are dismayed, and convert to a religion with a more responsive deity.", "6x3") },
        { "Lightning", new Answer("\"Aaaarrrrgggghhhhhh\" Your lightning bolt hits them right in the face. ", "Passersby witness this, and convert immediately.", "0x4") },
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
        { "Cast", new Answer("Hell", "", "") },
        { "Defend", new Answer("Defend the chosen ones in their hour of conflict. \"On it, my Lord!\"", "That should keep her busy.", "2x4") },
        { "Descend", new Answer("Descend at the hour of death. \"Yes, my Lord!\"", "That should keep her busy.", "4x4") },
        { "Dream", new Answer("Appear in a dream. \"Yes, my Lord!\"", "That should keep her busy.", "6x4") },
        { "Dragon", new Answer("Slay", "", "") },
        { "Dust", new Answer("Turn a city into dust. \"Yes, my Lord!\"", "That should keep her busy.", "4x4") },
        { "Expel", new Answer("Hell", "", "") },
        { "Heal", new Answer("There is a plague. Go cure the people. \"Yes, my Lord!\"", "That should keep her busy.", "2x4") },
        { "Hell", new Answer("You cast your archangel into Hell. \"Oof!\"", "That should shut her up!", "0x3") },
        { "Herald", new Answer("Go and herald your replacement. \"Yes, my Lord!\"", "Your archangel retires, but another soon replaces her. Damn it.", "4x4") },
        { "Safeguard", new Answer("Safeguard against the wickedness. \"Yes, my Lord!\"", "Whatever that means.", "2x4") },
        { "Scale", new Answer("Weigh a soul on that perfect scale of yours. \"Yes, my Lord!\"", "Where did he get that thing, anyway?", "2x4") },
        { "Slay", new Answer("Go slay a dragon. \"Yes, my Lord!\"", "Later, you remember that dragons do not exist.", "4x4") },
        { "Smite", new Answer("Go and smite someone. \"Yes, my Lord!\"", "The smitten deserved it. Probably.", "4x4") },
        { "Wander", new Answer("Wander through the world for the ruin of souls. \"Yes, my Lord!\"", "So many ruins of souls in the world.", "4x4") },
        { "Weigh", new Answer("Scale", "", "") },
    };
    private static Dictionary<string, Answer> _sonAnswers = new Dictionary<string, Answer>() {
        { "Agree", new Answer("You idly agree, which seems to please him. Idiot.", "He seems content. For now.", "0x0") },
        { "Crucify", new Answer("Sacrifice", "", "0x0") },
        { "Die", new Answer("Kill", "", "0x0") },
        { "Disagree", new Answer("You disagree with him, but he does not seem to listen.", "He seems content. For now.", "0x0") },
        { "Ignore", new Answer("You ignore him.", "He huffs and puffs and leaves, eventually. He will be back.", "0x0") },
        { "Kill", new Answer("You cast a lightning bolt to his face. \"Arrrgggghhh\"", "Once the smoke clears, you are dismayed to see him still standing, more annoyed than ever. Stupid immortality.", "0x0") },
        { "Lightning", new Answer("Kill", "", "0x0") },
        { "Persuade", new Answer("You try to convince him otherwise, but he does not seem to listen.", "He seems content. For now.", "0x0") },
        { "Reinforce", new Answer("You put on your dad hat and reinforce his beliefs.", "He seems content. For now.", "0x0") },
        { "Sacrifice", new Answer("Go to Earth, my son, and sacrifice yourself to save humanity.", "He seems keen to the idea. You never hear from him again.", "0x0") },
        { "Save", new Answer("Sacrifice", "", "0x0") },
        { "Soothe", new Answer("You soothe his fragile ego.", "He seems content. For now.", "0x0") },
        { "Smite", new Answer("Kill", "", "0x0") },
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
    //     new Answer("Damn", "You damn them to hell. \"NNNNOOOOOooooooo\"", "0x0"),
    //     new Answer("Deny", "You deny their request. \"Well, thanks anyway!\"", "0x0"),
    //     new Answer("Grant", "You grant their request. \"Praise be God!\"", "0x0"),
    //     new Answer("Ignite", "They spontaneously combust. \"Aaaarrrrgggghhhhhh\"", "0x0"),
    //     new Answer("Ignore", "You ignore them. \"Hello? Are you there?\"", "0x0"),
    //     new Answer("Lightning", "You cast a lightning bolt to their face. \"Aaaarrrrgggghhhhhh\"", "0x0"),
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
    private Dictionary<Dictionary<string, Answer>, InstrumentController.INSTRUMENT> _instruments;
    private List<string> _validKeys;
    private AnimationPlayer _animPlayer;
    private Random _rng = new Random();

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
        EventController.CommandEvent -= HandleCommand;
    }

    public override void _Ready() {
        _animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        _correctAnswers = new Dictionary<Dictionary<string, Answer>, List<string>>() {
            { _prayAnswers, new List<string>() { "Ignore" } },
            { _angelAnswers, new List<string>() { "Hell", "Cast" } },
            { _sonAnswers, new List<string>() { "Sacrifice", } },
        };
        _instruments = new Dictionary<Dictionary<string, Answer>, InstrumentController.INSTRUMENT>() {
            { _prayAnswers, InstrumentController.INSTRUMENT.FOLLOWER },
            { _angelAnswers, InstrumentController.INSTRUMENT.ANGEL },
            { _sonAnswers, InstrumentController.INSTRUMENT.SON },
        };

        EventController.Send("update_color", PrimaryColor);
        EventController.Send("toggle_intro_text", false);

        UpdateAnswers(_meaningOfLifeAnswer);

        EventController.Send("update_result_text", "");
        EventController.Send("update_player_input", "");
        EventController.Send("set_instrument", InstrumentController.INSTRUMENT.PLAYER);

        _state = State.WRITE_MOL;

    }

    public override void _Process(float delta) {
        if (_state == State.CONFIRM_PROMPT) {
            if (Input.IsActionJustPressed("END_Y")) {
                string finalAnswer = _input;
                finalAnswer = finalAnswer.Right(MOL_PROMPT.Length);
                finalAnswer = finalAnswer.Trim();
                finalAnswer = finalAnswer.Capitalize();
                if (finalAnswer.Length == 1) {
                    finalAnswer = "" + char.ToUpper(finalAnswer[0]);
                } else {
                    finalAnswer = char.ToUpper(finalAnswer[0]) + finalAnswer.Substring(1);
                }
                EventController.Send("update_final_answer", finalAnswer);
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
                    EventController.Send("set_instrument", InstrumentController.INSTRUMENT.PLAYER);
                    EventController.Send("update_player_input", "");
                    EventController.Send("update_glow", 0f);
                    _state = State.WRITE_REPLY;
                    EventController.Send("show_arrow", false);
                    break;
                case State.RESPONSE_PROMPT:
                    Answer answer = _validAnswers[_input];
                    if (_validAnswers.ContainsKey(answer.response)) answer = _validAnswers[answer.response];
                    string result = answer.result;
                    EventController.Send("set_instrument", InstrumentController.INSTRUMENT.PLAYER);
                    EventController.Send("update_result_text", result);
                    EventController.Send("start_result_text");
                    EventController.Send("show_arrow", false);
                    _state = State.RESULT_WAIT;
                    break;
                case State.RESULT_PROMPT:
                    // score for correctness
                    if (_correctAnswers[_validAnswers].Contains(_input) || _correctAnswers[_validAnswers].Contains(_validAnswers[_input].response)) {
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
                            EventController.Send("play_instrument");

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
                    EventController.Send("play_instrument");
                }
            }
            return false;
        }


        switch (_state) {
            case State.WRITE_MOL:
                if (HandleInput(true) == true) {
                    EventController.Send("update_result_text", "");
                    int iconX = _rng.Next(PORTRAIT_MAX_INDEX);
                    // update valid answers
                    if (_score == 0) {
                        UpdateAnswers(_prayAnswers);
                        EventController.UpdateIcon(iconX * 2, 0);
                    } else if (_score == 1) {
                        UpdateAnswers(_angelAnswers);
                        EventController.UpdateIcon(iconX * 2, 1);
                    } else if (_score == 2) {
                        UpdateAnswers(_sonAnswers);
                        EventController.UpdateIcon(iconX * 2, 2);
                    }
                    EventController.Send("set_instrument", _instruments[_validAnswers]);
                    EventController.Send("update_icon");
                    // get random prompt
                    if (_promptsIndex == _answerPrompts[_validAnswers].Count) {
                        _promptsIndex = 0;
                        List<string> prompts = _answerPrompts[_validAnswers];
                        Utility.Shuffle(prompts, _rng);
                    }
                    string prompt = _answerPrompts[_validAnswers][_promptsIndex++];
                    EventController.Send("update_bottom_text", prompt);
                    _animPlayer.Play("Distraction");
                    _state = State.DISTRACTION_WAIT;
                }
                EventController.Send("update_glow", 0.75f * _input.Length / MOL_PROMPT.Length);
                break;
            case State.WRITE_REPLY:
                if (HandleInput(true) == true) {
                    Answer answer = _validAnswers[_input];
                    if (_validAnswers.ContainsKey(answer.response)) answer = _validAnswers[answer.response];
                    EventController.Send("update_bottom_text", answer.response);
                    EventController.Send("set_instrument", _instruments[_validAnswers]);
                    string[] split = answer.texture.Split('x');
                    EventController.UpdateIcon(int.Parse(split[0]), int.Parse(split[1]));
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
                } else if (_canEndGame == false && _input.Trim().Length > MOL_PROMPT.Length) {
                    _canEndGame = true;
                    EventController.Send("show_arrow", true);
                } else if (_canEndGame == true && _input.Trim().Length == MOL_PROMPT.Length) {
                    _canEndGame = false;
                    EventController.Send("show_arrow", false);
                }
                break;
        }
    }

    public void AnimDistractionDone() {
        EventController.Send("start_bottom_text");
    }

    public void AnimUpdateIcon() {
        EventController.Send("update_icon");
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
        _lastChar = 0;
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