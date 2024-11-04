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
    private AudioSource musicPlayer;


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

    //Jibbe
    [Header("Tutorial Videos")]
    [SerializeField]
    private List<VideoClip> tutorialVideos;

    private Dictionary<string, VideoClip> tutorialVideoMap = new();
    //

    [Header("Audio")]
    [SerializeField]
    private AudioClip lightsOn;

    [SerializeField]
    private AudioClip lightsOff;

    [SerializeField]
    private AudioClip broadcastBGM;

    [SerializeField]
    private AudioClip idleBGM;

    private VideoClip video;

    private VideoClip currentIdleVideo;

    private bool isBroadcasting = false;

    private Animator animator;

    BroadcastStartEvent broadcastStartEvent = Events.BroadcastStartEvent;
    TutNStoryConfirmedEvent tutNStoryConfirmed = Events.TutNStoryConfirmedEvent;
    TutStatusEvent tutStatus = Events.TutStatusEvent;

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

        LoadTutorialStatus();
    }
    private void Start()
    {
        if (!tutStatus.TutorialFinished)
        {
            // Add tutorial videos to a dictionary (could go without this, but I think it's nice to be able to give it a keyword instead of a number)
            tutorialVideoMap["Introduction"] = tutorialVideos[0];
            tutorialVideoMap["Event 1"] = tutorialVideos[1];
            tutorialVideoMap["Event 2"] = tutorialVideos[2];
            tutorialVideoMap["Event 3"] = tutorialVideos[3];

            PlayTutorialVideo("Introduction");

            EventManager.AddListener<TutTurnedAroundEvent>(TutTurnedAround);
            EventManager.AddListener<TutCoffeeDeliveredEvent>(TutCoffeeDelivered);
            EventManager.AddListener<TutNStoryConfirmedEvent>(TutNStoryConfirmed);
        }
        else
        {
            //spawn the initial 2 news stories (remove this when tutorial is in)
            GetNewsStoryEvent getNews = Events.GetNewsStoryEvent;
            EventManager.Broadcast(getNews);
            //Start playing idle videos
            PlayIdle();
        }
    }

    #region TUTORIAL CODE

    // Call this to save the tutorial status
    public void SaveTutorialStatus()
    {
        PlayerPrefs.SetInt("TutorialFinished", tutNStoryConfirmed.TutorialFinished ? 1 : 0);
        PlayerPrefs.Save(); // Ensure it's saved immediately
        Debug.Log("Tutorial status saved: " + tutNStoryConfirmed.TutorialFinished);
    }

    // Call this to load the tutorial status
    public void LoadTutorialStatus()
    {
        // Retrieve the saved status, defaulting to false (0) if no saved value is found
        tutNStoryConfirmed.TutorialFinished = PlayerPrefs.GetInt("TutorialFinished", 0) == 1;
        Debug.Log("Tutorial status loaded: " + tutNStoryConfirmed.TutorialFinished);

        tutStatus.TutorialFinished = tutNStoryConfirmed.TutorialFinished;

        EventManager.Broadcast(tutStatus);
    }

    private void PlayTutorialVideo(string eventKey)
    {
        if (tutorialVideoMap.TryGetValue(eventKey, out VideoClip tutorialVideo))
        {
            videoPlayer.clip = tutorialVideo;
            videoPlayer.Play();

            // Remove the event to prevent replays
            tutorialVideoMap.Remove(eventKey);
        }
        else
        {
            Debug.LogWarning($"No tutorial video found for event: {eventKey}");
        }
    }

    #region METHODS TO ASSIGN LISTENERS TO
    private void TutTurnedAround(TutTurnedAroundEvent evt)
    {
        PlayTutorialVideo("Event 1");
    }
    private void TutCoffeeDelivered(TutCoffeeDeliveredEvent evt)
    {
        PlayTutorialVideo("Event 2");
    }
    private void TutNStoryConfirmed(TutNStoryConfirmedEvent evt)
    {
        PlayTutorialVideo("Event 3");
    }
    #endregion
    #endregion

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
        EventManager.Broadcast(broadcastStartEvent);
        broadcastStartEvent.IsBroadcasting = true;

        //set video player's video to video assigned 
        videoPlayer.clip = video;
        //make sure it isnt looping
        videoPlayer.isLooping = false;
        musicPlayer.clip = broadcastBGM;
        musicPlayer.Play();
        //play the video
        videoPlayer.Play();
        //start checking for when to drop the new news stories
        StartCoroutine(CheckVideoTime());
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        // Last tutorial video
        TutNStoryConfirmedEvent evt3 = Events.TutNStoryConfirmedEvent;
        TutCoffeeDeliveredEvent evt2 = Events.TutCoffeeDeliveredEvent;

        if (!evt3.TutorialFinished && evt2.StepCompleted)
        {
            EventManager.Broadcast(evt3);
            evt3.TutorialFinished = true;
            SaveTutorialStatus();

            animator.SetBool("IsBroadcasting", false);
            isBroadcasting = false;

            return;
        }

        //when one idle video is finished playing, start playing a new one, make sure you arent in broadcast mode so you dont interrupt the news story
        if (isBroadcasting)
        {
            animator.SetBool("IsBroadcasting", false);
            isBroadcasting = false;
            musicPlayer.Stop();
            musicPlayer.clip = idleBGM;
            musicPlayer.Play();
            PlayIdle();
        } else
        {
            PlayIdle();
        }

        broadcastStartEvent.IsBroadcasting = false;

        BroadcastEndEvent endBroadcast = Events.BroadcastEndEvent;
        EventManager.Broadcast(endBroadcast);
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
