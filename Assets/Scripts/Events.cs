using UnityEngine;

// The Game Events used across the Game.
// Anytime there is a need for a new event, it should be added here.

public static class Events
{
    // Jibbe's Events
    public static NSConfirmedEvent NSConfirmedEvent = new NSConfirmedEvent();
    public static NSStatsSentEvent NSStatsSentEvent = new NSStatsSentEvent();
}


// Jibbe's Events
public class NSConfirmedEvent : GameEvent
{
}

public class NSStatsSentEvent : GameEvent
{
    public NS_Template template;
}
