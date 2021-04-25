using Godot;
using System;

public class InstrumentController : AudioStreamPlayer {

    [Export]
    public float Time = 0.125f;

    [Export]
    public AudioStream[] PlayerClips, AngelClips, SonClips, ChosenClips, FollowerClips;

    public enum INSTRUMENT {
        PLAYER, FOLLOWER, ANGEL, SON, CHOSEN
    }

    private Random _rng = new Random();
    private AudioStream[] _currentClips;

    private int[] _indexOrder = new int[] { 0, 1, 2, 3, 4 };
    private float _timer = 0f;
    private int _index;

    public override void _EnterTree() {
        EventController.CommandEvent += HandleCommand;
    }

    public override void _ExitTree() {
        EventController.CommandEvent -= HandleCommand;
    }

    public override void _Ready() {
        Utility.Shuffle(_indexOrder, _rng);
        _currentClips = PlayerClips;
    }

    public override void _Process(float delta) {
        if (_timer > 0) {
            _timer -= delta;
        } else if (_timer < 0) {
            //Stop();
            _timer = 0;
        }
    }

    private void HandleCommand(object[] args) {
        if (args.Length == 0) return;

        if ("play_instrument".Equals(args[0])) {
            if (_timer <= 0) { // || true) {
                Stream = _currentClips[_indexOrder[_index++]];
                Play(0f);
                _timer += Time;

                if (_index == _indexOrder.Length) {
                    Utility.Shuffle(_indexOrder, _rng);
                    _index = 0;
                }
            }
        } else if ("set_instrument".Equals(args[0])) {
            if (args.Length == 1) return;
            if (args[1] is INSTRUMENT instrument) {
                switch (instrument) {
                    case INSTRUMENT.ANGEL:
                        _currentClips = AngelClips;
                        break;
                    case INSTRUMENT.CHOSEN:
                        _currentClips = ChosenClips;
                        break;
                    case INSTRUMENT.FOLLOWER:
                        _currentClips = FollowerClips;
                        break;
                    case INSTRUMENT.PLAYER:
                        _currentClips = PlayerClips;
                        break;
                    case INSTRUMENT.SON:
                        _currentClips = SonClips;
                        break;
                }
            }
        }
    }
}