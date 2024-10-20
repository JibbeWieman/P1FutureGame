using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

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


    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        trendManager = gameManagerType.Objects[0].GetComponent<TrendManager>();
    }
    protected override void OnNewsstoryReceived()
    {
        VideoClip video = GetContent(news => news.fmv);
        PlayNews(video);
    }

    private void PlayNews(VideoClip video)
    {
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
}
