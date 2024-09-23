using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsShoot : MonoBehaviour
{
    [SerializeField] private SceneTypeObject NewsStory;
    [SerializeField] private GameObject newsStoryPrefab;
    [SerializeField] private float spawnInterval = 1f; // Time interval between spawns

    private bool isSpawning = false;

    private void Update()
    {
        if (NewsStory.Objects.Count < 10 && !isSpawning)
        {
            StartCoroutine(SpawnNewsStory(spawnInterval));
        }
    }

    private IEnumerator SpawnNewsStory(float spawnInterval)
    {
        isSpawning = true;

        Instantiate(newsStoryPrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(spawnInterval);

        isSpawning = false;
    }
}
