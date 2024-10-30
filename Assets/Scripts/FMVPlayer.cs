using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using System;
using UnityEditor.Rendering;

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

    [SerializeField]
    private List<VideoClip> idleVideos;

    private Animator animator;

    private VideoClip currentIdleVideo;

    private bool isBroadcasting = false;


    private void Awake()
    {
        //get component references
        videoPlayer = GetComponent<VideoPlayer>();
        animator = GetComponent<Animator>();
        //set the audio settings 
        videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //subscribe to the external event to trigger new news stories
        EventManager.AddListener<NSStatsSentEvent>(OnNewsstoryReceived);
        //link the looppointreached videoplayer event to OnVideoFinished
        videoPlayer.loopPointReached += OnVideoFinished;
    }
    private void Start()
    {
        //spawn the initial 2 news stories (remove this when tutorial is in)
        GetNewsStoryEvent getNews = Events.GetNewsStoryEvent;
        EventManager.Broadcast(getNews);
        //Start playing idle videos
        PlayIdle();
    }
    public void OnNewsstoryReceived(NSStatsSentEvent evt)
    {
        //get the NS_TEMPLATE scriptableobject from the event
        this.news = evt.template;
        //get the videoclip assigned to the scriptableobject
        VideoClip video = GetContent(news => news.fmv);
        //set the state to broadcast mode
        animator.SetBool("IsBroadcasting", true);
        isBroadcasting = true;

        //start playing the news story
        PlayNews(video);
    }

    private void PlayIdle()
    {
        //make sure there's actual videos loaded in (null check)
        if (idleVideos == null || idleVideos.Count == 0)
        {
            Debug.LogError("no idle videos");
            return;
        }


        //set the state to not broadcasting
        animator.SetBool("IsBroadcasting", false);
        isBroadcasting = false;

        //set the current idle video to a random one picked from the list
        currentIdleVideo = idleVideos[UnityEngine.Random.Range(0, idleVideos.Count)];

        //assign that idle video to the videoplayer
        videoPlayer.clip = currentIdleVideo;
        //make sure it isnt looping
        videoPlayer.isLooping = false;
        //play the video
        videoPlayer.Play();
    }

    private void PlayNews(VideoClip video)
    {
        //set video player's video to video assigned 
        videoPlayer.clip = video;
        //make sure it isnt looping
        videoPlayer.isLooping = false;
        //play the video
        videoPlayer.Play();
        //start checking for when to drop the new news stories
        StartCoroutine(CheckVideoTime());
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        //when one idle video is finished playing, start playing a new one, make sure you arent in broadcast mode so you dont interrupt the news story
        if (isBroadcasting)
        {
            animator.SetBool("IsBroadcasting", false);
            isBroadcasting = false;
            PlayIdle();
        } else
        {
            PlayIdle();
        }
    }
    private IEnumerator CheckVideoTime()
    {
        //make sure video is actually playing
        while (!videoPlayer.isPlaying)
        {
            yield return null;
        }

        //take video length and remove the offset (how many seconds before the video you want the news stories to drop)
        double spawnTime = videoPlayer.length - negativeDelay;

        //spawn news stories when the specefied time before the news story's end is reached
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


    private void OnDestroy()
    {
        //remove listeners when not used
        EventManager.RemoveListener<NSStatsSentEvent>(OnNewsstoryReceived);
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
