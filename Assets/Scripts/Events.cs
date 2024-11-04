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
    public static TutTurnedAroundEvent TutTurnedAroundEvent = new();
    public static TutCoffeeDeliveredEvent TutCoffeeDeliveredEvent = new();
    public static TutNStoryConfirmedEvent TutNStoryConfirmedEvent = new();
    public static TutStatusEvent TutStatusEvent = new();
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
/// Event that fires whenever the player has turned towards the window. Flag for tutorial.
/// </summary>
public class TutTurnedAroundEvent : GameEvent
{
    public bool StepCompleted;
}

/// <summary>
/// Event that fires whenever the player has threw the filled mug towards Amelia. Flag for tutorial.
/// </summary>
public class TutCoffeeDeliveredEvent : GameEvent
{
    public bool StepCompleted;
}

/// <summary>
/// Event that fires whenever the player has confirmed their first story. Flag for tutorial.
/// </summary>
public class TutNStoryConfirmedEvent : GameEvent
{
    public bool TutorialFinished = false;
}

/// <summary>
/// Event that fires whenever the player has confirmed their first story. Flag for tutorial.
/// </summary>
public class TutStatusEvent : GameEvent
{
    public bool TutorialFinished = false;
}


// Sebastian's (awesome) events

//Event that gets fired whenever anything needs new news stories to be generated
public class GetNewsStoryEvent : GameEvent
{
}