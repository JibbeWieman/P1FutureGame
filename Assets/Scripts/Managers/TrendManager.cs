using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TrendManager : MonoBehaviour
{
    #region VARIABLES

    [Header("Trends")]
    [SerializeField] private List<GameObject> T1;
    [SerializeField] private List<GameObject> T2;
    [SerializeField] private List<GameObject> T3;
    [SerializeField] private List<GameObject> T4;
    [SerializeField] private List<GameObject> T5;
    [SerializeField] private List<GameObject> T6;
    [SerializeField] private List<GameObject> T7;
    [SerializeField] private List<GameObject> T8;
    [SerializeField] private List<GameObject> T9;
    [SerializeField] private List<GameObject> T10;
    [SerializeField] private List<GameObject> T11;
    [SerializeField] private List<GameObject> T12;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI trendMonitorText; // UI Text to display active trends

    [SerializeField]
    private SceneTypeObject newsStories;

    private int maxStoryAmount = 6;   // Maximum number of stories to spawn

    // Private Variables
    private Dictionary<string, List<GameObject>> trendTopics; // Dictionary to hold trend categories and their associated GameObjects
    private List<string> activeTrends = new List<string>(); // List to track currently active trends
    [SerializeField] private float trendSpawnInterval = 25f; // Time interval for spawning new trends

    public UnityEvent<List<GameObject>> onTrendUpdate; // Event triggered when trends are updated

    // Timer for delaying the trend update
    private Timer trendUpdateTimer;

    #endregion

    #region METHODS

    /// <summary>
    /// Initializes the TrendManager and starts the auto-trend update process.
    /// </summary>
    public void Start()
    {
        trendTopics = new Dictionary<string, List<GameObject>>
        {
            { "Climate Change Corpo", T1 },
            { "Climate Change Eco", T2 },
            { "Economics Corpo", T3 },
            { "Economics Eco", T4 },
            { "Energy Crisis Corpo", T5 },
            { "Energy Crisis Eco", T6 },
            { "Food Security Corpo", T7 },
            { "Food Security Eco", T8 },
            { "Migration Corpo", T9 },
            { "Migration Eco", T10 },
            { "Technology Corpo", T11 },
            { "Technology Eco", T12 },
        };

        // Initialize the timer with the trendSpawnInterval and set autoReset to true
        trendUpdateTimer = new Timer(trendSpawnInterval, true);

        // Immediately fetch the first random trend
        GetRandomTrend();
    }

    private void Update()
    {
        // Update the timer each frame
        if (trendUpdateTimer.UpdateTimer())
        {
            // When the timer elapses, get a new random trend
            if (newsStories.Objects.Count <= 0)
            {
                GetRandomTrend();
            }
        }
    }

    /// <summary>
    /// Selects a random trend from the available topics and activates it.
    /// </summary>
    public void GetRandomTrend()
    {
        Debug.Log("Picking Trend");
        if (trendTopics.Count <= 0)
        {
            Debug.LogWarning("No trending topics available.");
            return;
        }

        // Randomly pick a trend
        string randomCategoryKey = GetRandomKeyFromDictionary(trendTopics);

        // Add the selected trend to the activeTrends list and update the monitor text
        activeTrends.Add(randomCategoryKey);

        UpdateTrendMonitorText();

        // Randomize the trend duration
        float trendDuration = Random.Range(20f, 30f);
        StartCoroutine(TrendTimer(randomCategoryKey, trendDuration)); // Start the timer for this trend

        // Trigger the event
        onTrendUpdate?.Invoke(trendTopics[randomCategoryKey]);

        // Remove the selected topic from the list to prevent double stories
        trendTopics.Remove(randomCategoryKey);
    }

    /// <summary>
    /// Coroutine that manages the duration of the trending topic.
    /// </summary>
    private IEnumerator TrendTimer(string trendingTopic, float duration)
    {
        // Wait for x seconds before removing the trend
        yield return new WaitForSeconds(duration);

        if (activeTrends.Remove(trendingTopic)) // Remove the trend if it exists
        {
            UpdateTrendMonitorText(); // Update the text after removal
        }
    }

    /// <summary>
    /// Updates the trend monitor UI text to reflect active trends.
    /// </summary>
    private void UpdateTrendMonitorText()
    {
        trendMonitorText.text = string.Join("\n", activeTrends); // Join active trends with commas
    }

    /// <summary>
    /// Gets a random key from the specified dictionary.
    /// </summary>
    private string GetRandomKeyFromDictionary(Dictionary<string, List<GameObject>> dictionary)
    {
        List<string> keys = new List<string>(dictionary.Keys);
        return keys[Random.Range(0, keys.Count)];
    }

    #endregion
}
