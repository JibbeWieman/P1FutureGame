using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewsSpawner : MonoBehaviour
{
    #region NEWS STORY SPAWNING

    #region VARIABLES

    [SerializeField, Tooltip("Time between news story spawns")]
    private float spawnInterval = 1f; // Time interval between story spawns

    private bool isSpawning = false; // Indicates if news stories are currently spawning

    [SerializeField]
    private AudioSource audioSource;
    #endregion

    #region METHODS

    /// <summary>
    /// Initiates the story generation process if not already spawning.
    /// </summary>
    /// <param name="prefabs">List of story prefabs to spawn.</param>
    public void GenerateStory(List<GameObject> prefabs)
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnStory(prefabs));
        }
    }

    #endregion

    #region COROUTINES

    /// <summary>
    /// Coroutine to spawn stories based on the provided prefabs.
    /// </summary>
    /// <param name="prefabs">List of story prefabs to spawn.</param>
    /// <returns>IEnumerator for coroutine control.</returns>
    private IEnumerator SpawnStory(List<GameObject> prefabs)
    {
        isSpawning = true; // Set spawning to true at the start

        foreach (GameObject prefab in prefabs)
        {

            audioSource.Play();
            // Spawn the news story
            GameObject instantiatedStory = Instantiate(prefab, transform.position, Quaternion.identity);
            instantiatedStory.GetComponentInChildren<TextMeshProUGUI>().text = prefab.name;

            // Wait for the specified spawn interval before the next story
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false; // Reset spawning state when done
    }

    #endregion

    #endregion // End of News Story Spawning Section

    #region MONEY & MUG SPAWNING

    #region REFERENCES    
    private StatsManager statsManager;

    [Header("Money Spawning")]
    [SerializeField]
    private GameObject moneyPrefab; // Prefab to spawn for money
    
    [SerializeField]
    private GameObject moneyAwardPrefab; // Prefab to spawn for money

    #endregion

    #region VARIABLES

    private Stack<GameObject> spawnedMoneyPrefabs = new(); // Stack to track spawned money prefabs
    private int previousMoneyStat = 0; // To track the previous money stat

    [SerializeField]
    private int moneyStackWorth = 10000;

    [Header("Mug Spawning")]
    [SerializeField]
    private SceneTypeObject ST_Mug;

    [SerializeField]
    private GameObject MugPrefab;

    #endregion

    #region METHODS
    private void Start()
    {
        statsManager = FindObjectOfType<StatsManager>();
        previousMoneyStat = statsManager.MoneyStat; // Initialize with the starting money stat
    }

    private void Update()
    {
        //Instantiate(newsStory, transform, false); YT Bit
        UpdateMoneyPrefabs();

        // Ensure that there always is a mug to prevent the player getting stuck in the tutorial
        // in case they drop the mug outside of the level
        if (ST_Mug.Objects.Count <= 0)
        {
            Instantiate(MugPrefab, transform.position, Quaternion.identity);
        }

        // Spawn award if player has earned enough money
        if (statsManager.MoneyStat >= 100000 && moneyAwardPrefab != null)
        {
            Instantiate(moneyAwardPrefab, transform.position, Quaternion.identity);
            moneyAwardPrefab = null;
        }
    }

    /// <summary>
    /// Spawns money prefabs incrementally, one per each 100 money gained.
    /// </summary>
    public void UpdateMoneyPrefabs()
    {
        // Calculate the difference between the current and previous moneyStat
        int moneyDifference = statsManager.MoneyStat - previousMoneyStat;

        // Check if we gained at least 100
        while (moneyDifference >= moneyStackWorth)
        {
            // Spawn one money prefab for each 200 increment
            GameObject newMoneyPrefab = Instantiate(moneyPrefab, transform.position, Quaternion.identity);
            spawnedMoneyPrefabs.Push(newMoneyPrefab);

            // Reduce the difference by 200 and update previousMoneyStat accordingly
            moneyDifference -= moneyStackWorth;
            previousMoneyStat += moneyStackWorth;
        }

        // If we somehow lose money and need to remove prefabs (optional logic)
        int moneyToSpawn = statsManager.MoneyStat / moneyStackWorth;
        while (spawnedMoneyPrefabs.Count > moneyToSpawn)
        {
            GameObject lastSpawned = spawnedMoneyPrefabs.Pop();
            Destroy(lastSpawned);
        }
    }

    #endregion

    #endregion

}
