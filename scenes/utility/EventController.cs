using Godot;
using System;
using System.Collections.Generic;

public class EventController : Node {

    public static event CommandEventHandler CommandEvent;
    public delegate void CommandEventHandler(object[] args);
    public static void Send(params object[] args) {if (CommandEvent != null) CommandEvent.Invoke(args); }

    public static event ValidInputEventHandler ValidInputEvent;
    public delegate void ValidInputEventHandler(List<string> validInputs);
    public static void UpdateValidInputs(List<string> validInputs) { if (ValidInputEvent != null) ValidInputEvent.Invoke(validInputs); }

}