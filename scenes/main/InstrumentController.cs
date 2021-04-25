using Godot;
using System;

public class InstrumentController : AudioStreamPlayer {

    [Export]
    public float Time = 0.125f;

    [Export]
    public AudioStream[] PlayerClips;

    public enum INSTRUMENT {
        PLAYER, FOLLOWER, ANGEL, SON
    }
    private INSTRUMENT _instrument;

    private Random _rng = new Random();

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
        _instrument = INSTRUMENT.PLAYER;
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
            if (_timer <= 0 || true) {
                Stream = PlayerClips[_indexOrder[_index++]];
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

            }
        }
    }
}