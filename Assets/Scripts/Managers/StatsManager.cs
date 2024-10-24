using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatsManager : NewsStoryManager
{
    #region REFERENCES

    protected UIManager uiManager;
    protected PoliticalCompass politicalCompass;

    [SerializeField]
    private SceneTypeObject ST_GameManager;

    #endregion

    #region VARIABLES

    [SerializeField, Tooltip("Flag to control ad money generation")]
    protected bool isBroadcasting = false;
    [Tooltip("Flag to control stat updates")]
    protected bool isUpdatingStat = false;

    [Header("Statistics")]
    [SerializeField] private int _moneyStat;   // Private serialized field for moneyStat
    [SerializeField] private int _viewerStat;   // Private serialized field for viewerStat
    [SerializeField] private int _awarenessStat; // Private serialized field for awarenessStat

    // Public properties for stats
    public int moneyStat => _moneyStat;       // Public getter for moneyStat
    public int viewerStat => _viewerStat;     // Public getter for viewerStat
    public int awarenessStat => _awarenessStat; // Public getter for awarenessStat

    // Maximum values for each statistic
    [Space(5)]
    [Header("Max Values")]
    [SerializeField]
    protected int maxMoneyStat = 1000000;
    [SerializeField]
    protected int maxViewerStat = 1000000;
    [SerializeField]
    protected int maxAwarenessStat = 100;

    // Minimum values for each statistic
    protected readonly int minMoneyStat = -100;
    protected readonly int minViewerStat = 0;
    protected readonly int minAwarenessStat = -100;

    // Viewer change rate
    [Space(5)]
    [Header("Stat Decay")]
    [SerializeField]
    private float viewerChangeInterval = 2.5f;

    [System.Serializable]
    public struct AdTypeStats
    {
        public float adMoneyRate;      // Rate at which money is earned per viewer
        public float adViewerRate;     // Rate at which viewers are impacted
        public float adAwarenessRate;  // Rate at which awareness is impacted
    }

    [Header("Ad System")]

    [SerializeField]
    private List<AdTypeStats> adTypes;

    private AdTypeStats currentAdType;
    
    //[SerializeField, Tooltip("Money earned per viewer per second")]
    //private float adMoneyRate = 0.1f;

    private float vidTime = 10f;

    [Space(5)]
    [Header("Live Level Changes")]
    [SerializeField]
    private List<GameObject> liveChanges;

    #endregion

    #region UNITY METHODS

    /// <summary>
    /// Initializes the StatsManager by getting the UIManager reference and starting necessary coroutines.
    /// </summary>
    private void Start()
    {
        uiManager = ST_GameManager.Objects[0].GetComponent<UIManager>();
        Debug.Assert(uiManager != null);

        politicalCompass = ST_GameManager.Objects[0].GetComponent<PoliticalCompass>();
        Debug.Assert(politicalCompass != null);

        //StartCoroutine(StatsDecay());
        StartCoroutine(DelayGenerateAdMoney());
    }

    #endregion

    #region STATISTIC MANAGEMENT

    /// <summary>
    /// Handles the actions taken when a news story is received, including logging and updating stats.
    /// </summary>
    public void OnNewsstoryReceived(NS_Template news)
    {
        this.news = news;
        Debug.Log("Running Stats Script");

        int money = GetContent(news => news.money);
        int entertainment = GetContent(news => news.entertainment);
        int awareness = GetContent(news => news.awareness);

        UpdateStats(news);
        StartCoroutine(SetBroadcasting());
    }

    /// <summary>
    /// Updates the statistics based on the provided news data.
    /// </summary>
    /// <param name="news">The news data used to update the statistics.</param>
    public void UpdateStats(NS_Template news)
    {
        isUpdatingStat = true;

        int money = news.money;
        int awareness = news.awareness;
        int entertainment = news.entertainment;

        UpdateStat(ref _moneyStat, money, minMoneyStat, maxMoneyStat, uiManager.moneyStat, "Money");
        UpdateStat(ref _awarenessStat, awareness, minAwarenessStat, maxAwarenessStat, uiManager.awarenessStat, "Awareness");
        UpdateStat(ref _viewerStat, entertainment, minViewerStat, maxViewerStat, uiManager.viewerStat, "Viewers");

        politicalCompass.UpdatePoliticalPosition(news);

        isUpdatingStat = false;
    }

    /// <summary>
    /// Updates an individual statistic and notifies the UIManager to refresh the display.
    /// </summary>
    /// <param name="statValue">The reference to the statistic value to update.</param>
    /// <param name="changeAmount">The amount to change the statistic by.</param>
    /// <param name="minValue">The minimum value the statistic can take.</param>
    /// <param name="maxValue">The maximum value the statistic can take.</param>
    /// <param name="statUI">The UI element representing the statistic.</param>
    /// <param name="statType">The name of the statistic for logging purposes.</param>
    private void UpdateStat(ref int statValue, int changeAmount, int min, int max, StatUI statUI, string statType)
    {
        int newStatValue = Mathf.Clamp(statValue + changeAmount, min, max);

        if (uiManager != null)
        {
            uiManager.UpdateStatDisplay(statUI, statValue, newStatValue, max, statType);
        }

        statValue = newStatValue;
    }

    #endregion

    #region AD MANAGEMENT
    /// <summary>
    /// Changes the current ad type.
    /// </summary>
    /// <param name="adTypeIndex">The index of the ad type to switch to.</param>
    public void ChangeAdType(int adTypeIndex)
    {
        Debug.Log($"Trying to change ad type to index: {adTypeIndex}");

        if (adTypeIndex >= 0 && adTypeIndex < adTypes.Count)
        {
            currentAdType = adTypes[adTypeIndex]; // Assign the corresponding struct
            Debug.Log($"Ad type changed successfully to index {adTypeIndex}");
        }
        else
        {
            Debug.LogWarning($"Invalid ad type index: {adTypeIndex}");
        }
    }

    private IEnumerator DelayGenerateAdMoney()
    {
        yield return new WaitForSeconds(2f);  // Ensure everything is set up first
        StartCoroutine(RunAd());
    }

    /// <summary>
    /// Generates money from ads over time when not broadcasting.
    /// </summary>
    private IEnumerator RunAd()
    {
        while (true)
        {
            Debug.Log("Generating ad money loop active.");

            if (isBroadcasting)
                Debug.Log("Not making money :(");

            if (!isBroadcasting)
            {
                Debug.Log("Making monayyyyyyyy");

                int moneyEarned = Mathf.RoundToInt(_viewerStat * currentAdType.adMoneyRate);
                int awarenessEarned = Mathf.RoundToInt(_viewerStat * currentAdType.adAwarenessRate);
                int viewersEarned = Mathf.RoundToInt(_viewerStat * currentAdType.adViewerRate);

                UpdateStat(ref _moneyStat, moneyEarned, minMoneyStat, maxMoneyStat, uiManager.moneyStat, "Money");
                UpdateStat(ref _viewerStat, viewersEarned, minViewerStat, maxViewerStat, uiManager.viewerStat, "Viewers");
                UpdateStat(ref _awarenessStat, awarenessEarned, minAwarenessStat, maxAwarenessStat, uiManager.awarenessStat, "Awareness");
                
                //_moneyStat = Mathf.Clamp(_moneyStat + moneyEarned, minMoneyStat, maxMoneyStat);
                //_viewerStat = Mathf.Clamp(_viewerStat + viewersEarned, minViewerStat, maxViewerStat);
                //_awarenessStat = Mathf.Clamp(_awarenessStat + awarenessEarned, minAwarenessStat, maxAwarenessStat);
            }

            yield return new WaitForSeconds(.5f);
        }
    }
    #endregion

    #region COROUTINES

    /// <summary>
    /// Decreases viewer stats over time based on a specified interval.
    /// </summary>
    private IEnumerator StatsDecay()
    {
        while (true)
        {
            yield return new WaitForSeconds(viewerChangeInterval);

            if (!isUpdatingStat)
            {
                int viewerChange = Random.Range(-3, 2) * (isBroadcasting ? 1 : 2);
                UpdateStat(ref _viewerStat, viewerChange, minViewerStat, maxViewerStat, uiManager.viewerStat, "Viewers");
            }
        }
    }

    /// <summary>
    /// Sets the broadcasting state and manages the timing of broadcasting.
    /// </summary>
    private IEnumerator SetBroadcasting()
    {
        isBroadcasting = true;
        foreach (var obj in liveChanges) obj.SetActive(isBroadcasting);

        yield return new WaitForSeconds(vidTime);

        isBroadcasting = false;
        foreach (var obj in liveChanges) obj.SetActive(isBroadcasting);
    }

    #endregion
}
