using System.Collections;
using UnityEngine;

public class StatsManager : NewsStoryManager
{
    #region REFERENCES

    protected UIManager uiManager;
    protected PoliticalCompass politicalCompass;

    [SerializeField]
    private SceneTypeObject ST_GameManager;

    #endregion

    #region VARIABLES

    [Tooltip("Flag to control ad money generation")]
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
    [Header("Decrease Rate")]
    [SerializeField]
    private float viewerChangeInterval = 2.5f;

    [Header("Ad System")]
    [SerializeField, Tooltip("Money earned per viewer per second")]
    private float adMoneyRate = 0.1f;

    private Coroutine adMoneyCoroutine;
    private float vidTime = 10f;

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

        StartCoroutine(DecreaseStatsOverTime());
        adMoneyCoroutine = StartCoroutine(DelayStartGenerateAdMoney());
    }

    private IEnumerator DelayStartGenerateAdMoney()
    {
        yield return new WaitForSeconds(1);  // Ensure everything is set up first
        adMoneyCoroutine = StartCoroutine(GenerateAdMoney());
    }

    #endregion

    #region STATISTIC MANAGEMENT

    /// <summary>
    /// Handles the actions taken when a news story is received, including logging and updating stats.
    /// </summary>
    protected override void OnNewsstoryReceived()
    {
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
    private void UpdateStat(ref int statValue, int changeAmount, int minValue, int maxValue, StatUI statUI, string statType)
    {
        int newStatValue = statValue + changeAmount;
        newStatValue = Mathf.Clamp(newStatValue, minValue, maxValue);

        if (uiManager != null)
        {
            uiManager.UpdateStatDisplay(statUI, statValue, newStatValue, maxValue, statType);
        }

        statValue = newStatValue;
    }

    #endregion

    #region COROUTINES

    /// <summary>
    /// Decreases viewer stats over time based on a specified interval.
    /// </summary>
    private IEnumerator DecreaseStatsOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(viewerChangeInterval);

            if (!isUpdatingStat)
            {
                int viewerChangeAmount = Random.Range(-3, 2);
                if (!isBroadcasting)
                    viewerChangeAmount *= 2;

                UpdateStat(ref _viewerStat, viewerChangeAmount, minViewerStat, maxViewerStat, uiManager.viewerStat, "Viewers");
            }
        }
    }

    /// <summary>
    /// Sets the broadcasting state and manages the timing of broadcasting.
    /// </summary>
    private IEnumerator SetBroadcasting()
    {
        isBroadcasting = true;

        yield return new WaitForSeconds(vidTime);

        isBroadcasting = false;
    }

    /// <summary>
    /// Generates money from ads over time when not broadcasting.
    /// </summary>
    private IEnumerator GenerateAdMoney()
    {
        while (true)
        {
            if (isBroadcasting)
                Debug.Log("Not making money :(");

            if (!isBroadcasting)
            {
                Debug.Log("Making monayyyyyyyy");
                int moneyEarned = Mathf.RoundToInt(_viewerStat * adMoneyRate);
                _moneyStat = Mathf.Clamp(_moneyStat + moneyEarned, minMoneyStat, maxMoneyStat);

                UpdateStat(ref _moneyStat, moneyEarned, minMoneyStat, maxMoneyStat, uiManager.moneyStat, "Money");
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    #endregion
}
