using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NewsStoryDetector : MonoBehaviour
{
    [SerializeField] private UnityEvent _newsStoryConfirm;

    private void OnTriggerEnter(Collider other)
    {
        var newsStory = other.GetComponent<NewsStoryClass>();
        if (newsStory != null )
        {
            newsStory.SendStats();
        }
    }
}
