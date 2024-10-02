using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewsSpawner : MonoBehaviour
{
    [SerializeField] 
    private StatsManager statsManager;

    [SerializeField, Tooltip("Time between spawns")] 
    private float spawnInterval = 1f;
    
    public int maxStoryAmount = 100; 

    private bool isSpawning = false;

    public void GenerateStory(List<GameObject> prefabs)
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnStory(prefabs));
        }
    }

    private IEnumerator SpawnStory(List<GameObject> prefabs)
    {
        foreach(GameObject prefab in prefabs)
        {
            // Spawn the story
            GameObject instantiatedStory = Instantiate(prefab, transform.position, Quaternion.identity);
            instantiatedStory.GetComponentInChildren<TextMeshProUGUI>().text = prefab.name;

            // Wait X amount of seconds for the next story to spawn
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
