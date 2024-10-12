using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewsSpawner : MonoBehaviour
{
    #region REFERENCES

    [SerializeField]
    private StatsManager statsManager; // Reference to the StatsManager

    #endregion

    #region VARIABLES

    [SerializeField, Tooltip("Time between spawns")]
    private float spawnInterval = 1f; // Time interval between story spawns

    private bool isSpawning = false;    // Indicates if spawning is currently active

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
            // Spawn the story
            GameObject instantiatedStory = Instantiate(prefab, transform.position, Quaternion.identity);
            instantiatedStory.GetComponentInChildren<TextMeshProUGUI>().text = prefab.name;

            // Wait for the specified spawn interval before the next story
            yield return new WaitForSeconds(spawnInterval);
        }
        isSpawning = false; // Reset spawning state when done
    }

    #endregion
}
