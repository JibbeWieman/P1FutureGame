using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using System;

public class FMVPlayer : NewsStoryManager
{
    private TrendManager trendManager;

    [SerializeField]
    private SceneTypeObject gameManagerType;

    private VideoClip video;

    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private int negativeDelay;


    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //trendManager = gameManagerType.Objects[0].GetComponent<TrendManager>();
        gameObject.SetActive(true);
        EventManager.AddListener<NSStatsSentEvent>(OnNewsstoryReceived);
    }
    private void Start()
    {
        GetNewsStoryEvent getNews = Events.GetNewsStoryEvent;
        EventManager.Broadcast(getNews);
    }
    public void OnNewsstoryReceived(NSStatsSentEvent evt)
    {
        this.news = evt.template;
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
                GetNewsStoryEvent GetNews = Events.GetNewsStoryEvent;
                EventManager.Broadcast(GetNews);
                break;
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
