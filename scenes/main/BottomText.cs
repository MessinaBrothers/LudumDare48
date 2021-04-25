using Godot;
using System;

public class BottomText : RichTextLabel {

    [Export]
    public float PrintCharTime = 1.0f;

    private bool _isOn = false;
    private float _timer = 0f;
    private int _quotationMarkCount = 0;
    private float DelayComma = 0.51f;
    private float DelayPeriod = 0.94f;
    private string _originalText;

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _ExitTree() {
        EventController.CommandEvent -= HandleCommand;
    }

    public override void _Process(float delta) {
        if (_isOn == false) return;

        if (Input.IsActionJustPressed("confirm") && VisibleCharacters > 4) {
            VisibleCharacters = _originalText.Length - 1;
        }

        _timer += delta;
        if (_timer >= PrintCharTime) {
            _timer -= PrintCharTime;

            VisibleCharacters += 1;

            EventController.Send("play_instrument");

            //GD.Print(_originalText[VisibleCharacters - 1]);
            //GD.Print(_quotationMarkCount);
            //GD.Print(string.Format("{0} - {1} < {2}", VisibleCharacters, _quotationMarkCount, _originalText.Length));
            if (VisibleCharacters < _originalText.Length) {
                float delay = 0;
                switch (_originalText[VisibleCharacters - 1]) {
                    case '\"':
                        _quotationMarkCount += 1;
                        if (_quotationMarkCount > 1) {
                            EventController.Send("set_instrument", InstrumentController.INSTRUMENT.PLAYER);
                        }
                        break;
                    case ',':
                        delay += DelayComma;
                        break;
                    case '.':
                        delay += DelayPeriod;
                        break;
                    case '!':
                        delay += DelayPeriod;
                        break;
                    case '?':
                        delay += 0;
                        break;
                }
                if (delay > 0 &&
                    VisibleCharacters < _originalText.Length &&
                    _originalText[VisibleCharacters] == '\"') {
                    VisibleCharacters += 2;
                    _timer -= delay;
                    EventController.Send("set_instrument", InstrumentController.INSTRUMENT.PLAYER);
                }
            }

            if (VisibleCharacters >= _originalText.Length) {
                _isOn = false;
                EventController.Send("done_bottom_text");
                VisibleCharacters += 1;
            }

        }
    }

    // private char GetPlayerInput() {
    //     return '0';
    // }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("update_bottom_text".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is string s) {
                _originalText = s;
                string[] split = s.Split('\"');
                if (split.Length == 1) {
                    BbcodeText = s;
                } else {
                    BbcodeText = "[i]" + split[0] + "\"" + split[1] + " \"[/i]" + split[2];
                }
                _quotationMarkCount = 0;
                VisibleCharacters = 0;
            }
        } else if ("start_bottom_text".Equals(args[0])) {
            _isOn = true;
            _timer = PrintCharTime;
        }
    }

}