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
    [Header("Type")]
    [SerializeField]
    private SceneTypeObject gameManagerType;

    [Header("References")]
    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Light light1;

    //[SerializeField]
    //private Light light2;

    [SerializeField]
    private Light light3;

    [Header("Delays")]
    [SerializeField]
    private int negativeDelay;

    [SerializeField]
    private float lightDelay;

    [Header("Idle Videos")]
    [SerializeField]
    private List<VideoClip> idleVideos;

    [Header("Audio")]
    [SerializeField]
    private AudioClip lightsOn;

    [SerializeField]
    private AudioClip lightsOff;

    private VideoClip video;

    private VideoClip currentIdleVideo;

    private bool isBroadcasting = false;

    private Animator animator;

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

        //start playing the news story after turning the lights off and on
        StartCoroutine(ToggleLights(() => PlayNews(video)));

    }

    private void PlayIdle()
    {
        //set the state to not broadcasting
        animator.SetBool("IsBroadcasting", false);
        isBroadcasting = false;
        StartCoroutine(ToggleLights(PlayIdleVideo));

    }

    private void PlayIdleVideo()
    {
        //make sure there's actual videos loaded in (null check)
        if (idleVideos == null || idleVideos.Count == 0)
        {
            Debug.LogError("no idle videos");
            return;
        }

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

    private IEnumerator ToggleLights(Action onComplete)
    {

        audioSource.clip = lightsOff;
        audioSource.Play();
        //Turn off lights
        light1.enabled = false;
        //light2.enabled = false;
        light3.enabled = false;

        //Wait
        yield return new WaitForSeconds(lightDelay);
        onComplete?.Invoke();

        audioSource.clip = lightsOn;
        audioSource.Play();
        //Turn on lights
        light1.enabled = true;
        //light2.enabled = true;
        light3.enabled = true;
    }

    private void OnDestroy()
    {
        //remove listeners when not used
        EventManager.RemoveListener<NSStatsSentEvent>(OnNewsstoryReceived);
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
