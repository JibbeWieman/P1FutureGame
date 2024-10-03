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

    #region GETTERS
    public List<GameObject> GetT1()
    {
        return T1;
    }

    public List<GameObject> GetT2()
    {
        return T2;
    }

    public List<GameObject> GetT3()
    {
        return T3;
    }
    #endregion
}


// Code below does the same but spawns a button which needs to be clicked to spawn the stories
/* using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TrendManager : MonoBehaviour
{
    [Header("Trends")]
    [SerializeField] private List<GameObject> T1;
    [SerializeField] private List<GameObject> T2;
    [SerializeField] private List<GameObject> T3;

    [Header("References")]
    [SerializeField] private GameObject buttonParent;  // Parent GameObject where buttons will be added
    [SerializeField] private Button buttonPrefab;      // Button prefab with TextMeshProUGUI for displaying trend topic

    private Dictionary<string, List<GameObject>> trendTopics;
    private Coroutine _trendCoroutine;

    public UnityEvent<List<GameObject>> onTrendUpdate;

    private float trendDuration;

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

        trendDuration = Random.Range(10f, 30f);
    }

    private void GetRandomTrend()
    {
        if (trendTopics.Count == 0)
        {
            Debug.LogWarning("No trending topics available.");
            return;
        }

        // Select a random category
        int randomCategoryIndex = Random.Range(0, trendTopics.Count);
        string randomCategoryKey = new List<string>(trendTopics.Keys)[randomCategoryIndex];

        // Get the randomly selected list of GameObjects
        List<GameObject> trendingTopic = trendTopics[randomCategoryKey];
        Debug.Log($"Selected Trend: {randomCategoryKey} with {trendingTopic.Count} items.");

        // Create and display a button with the trend topic as the label
        CreateTrendButton(randomCategoryKey, trendingTopic);
    }

    private void CreateTrendButton(string trendTopic, List<GameObject> trendObjects)
    {
        // Instantiate the button prefab as a child of the buttonParent object
        Button newButton = Instantiate(buttonPrefab, buttonParent.transform);
        TrendTimer(newButton);

        // Get the TextMeshProUGUI component from the button's child and set the trend topic as the button text
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = trendTopic;

        // Add an event listener to the button, so it triggers onTrendUpdate with the correct trend objects when clicked
        newButton.onClick.RemoveAllListeners();
        newButton.onClick.AddListener(() =>
        {
            onTrendUpdate?.Invoke(trendObjects);
            Debug.Log($"Button for {trendTopic} clicked!");
        });
    }

    private IEnumerator _AutoUpdateTrend()
    {
        while (true)
        {
            // Call GetRandomTrend every 20-30 seconds
            GetRandomTrend();

            // Wait for 20 to 30 seconds randomly
            float waitTime = Random.Range(20f, 30f);
            yield return new WaitForSeconds(waitTime);
        }
    }
    private IEnumerator TrendTimer(Button button)
    {
        // Wait for 30 seconds before removing the trend
        yield return new WaitForSeconds(trendDuration);

        // Destroy the button's GameObject
        if (button != null)
        {
            Destroy(button);
        }
    }

    private void OnDisable()
    {
        // Stop the coroutine when the object is disabled/destroyed
        if (trendCoroutine != null)
        {
            StopCoroutine(trendCoroutine);
        }
    }
} */