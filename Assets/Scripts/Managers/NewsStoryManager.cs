using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NewsStoryManager : MonoBehaviour
{
    //reference the news story
    protected NS_Template news;
    //recieves scriptableobject thru unityevent
    public UnityEvent<NS_Template> startNews;

    public void AssignNewsStory(NS_Template newsStory)
    {
        Debug.Log("Event Received");
        //news = newsStory;
        Debug.Log($"News story received: Money = {newsStory.money}, Entertainment = {newsStory.entertainment}, Awareness = {newsStory.awareness}");

        startNews.Invoke(newsStory);
    }


    //function used by child scripts to actually get data from the passed through news stories
    protected T GetContent<T>(System.Func<NS_Template, T> getter)
    {
        if (news != null)
        {
            return getter(news);
        }
        Debug.Log("No News Story Is Assigned");
        return default;
    }

}
