using UnityEngine;

// The Game Events used across the Game.
// Anytime there is a need for a new event, it should be added here.

public static class Events
{
    // Jibbe's Events
    public static NSConfirmedEvent NSConfirmedEvent = new();
    public static NSStatsSentEvent NSStatsSentEvent = new();
    public static BroadcastEvent BroadcastEvent = new();
}


// Jibbe's Events
public class NSConfirmedEvent : GameEvent
{
}

public class NSStatsSentEvent : GameEvent
{
    public NS_Template template;
}

/// <summary>
/// Event that gets fired whenever broadcast gets set to true. Also holds the IsBroadcasting boolean.
/// </summary>
public class BroadcastEvent : GameEvent
{
    public bool IsBroadcasting;
}
