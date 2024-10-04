using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TrendManager : MonoBehaviour
{
    #region VARIABLES

    [Header("Trends")]
    [SerializeField] private List<GameObject> T1; // List for Trend 1
    [SerializeField] private List<GameObject> T2; // List for Trend 2
    [SerializeField] private List<GameObject> T3; // List for Trend 3

    [Header("References")]
    [SerializeField] private TextMeshProUGUI trendMonitorText; // UI Text to display active trends

    // Private Variables
    private Dictionary<string, List<GameObject>> trendTopics; // Dictionary to hold trend categories and their associated GameObjects
    private List<string> activeTrends = new List<string>(); // List to track currently active trends
    [SerializeField] private float trendSpawnInterval; // Time interval for spawning new trends
    private Coroutine trendCoroutine; // Coroutine for auto-updating trends

    public UnityEvent<List<GameObject>> onTrendUpdate; // Event triggered when trends are updated

    #endregion

    #region METHODS

    /// <summary>
    /// Initializes the TrendManager and starts the auto-trend update process.
    /// </summary>
    public void Start()
    {
        trendTopics = new Dictionary<string, List<GameObject>>
        {
            { "Trend01", T1 },
            { "Trend02", T2 },
            { "Trend03", T3 }
        };

        // Start the auto-trend update process
        trendCoroutine = StartCoroutine(AutoUpdateTrend());
    }

    /// <summary>
    /// Selects a random trend from the available topics and activates it.
    /// </summary>
    private void GetRandomTrend()
    {
        if (trendTopics.Count == 0)
        {
            Debug.LogWarning("No trending topics available.");
            return;
        }

        // Randomly pick a trend
        string randomCategoryKey = GetRandomKeyFromDictionary(trendTopics);

        // Check if it's already active
        if (activeTrends.Contains(randomCategoryKey)) return;

        // Add the selected trend to the activeTrends list and update the monitor text
        activeTrends.Add(randomCategoryKey);
        UpdateTrendMonitorText();

        // Randomize the trend duration
        float trendDuration = Random.Range(20f, 30f);
        StartCoroutine(TrendTimer(randomCategoryKey, trendDuration)); // Start the timer for this trend

        // Trigger the event
        onTrendUpdate?.Invoke(trendTopics[randomCategoryKey]);
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
        trendMonitorText.text = string.Join(", ", activeTrends); // Join active trends with commas
    }

    /// <summary>
    /// Coroutine to automatically update trends at random intervals.
    /// </summary>
    private IEnumerator AutoUpdateTrend()
    {
        while (true)
        {
            GetRandomTrend();

            // Randomize the next trendSpawnInterval
            trendSpawnInterval = Random.Range(20f, 30f);
            yield return new WaitForSeconds(trendSpawnInterval);
        }
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
