using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TrendManager : MonoBehaviour
{
    [Header("Trends")]
    [SerializeField] private List<GameObject> T1;
    [SerializeField] private List<GameObject> T2;
    [SerializeField] private List<GameObject> T3;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI trendMonitorText;

    private Dictionary<string, List<GameObject>> trendTopics;
    private List<string> activeTrends = new List<string>();

    [SerializeField] private float trendSpawnInterval;
    private float trendDuration; 
    private Coroutine trendCoroutine;

    public UnityEvent<List<GameObject>> onTrendUpdate;

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

        // Add the selected trend to the activeTrends list and update the monitor text
        if (!activeTrends.Contains(randomCategoryKey))
        {
            activeTrends.Add(randomCategoryKey);
            UpdateTrendMonitorText();

            // Randomize the trend duration
            trendDuration = Random.Range(20f, 30f);

            StartCoroutine(TrendTimer(randomCategoryKey)); // Start the timer for this trend
        }

        // Trigger the onTrendUpdate event with the selected list
        onTrendUpdate?.Invoke(trendingTopic);
    }

    private IEnumerator TrendTimer(string trendingTopic)
    {
        // Wait for x seconds before removing the trend
        yield return new WaitForSeconds(trendDuration);

        // Remove the current trend from the active trends list if it exists
        if (activeTrends.Contains(trendingTopic))
        {
            activeTrends.Remove(trendingTopic); // Remove the oldest trend (first added)
            UpdateTrendMonitorText(); // Update the text after removal
        }
    }
    private void UpdateTrendMonitorText()
    {
        trendMonitorText.text = string.Join(", ", activeTrends); // Join active trends with commas
    }

    private IEnumerator AutoUpdateTrend()
    {
        while (true)
        {
            // Call GetRandomTrend every 20-30 seconds
            GetRandomTrend();

            // Randomize trendSpawnInterval for the next iteration
            trendSpawnInterval = Random.Range(20f, 30f);

            // Wait for x sec for next trend
            yield return new WaitForSeconds(trendSpawnInterval);
        }
    }
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