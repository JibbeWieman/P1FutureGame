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
    [SerializeField] private List<GameObject> T13;
    [SerializeField] private List<GameObject> T14;
    [SerializeField] private List<GameObject> T15;
    [SerializeField] private List<GameObject> T16;
    [SerializeField] private List<GameObject> T17;
    [SerializeField] private List<GameObject> T18;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI trendMonitorText; // UI Text to display active trends

    [SerializeField]
    private SceneTypeObject newsStories;

    // Private Variables
    private Dictionary<string, List<GameObject>> trendTopics; // Dictionary to hold trend categories and their associated GameObjects
    private List<string> activeTrends = new List<string>(); // List to track currently active trends

    public UnityEvent<List<GameObject>> onTrendUpdate; // Event triggered when trends are updated

    // Timer for delaying the trend update
    private Timer trendUpdateTimer;

    #endregion

    #region METHODS

    private void Start()
    {
        EventManager.AddListener<GetNewsStoryEvent>(GetRandomTrend);

        trendTopics = new Dictionary<string, List<GameObject>>
        {
            { "AI Enables Small Companies to Compete Globally", T1 },
            { "AI Revolution replaces most jobs", T2 },
            { "AI vs. Humans Championship", T3 },
            { "AI Warfare: The New Battlefield of Automated Conflicts", T4 },
            { "All corporations are ruled by AI", T5 },
            { "Amazon pledges to make Amazonia fully carbon neutral without harming its citizens.", T6 },
            { "Flying car falls out of sky, squashes pedestrian", T7 },
            { "Fossil fuel companies go bankrupt", T8 },
            { "Mall shopper numbers going down", T9 },
            { "MEGA Corp builds entire city for workers", T10 },
            { "Michelin star lab grown food, cultivated right in front of you", T11 },
            { "New floating highways project", T12 },
            { "New giant AI server rooms", T13 },
            { "Robot bees not as good as normal bees", T14 },
            { "Space waste falls back on Earth", T15 },
            { "Space waste poses unexpected challenges for future Operations", T16 },
            { "Tsunamis strike the bottom layer of the city", T17 },
            { "Wildlife increase in lower layers", T18 },
        };
    }

    /// <summary>
    /// Selects a random trend from the available topics and activates it.
    /// </summary>
    public void GetRandomTrend(GetNewsStoryEvent getNews)
    {
        activeTrends.Clear();

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

        // Trigger the event
        onTrendUpdate?.Invoke(trendTopics[randomCategoryKey]);

        // Remove the selected topic from the list to prevent double stories
        trendTopics.Remove(randomCategoryKey);
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

    private void OnDestroy()
    {
        EventManager.RemoveListener<GetNewsStoryEvent>(GetRandomTrend);
    }

    #endregion
}

/* OLD CODE
    //[SerializeField] private float trendSpawnInterval = 25f; // Time interval for spawning new trends
    //private int maxStoryAmount = 6;   // Maximum number of stories to spawn

/// <summary>
/// Initializes the TrendManager and starts the auto-trend update process.
/// </summary>
public void Start()
{

    // Initialize the timer with the trendSpawnInterval and set autoReset to true
    //trendUpdateTimer = new Timer(trendSpawnInterval, true);

    // Immediately fetch the first random trend
    // had to temporarily move this to FMV player until the tutorial is in 

}

void GetRandomTrend
{
        //// Randomize the trend duration
        //float trendDuration = Random.Range(20f, 30f);
        //StartCoroutine(TrendTimer(randomCategoryKey, trendDuration)); // Start the timer for this trend
}

/// <summary>
    /// Coroutine that manages the duration of the trending topic.
    /// </summary>
    //private IEnumerator TrendTimer(string trendingTopic, float duration)
    //{
    //    // Wait for x seconds before removing the trend
    //    yield return new WaitForSeconds(duration);

    //    if (activeTrends.Remove(trendingTopic)) // Remove the trend if it exists
    //    {
    //        UpdateTrendMonitorText(); // Update the text after removal
    //    }
    //}
*/