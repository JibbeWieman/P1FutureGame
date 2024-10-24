using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class FMVPlayer : NewsStoryManager
{
    private TrendManager trendManager;

    [SerializeField]
    private SceneTypeObject gameManagerType;

    private VideoClip video;

    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private int negativeDelay;


    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        trendManager = gameManagerType.Objects[0].GetComponent<TrendManager>();
    }
    public void OnNewsstoryReceived(NS_Template news)
    {
        this.news = news;
        Debug.Log("Running FMV Script");
        VideoClip video = GetContent(news => news.fmv);
        StartCoroutine(DelayedPlayNews(video));
    }

    private void PlayNews(VideoClip video)
    {

        if (!gameObject.activeInHierarchy)
        {
            Debug.LogError("FMVPlayer is not active when trying to play news.");
            return;
        }
        videoPlayer.clip = video;
        videoPlayer.Play();
        StartCoroutine(CheckVideoTime());
    }

    private IEnumerator CheckVideoTime()
    {
        while (!videoPlayer.isPlaying)
        {
            yield return null;
        }
        double spawnTime = videoPlayer.length - negativeDelay;

        while (videoPlayer.isPlaying)
        {
            if (videoPlayer.time >= spawnTime)
            {
                trendManager.GetRandomTrend();

            }
            yield return null;
        }
    }

    IEnumerator DelayedPlayNews(VideoClip video)
    {
        yield return new WaitForSeconds(0.3f); // Small delay to ensure everything is active
        PlayNews(video);
    }
}
