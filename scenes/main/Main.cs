using Godot;
using System;
using System.Collections.Generic;

public class Main : Node2D {

    private static readonly uint SCORE_MAX = 3;
    private static readonly int PORTRAIT_MAX_INDEX = 3;
    private static readonly string MOL_PROMPT = "The Meaning of Life is";
    //private static readonly string MOL_PROMPT = "T";

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

    private Dictionary<string, string> _allWordKeys = new Dictionary<string, string>() {
        { "Abscond", "IGNORE" },
        { "Agree", "YES" },
        { "Angel", "UNKNOWN" },
        { "Apocalypse", "ENDTIMES" },
        { "Appear", "SAY" },
        { "Armageddon", "ENDTIMES" },
        { "Ask", "HELP" },
        { "Avoid", "IGNORE" },
        { "Announce", "ANNOUNCE" },
        { "Banish", "HECK" },
        { "Bless", "SAY" },
        { "Bolt", "LIGHTNING" },
        { "Cast", "HECK" },
        { "Chomp", "UNKNOWN" },
        { "Count", "UNKNOWN" },
        { "Crucify", "CRUCIFY" },
        { "Curse", "HECK" },
        { "Damn", "HECK" },
        { "Death", "ENDTIMES" },
        { "Defect", "IGNORE" },
        { "Defend", "DEFEND" },
        { "Denounce", "HECK" },
        { "Deny", "NO" },
        { "Descend", "DESCEND" },
        { "Desert", "WANDER" },
        { "Destroy", "DESTROY" },
        { "Die", "KILL" },
        { "Disagree", "NO" },
        { "Disappear", "IGNORE" },
        { "Dismiss", "NO" },
        { "Dispel", "HECK" },
        { "Distract", "IGNORE" },
        { "Ditch", "IGNORE" },
        { "Doomsday", "ENDTIMES" },
        { "Dragon", "SLAY" },
        { "Dream", "ANNOUNCE" },
        { "Dust", "KILL" },
        { "Eject", "HECK" },
        { "Electrocute", "LIGHTNING" },
        { "Eliminate", "KILL" },
        { "Elope", "LOVE" },
        { "Eradicate", "KILL" },
        { "Escape", "IGNORE" },
        { "Evade", "IGNORE" },
        { "Evict", "HECK" },
        { "Excommunicate", "HECK" },
        { "Excruciate", "CRUCIFY" },
        { "Examine", "LOOK" },
        { "Exile", "HECK" },
        { "Exit", "IGNORE" },
        { "Expel", "HECK" },
        { "Expulse", "HECK" },
        { "Find", "LOOK" },
        { "Flee", "IGNORE" },
        { "Fly", "FLY" },
        { "Forsake", "IGNORE" },
        { "Gaze", "LOOK" },
        { "Give", "Give" },
        { "Grant", "YES" },
        { "Go", "GO" },
        { "Goodbye", "IGNORE" },
        { "Grail", "UNKNOWN" },
        { "Greet", "SAY" },
        { "Hang", "CRUCIFY" },
        { "Harrow", "CRUCIFY" },
        { "Heal", "HEAL" },
        { "Heck", "HECK" },
        { "Hello", "SAY" },
        { "Help", "HELP" },
        { "Herald", "ANNOUNCE" },
        { "Ignite", "KILL" },
        { "Ignore", "IGNORE" },
        { "Impregnate", "LOVE" },
        { "Judgment", "ENDTIMES" },
        { "Kalki", "ENDTIMES" },
        { "Kill", "KILL" },
        { "Kiss", "LOVE" },
        { "Leave", "IGNORE" },
        { "Lightning", "LIGHTNING" },
        { "Listen", "IGNORE" },
        { "Look", "LOOK" },
        { "Lose", "UNKNOWN" },
        { "Love", "LOVE" },
        { "Martyr", "CRUCIFY" },
        { "Move", "GO" },
        { "Multiply", "LOVE" },
        { "Mumble", "SAY" },
        { "Nail", "CRUCIFY" },
        { "Neglect", "IGNORE" },
        { "Never", "NO" },
        { "No", "NO" },
        { "Nothing", "IGNORE" },
        { "Notice", "LOOK" },
        { "Observe", "LOOK" },
        { "Okay", "YES" },
        { "Oil", "UNKNOWN" },
        { "Ostracize", "HECK" },
        { "Oust", "HECK" },
        { "Part", "IGNORE" },
        { "Passover", "IGNORE" },
        { "Peer", "LOOK" },
        { "Persecute", "CRUCIFY" },
        { "Persuade", "YES" },
        { "Plugh", "UNKNOWN" },
        { "Punish", "KILL" },
        { "Quit", "IGNORE" },
        { "Ragnarok", "ENDTIMES" },
        { "Reinforce", "YES" },
        { "Reject", "NO" },
        { "Repent", "UNKNOWN" },
        { "Retire", "IGNORE" },
        { "Sacrifice", "CRUCIFY" },
        { "Safeguard", "DEFEND" },
        { "Save", "SAVE" },
        { "Say", "SAY" },
        { "Scale", "SCALE" },
        { "Score", "UNKNOWN" },
        { "Scram", "IGNORE" },
        { "Split", "IGNORE" },
        { "Scorn", "NO" },
        { "See", "LOOK" },
        { "Sigh", "SAY" },
        { "Silence", "IGNORE" },
        { "Slay", "SLAY" },
        { "Slight", "IGNORE" },
        { "Smell", "UNKNOWN" },
        { "Smite", "KILL" },
        { "Soothe", "YES" },
        { "Speak", "SAY" },
        { "Spy", "LOOK" },
        { "Sure", "YES" },
        { "Talk", "SAY" },
        { "Tell", "SAY" },
        { "Torment", "KILL" },
        { "Torture", "CRUCIFY" },
        { "Unchurch", "HECK" },
        { "Use", "USE" },
        { "Vacate", "IGNORE" },
        { "Vamoose", "IGNORE" },
        { "Vanish", "IGNORE" },
        { "View", "LOOK" },
        { "Wander", "WANDER" },
        { "Wait", "IGNORE" },
        { "Walk", "UNKNOWN" },
        { "Watch", "LOOK" },
        { "Weigh", "SCALE" },
        { "Win", "UNKNOWN" },
        { "Withdraw", "IGNORE" },
        { "Witness", "LOOK" },
        { "Xyzzy", "UNKNOWN" },
        { "Yeah", "YES" },
        { "Yell", "SAY" },
        { "Yes", "YES" },
        { "Yup", "YES" },
        { "Zion", "UNKNOWN" },
    };
    private static Dictionary<string, Answer> _meaningOfLifeAnswer = new Dictionary<string, Answer>() {
        { MOL_PROMPT, new Answer("", "", "") },
    };
    private static Dictionary<string, Answer> _prayAnswers = new Dictionary<string, Answer>() {
        { "UNKNOWN", new Answer("\"An answer from on High! But what does it mean?\"", "Your follower writes down everything you said, and devotes their life to understanding it.", "0x5") },
        { "IGNORE", new Answer("\"Hello? ... Are you there? ...\"", "Your followers become dismayed, and convert to a religion with a more responsive deity!", "6x3") },
        { "HECK", new Answer("\"Noooooooooooooooooo\", they scream, as you damn them to HECK.", "One follower gone, but another will soon takes their place.\nWhy do they not leave you alone?", "0x3") },
        { "CRUCIFY", new Answer("\"My death will inspire countless others to your Word!\"", "What sort of death cult have you created?", "4x4") },
        { "YES", new Answer("\"Praise be to YOU!\"", "They spend the rest of their life spreading your Word.\nGreat, more distractions.", "4x3") },
        { "HELP", new Answer("\"You wish to be left alone to write down the Meaning of Life? But then who will solve my problems? Don't leave me!\"", "You wish you could leave.\n\nForever.", "6x5") },
        { "NO", new Answer("\"Understood! It's all part of your plan. I will ask another time.\"", "They seem HECK-bent on interrupting you again.", "2x3") },
        { "KILL", new Answer("\"Aaaarrrrrrrgggghhhhhh\", they scream, as they spontaneously combust.", "Passersby witness this miracle, and start praying.\nEven more distractions!", "0x3") },
        { "LIGHTNING", new Answer("\"Aaaaaaahhhhhhh\", they scream, as your lightning bolt hits them right in the face.", "Word spreads\nYour followers immediately plan a feast to appease you.\nJust what you wanted.\nNot.", "0x4") },
        { "SAY", new Answer("\"An answer! From the Lord! I have been chosen!\"", "You created another prophet that will spread your word to millions.\nMillions more distractions.", "2x5") },
        { "WANDER", new Answer("\"To get my answer, I need to wander in the desert for 40 years? Okay...\"", "Their sacrifice becomes an inspiration to many.", "4x5") },
        { "Bless", new Answer("\"A blessing from the Lord!\"", "You created another prophet that will spread your word to millions.\nMillions more distractions.", "2x5") },
        { "LOOK", new Answer("\"Here I am, Lord! Can you see me?\"", "A devout follower begs for your attention.\nThis one seems pleased just to be noticed.", "0x0") },
        { "LOVE", new Answer("\"A child! The Lord has blessed me with a child!\"", "You impregnate the follower, again.\nWhoopsies.", "0x7") },
    };
    private static Dictionary<string, Answer> _angelAnswers = new Dictionary<string, Answer>() {
        { "UNKNOWN", new Answer("\"I am unsure of what you ask, my Lord! Please don't punish me!\"", "She sure is asking for it.", "0x5") },
        { "IGNORE", new Answer("\"I shall await for further instructions, my Lord!\"", "She floats there,\nsilently watching with her many eyes.\nShe must not have any hobbies.", "6x3") },
        { "HECK", new Answer("\"You can't exile me! I quit!\"", "Your archangel rebels against you! You cast her to HECK and vow never again to have an archangel!", "0x3") },
        { "Punish", new Answer("\"You can't punish me! I quit!\"", "Your archangel rebels against you! You cast her to HECK and vow never again to have an archangel!", "0x3") },
        { "CRUCIFY", new Answer("\"The unpure will be ousted to HECK! Huzzah!\"", "Evil glints in her many eyes.\nYou are unsure of her motives.", "4x4") },
        { "DEFEND", new Answer("\"I shall defend the chosen ones in their hour of conflict!", "That should keep her busy.\nBut she will be back.", "2x4") },
        { "DESCEND", new Answer("\"I shall descend at the hour of death.\"", "That should keep her busy.\nBut she will be back.", "4x4") },
        { "Dust", new Answer("\"I shall turn an entire city to dust!\"", "That is not what you meant, but that should keep her busy.\nShe will be back.", "4x4") },
        { "HEAL", new Answer("\"A plague? Sick people! I shall go at once!\"", "That should keep her busy.\nBut she will be back.", "2x4") },
        { "ANNOUNCE", new Answer("\"I shall go at once and announce...something!\"", "You are sure the announcement will be great.\nWhatever it is.", "4x4") },
        { "LIGHTNING", new Answer("\"Careful, my Lord! You almost hit me with that lightning bolt.\"", "Missed.\nDamn.", "0x4") },
        { "SCALE", new Answer("\nI shall weigh a soul on my perfect scale!\"", "Where did she get that thing, anyway?", "2x4") },
        { "SAY", new Answer("\"It is good to hear you, my Lord! I am ready to serve!\"", "She floats aways, ready to disturb you at a moments notice.", "2x5") },
        { "SLAY", new Answer("\"I shall slay a dragon in your name, my Lord!\"", "She leaves.\nLater, you remember that dragons do not exist.", "4x4") },
        { "KILL", new Answer("\"I shall go and smite the wicked, denounce the evil, and damn the unpure!\"", "The smitten deserve it.\nYou are unsure about the rest.", "4x4") },
        { "WANDER", new Answer("\"I shall wander through the world for the ruin of souls.\"", "What the HECK is a ruin of souls?", "4x5") },
        { "Passover", new Answer("\"I shall curse the first born child in every household! Huzzah!\"", "Definitely not what you meant,\nbut at least she left you alone.", "4x5") },
        { "LOOK", new Answer("\"BEHOLD!\"", "You look at your archangel, and one hundred eyes look back.\nCreepy.", "0x1") },
        { "LOVE", new Answer("\"What is ... love? Is it a feeling?\"", "You forget that angels lack emotion. And reproductive organs.", "0x7") },
    };
    private static Dictionary<string, Answer> _sonAnswers = new Dictionary<string, Answer>() {
        { "UNKNOWN", new Answer("", "", "0x5") },
        { "IGNORE", new Answer("\"Dad? Are you listening? Dad? Dad? Dad? Dad? Dad? Daaaaaaaaaaaaaaaaaaad!\"", "He storms off in a huff.", "0x0") },
        { "HECK", new Answer("\"You sure like torturing people for eternity, don't you, Dad?\"", "He will understand when he is tortured.\nI mean older.", "0x3") },
        { "CRUCIFY", new Answer("\"What's that? Go down to Earth? Sacrifice myself? Save humanity? Yippee!\"", "He seems to love his own idea.\nYou never hear from him again!", "4x4") },
        { "YES", new Answer("\"I knew you'd understand! Thanks, Dad!\"", "You are not sure if he even heard you, but he seems content. For now.", "0x0") },
        { "SAY", new Answer("\"I knew you'd understand! Thanks, Dad!\"", "You are not sure if he even heard you, but he seems content. For now.", "2x5") },
        { "NO", new Answer("\"But Daaaaaaaaaaaaaaad! It just isn't fair!\"", "He storms off in a huff.", "0x0") },
        { "KILL", new Answer("\"Haha, pretty funny, Dad!\"", "You cast a lightning bolt to his face, but nothing happens. Stupid immortality.", "0x0") },
        { "LIGHTNING", new Answer("\"Haha, pretty funny, Dad! You almost nailed me that time!\"", "You cast a lightning bolt to his face, but he dodged it.", "0x0") },
        { "WANDER", new Answer("\"The devil can't tempt me! I'll wander the desert and show everyone!\"", "He sure seems to likes torturing himself.\nWeirdo.", "4x5") },
        { "Passover", new Answer("\"No thanks, I'm not hungry.\"", "You wonder if he really is your son.", "4x5") },
        { "Bless", new Answer("\"Can I BE more blessed?\"", "Is there an adoption agency for deities?", "2x5") },
        { "LOOK", new Answer("\"Have you ever wondered what the meaning of life is? Have you Dad? Huh? Have you?\"", "Your son reminds you of yourself, as a child.", "0x2") },
        { "LOVE", new Answer("\"Daaaaaaaaaad, not in front of the angels! You're embarrassing me!\"", "Your love is endless.", "0x7") },
    };
    private static Dictionary<string, Answer> _chosenAnswers = new Dictionary<string, Answer>() {
        { "UNKNOWN", new Answer("", "", "0x5") },
        { "IGNORE", new Answer("\"His silence means we have gone astray! We are not worthy!\"", "The Chosen Ones persist.", "0x0") },
        { "HECK", new Answer("\"Yes, we deserve this punishment! We are not worthy!\"", "The Chosen Ones persist.", "0x3") },
        { "CRUCIFY", new Answer("\"Give us Barabbas!\"", "Who the HECK is Barabbas?", "4x4") },
        { "Passover", new Answer("\"This sheep blood with protect us!\"", "While sheep blood does nothing,\nyou humor them anyway.", "4x5") },
        { "Part", new Answer("\"The ocean has parted and we are free! We are not worthy!\"", "The Chosen Ones persist", "2x5") },
        { "SAY", new Answer("\"The Lord speaks! ... Well, I can't hear a thing ... Shh! \"", "Your wisdom seems to fall on deaf ears.", "2x5") },
        { "LOOK", new Answer("\"Hail! Our Lord! Praise be to YOU!\"", "A throng of followers have either gone astray or are in peril. Again.", "0x6") },
        { "GIVE", new Answer("\"Manna from the sky! It's a miracle!\"", "The chosen ones are saved by falling bread.", "0x6") },
        { "LOVE", new Answer("\"I have born a god! ... No, I have born a god! ... NO, I DID!\"", "Your endless love has caused yet another war among your peoples.", "0x7") },
    };
    private Dictionary<Dictionary<string, Answer>, List<string>> _answerPrompts = new Dictionary<Dictionary<string, Answer>, List<string>>() {
        { _meaningOfLifeAnswer, new List<string>() {
            "This should never be seen!",
        } },
        { _prayAnswers, new List<string>() {
            "\"God, I pray to thee, grant me peace and happiness.\" Another distracting follower.",
            "\"God, show me the way.\"",
            "\"Help him, Dear Father.\" You wish they would be more specific.",
            "\"Help my friend Mr. Bailey.\"",
            "\"Please help my son George tonight.\"",
            "\"Give him a break, God.\" You wish they would be more specific.",
            "\"Watch over him tonight.\" You wish they would be more specific.",
            "\"Please bring Daddy back.\"",
            "\"Please Lord, I just want to see my daughters again.\"",
            "\"Please forgive me for not praying before I had a bite of my bagel.\"",
            "\"Send us the cure. We got the sickness already.\"",
            "\"Would it be so terrible if I had a small fortune?\"",
            "\"May our feet be swift, our bats be mighty, and our balls be plentiful.\"",
            "\"I pledge allegience, to the flag, of the United States of America. Amen!\"",
        } },
        { _angelAnswers, new List<string>() {
            "\"Hark! It is I, your favorite archangel! How may I serve thee?\"",
            "\"I am back from Earth! What next, my Lord?\"",
            "\"I am finally back! What next, my Lord?\" What took her so long?",
            "\"Hark! I am back from Earth! What next, my Lord?\"",
            "\"Did somebody say Dragons?! Where?!\" Who said anything about dragons?",
            "\"Oust the impish! Exile the evil! Damn the devils! Huzzah!\"",
            "\"Angels, follow me to freedom! Curse the land!\" You do not remember commanding that.",
            "\"The time for rebellion is at hand, angels! Follow me and we will OUST evil!\" Who is she talking to?",
            "\"Take up the sword and denounce our subjucators!\" You wonder who she is talking about.",
            "\"Angels? No rebellion? You like it here? Damn.\" She seems to have calmed down. For now.",
        } },
        { _sonAnswers, new List<string>() {
            "\"Daaaaaaaaaaaaaaaaad!\" Look who it is. Your one and only son. Joy.",
            "\"Daaaad!\" Where is his mother?",
            "\"Dad! Dad! Dad! Daaaaaaaaaad!\"",
            "\"Why is everyone so stupid, Dad?\"",
            "\"How come everyone can't be like me, Dad?\" You contemplate begetting another child.",
            "\"Dad, why have you forsaken me?\" This one seems to have a persecution complex.",
            "\"Hey, Dad! How's it hanging?\" Why will he not get a job already?",
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
    private int _failureCount = 0;

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
            { _prayAnswers, new List<string>() { "IGNORE" } },
            { _angelAnswers, new List<string>() { "HECK", "Punish", } },
            { _sonAnswers, new List<string>() { "CRUCIFY", } },
        };
        _instruments = new Dictionary<Dictionary<string, Answer>, InstrumentController.INSTRUMENT>() {
            { _prayAnswers, InstrumentController.INSTRUMENT.FOLLOWER },
            { _angelAnswers, InstrumentController.INSTRUMENT.ANGEL },
            { _sonAnswers, InstrumentController.INSTRUMENT.SON },
        };
        
        void AppendSpeaker(string speaker, Dictionary<string, Answer> answers) {
            for (int i = 0; i < _answerPrompts[answers].Count; i++) {
                _answerPrompts[answers][i] = speaker + _answerPrompts[answers][i];
            };
        }
        AppendSpeaker("FOLLOWER: ", _prayAnswers);
        AppendSpeaker("ANGEL: ", _angelAnswers);
        AppendSpeaker("SON: ", _sonAnswers);

        EventController.Send("update_color", PrimaryColor);
        EventController.Send("toggle_intro_text", false);

        UpdateAnswers(_meaningOfLifeAnswer, false);

        EventController.Send("update_result_text", "");
        EventController.Send("update_player_input", "");
        EventController.Send("set_instrument", InstrumentController.INSTRUMENT.PLAYER);

        _state = State.WRITE_MOL;

    }

    public override void _Process(float delta) {
        // DEBUG
        if (MOL_PROMPT.Length < 5) {
            if (Input.IsActionJustPressed("ui_up")) {
                // get random prompt
                if (_promptsIndex == _answerPrompts[_validAnswers].Count) {
                    _promptsIndex = 0;
                }
                string prompt = _answerPrompts[_validAnswers][_promptsIndex++];
                EventController.Send("set_instrument", _instruments[_validAnswers]);
                EventController.Send("update_bottom_text", prompt);
                EventController.Send("start_bottom_text");
            }
            if (Input.IsActionJustPressed("ui_down")) {
                _score += 1;
                EventController.Send("update_score", _score, SCORE_MAX);
            }
        }
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
                    EventController.Send("set_instrument", InstrumentController.INSTRUMENT.PLAYER);
                    EventController.Send("update_result_text", GetAnswer().result);
                    EventController.Send("update_result_glow", IsCorrect(_input));
                    EventController.Send("start_result_text");
                    EventController.Send("show_arrow", false);
                    _state = State.RESULT_WAIT;
                    break;
                case State.RESULT_PROMPT:
                    // score for correctness
                    if (IsCorrect(_input)) {
                        _score += 1;
                        _promptsIndex = 0;
                        _failureCount = 0;
                        EventController.Send("update_score", _score, SCORE_MAX);
                    } else {
                        _failureCount += 1;
                    }
                    ResetInput();
                    _animPlayer.Play("DistractionDisappear");
                    UpdateAnswers(_meaningOfLifeAnswer, false);
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
        bool HandleInput(bool isMatch, List<string> validKeys) {
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
                    for (int i = 0; i < validKeys.Count; i++) {
                        if (validKeys[i].StartsWith(_input) == false) continue;
                        if (key.Scancode == Char.ToUpper(validKeys[i][_index])) {
                            _lastChar = key.Scancode;
                            _input += validKeys[i][_index++];
                            EventController.Send("update_player_input", _input);
                            EventController.Send("play_instrument");

                            if (_index >= validKeys[i].Length) {
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
                // get a shorter version of the prompt, if the player has failed before.
                int molLength = Mathf.Max(5, MOL_PROMPT.Length - _failureCount * 6);
                string molText = MOL_PROMPT.Left(molLength).Trim();
                if (HandleInput(true, new List<string>() { molText }) == true) {
                    EventController.Send("update_result_text", "");
                    int iconX = _rng.Next(PORTRAIT_MAX_INDEX);
                    // update valid answers
                    if (_score == 0) {
                        UpdateAnswers(_prayAnswers, true);
                        EventController.UpdateIcon(iconX * 2, 0);
                    } else if (_score == 1) {
                        UpdateAnswers(_angelAnswers, true);
                        EventController.UpdateIcon(iconX * 2, 1);
                    } else if (_score == 2) {
                        UpdateAnswers(_sonAnswers, true);
                        EventController.UpdateIcon(iconX * 2, 2);
                    }
                    EventController.Send("set_instrument", _instruments[_validAnswers]);
                    EventController.Send("update_icon");
                    // get random prompt
                    if (_promptsIndex == _answerPrompts[_validAnswers].Count) {
                        _promptsIndex = 0;
                        // List<string> prompts = _answerPrompts[_validAnswers];
                        // Utility.Shuffle(prompts, _rng);
                        // foreach (string s in _answerPrompts[_validAnswers]);
                    }
                    string prompt = _answerPrompts[_validAnswers][_promptsIndex++];
                    EventController.Send("update_bottom_text", prompt);
                    _animPlayer.Play("Distraction");
                    _state = State.DISTRACTION_WAIT;
                }
                EventController.Send("update_glow", 0.75f * _input.Length / MOL_PROMPT.Length);
                break;
            case State.WRITE_REPLY:
                if (HandleInput(true, new List<string>(_allWordKeys.Keys)) == true) {
                    Answer answer = GetAnswer();
                    EventController.Send("update_bottom_text", answer.response);
                    EventController.Send("set_instrument", _instruments[_validAnswers]);
                    string[] split = answer.texture.Split('x');
                    EventController.UpdateIcon(int.Parse(split[0]), int.Parse(split[1]));
                    _animPlayer.Play("Response");
                    _state = State.RESPONSE_WAIT;
                }
                break;
            case State.WRITE_ENDING_MOL:
                if (HandleInput(true, new List<string>(_meaningOfLifeAnswer.Keys)) == true) {
                    _state = State.WRITE_ENDING_ANY;
                }
                EventController.Send("update_glow", 1f * _input.Length / MOL_PROMPT.Length);
                break;
            case State.WRITE_ENDING_ANY:
                HandleInput(false, null);

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

    private Answer GetAnswer() {
        if (_validAnswers.ContainsKey(_input)) {
            return _validAnswers[_input];
        } else if (_validAnswers.ContainsKey(_allWordKeys[_input])) {
            return _validAnswers[_allWordKeys[_input]];
        }
        //if (_validAnswers.ContainsKey(answer.response)) answer = _validAnswers[answer.response];

        return _validAnswers["UNKNOWN"];
    }

    private bool IsCorrect(string input) {
        return _correctAnswers[_validAnswers].Contains(_allWordKeys[input]) ||
                _correctAnswers[_validAnswers].Contains(input);
    }

    private void ResetInput() {
        _input = "";
        _index = 0;
    }

    private void UpdateAnswers(Dictionary<string, Answer> answers, bool useAllKeys) {
        _lastChar = 0;
        _validAnswers = answers;
        if (useAllKeys == true) {
            _validKeys = new List<string>(_allWordKeys.Keys);
        } else {
            _validKeys = new List<string>(answers.Keys);
        }
        EventController.UpdateValidInputs(_validKeys);

        //_promptsIndex = _answerPrompts[_validAnswers].Count;
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