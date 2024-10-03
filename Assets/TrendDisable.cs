using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrendDisable : MonoBehaviour
{
    private enum Trend { None, T1, T2, T3 }
    private Trend trend = Trend.None;

    private TrendManager trendManager;
    void Start()
    {
        //find trendmanager and make sure its there
        trendManager = FindObjectOfType<TrendManager>();
        if (trendManager != null)
        {

        }
        else
        {
            Debug.LogError("No TrendManager found in the scene.");
        }
    }

    public void DisableOthers()
    {
        List<GameObject> otherStories = null;
        Debug.Log($"{gameObject.name} Disabling others");
        //Detect the trend and get other GameObjects in the same trend
        if (trendManager.GetT1().Contains(gameObject))
        {
            trend = Trend.T1;
            otherStories = trendManager.GetT1(); 
        }
        else if (trendManager.GetT2().Contains(gameObject))
        {
            trend = Trend.T2;
            otherStories = trendManager.GetT2(); 
        }
        else if (trendManager.GetT3().Contains(gameObject))
        {
            trend = Trend.T3;
            otherStories = trendManager.GetT3(); 
        }

        //list other stories in same trend
        if (trend != Trend.None)
        {
            if (otherStories != null && otherStories.Count > 0)
            {
                Debug.Log($"Other objects in {trend}:");
                foreach (GameObject story in otherStories)
                {
                    if (story != gameObject) //Exclude self 
                    {
                        Debug.Log($"{story.name} Disabled");
                        NewsStoryClass newsStoryClass = story.GetComponentInChildren<NewsStoryClass>();
                        newsStoryClass._used = true;
                    }
                }
            }
        }
        else
        {
            Debug.Log($"{gameObject.name} is not in any trend");
        }
    }
}

