using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsStoryManager : MonoBehaviour
{
    //reference the news story
    protected NS_Template news;
    //recieves scriptableobject thru unityevent

    public void AssignNewsStory(NS_Template newsStory)
    {
        Debug.Log("Event Received");
        news = newsStory;
        Debug.Log($"News story received: Money = {news.money}, Entertainment = {news.entertainment}, Awareness = {news.awareness}");
        
        OnNewsstoryReceived();
    }

    //runs whenever the unityevent fires, child scripts can run stuff based off it
    protected virtual void OnNewsstoryReceived()
    {
        Debug.Log("Running Stats Script In Manager");

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
