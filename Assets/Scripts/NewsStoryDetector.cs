//using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NewsStoryDetector : MonoBehaviour
{
    [SerializeField] private UnityEvent _newsStoryConfirm;
    private bool _storyAvaliable = false;
    private Component newsStoryComponent;
    private void OnTriggerEnter(Collider other)
    {
        newsStoryComponent = other;
        _storyAvaliable = true;
    }

    public void ConfirmButtonPressed()
    {
        if (_storyAvaliable)
        {
            var newsStory = newsStoryComponent.GetComponent<NewsStoryClass>();
            //NS_Template template = newsStoryComponent.GetComponent<NS_Template>();
            newsStory.SendStats();
            _storyAvaliable = false;

            //NSConfirmedEvent nsConfirmedEvent = Events.NSConfirmedEvent;
            //EventManager.Broadcast(nsConfirmedEvent);
        }
    }
}
