using UnityEngine;

// The Game Events used across the Game.
// Anytime there is a need for a new event, it should be added here.

public static class Events
{
    // Jibbe's Events
    public static NSConfirmedEvent NSConfirmedEvent = new();
    public static NSStatsSentEvent NSStatsSentEvent = new();
    public static BroadcastStartEvent BroadcastStartEvent = new();
    public static BroadcastEndEvent BroadcastEndEvent = new();
    public static MugFilledEvent MugFilledEvent = new();
    //Sebastian's (Awesome) events
    public static GetNewsStoryEvent GetNewsStoryEvent = new();
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
public class BroadcastStartEvent : GameEvent
{
    public bool IsBroadcasting;
}

public class BroadcastEndEvent : GameEvent
{
}

/// <summary>
/// Event that fires whenever the player has filled the news presentator's coffee mug. Flag for tutorial.
/// </summary>
public class MugFilledEvent : GameEvent
{
}

// Sebastian's (awesome) events

//Event that gets fired whenever anything needs new news stories to be generated
public class GetNewsStoryEvent : GameEvent
{
}