using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatStream : MonoBehaviour
{
    public TextMeshProUGUI chatText;  // Reference to the TMPro UI text component

    // List of predefined chat responses
    private List<string> chatResponses = new List<string>
    {
        "Hey everyone!",
        "Wow, did you see that?",
        "Lol, that's hilarious!",
        "Can't believe that happened!",
        "Good job!",
        "What’s going on?",
        "This stream is lit 🔥",
        "Any tips for beginners?",
        "I’m loving this!",
        "That was insane!",
        "When's the next update?",
        "Shout out to the devs!",
        "Awesome content, keep it up!",
        "This chat is wild!",
        "Best stream ever!",
        "Did someone just say bees are extinct? 😱",
        "AI is running the world now... and honestly, it's kind of creepy!",
        "Bruh, we're all gonna be living underwater soon, aren't we? 🌊",
        "Mall shopping is so 2020. Who needs malls when you have VR?",
        "Imagine commuting by horse again 🐴 lol #bringbackthewildwest",
        "Yo, Elon just sent another trash rocket to Mars! 🚀",
        "Bees with lasers? Sign me up! Pew pew! 🐝🔫",
        "The future is vegan, y'all! 🌱",
        "Peter Griffin said it best: ‘Lois, did climate change just fart?’ 😂",
        "Floods again? That's it, I'm investing in a boat! 🛶",
        "Who needs fossil fuels when we have solar panels now? ☀️",
        "AI warfare? More like *pew pew* digital madness 🔫🤖",
        "Mall's closed, but at least the deer are shopping there 🦌 #wildlife",
        "Bro, the planet’s a dumpster fire, but at least we got memes 🔥🤣",
        "Corporations be like 'we planted a tree' 🌳 but built 100 more AI servers 🤖",
        "Oil spills again? Alexa, fix it! Oh wait, she can’t. 🙄",
        "Yooo, bees are going corporate? Expect honey delivery in 24 hrs 🚚🍯"
    };

    private List<string> currentChat = new List<string>();  // Keeps track of current chats

    private const int maxChatLines = 10;  // Maximum number of chat lines visible

    // Method to simulate receiving a random chat message
    public void AddRandomChat()
    {
        // Simulate a random user with a username prefix
        string username = "User" + Random.Range(1000, 9999);
        string randomChat = username + ": " + chatResponses[Random.Range(0, chatResponses.Count)];

        // Add the new chat to the current chat list
        currentChat.Add(randomChat);

        // If the number of chat lines exceeds the maximum allowed, remove the oldest
        if (currentChat.Count > maxChatLines)
        {
            currentChat.RemoveAt(0);  // Remove the oldest chat message
        }

        // Update the chat UI
        UpdateChatUI();
    }

    // Method to update the chatText component with the current chat list
    private void UpdateChatUI()
    {
        // Join all current chat messages into a single string with new lines
        chatText.text = string.Join("\n", currentChat);
    }

    // You could use this to trigger new chats periodically, for example with a button or timer
    void Start()
    {
        // Example: Add a random chat message every 2 seconds
        InvokeRepeating("AddRandomChat", 0f, 2f);
    }
}
