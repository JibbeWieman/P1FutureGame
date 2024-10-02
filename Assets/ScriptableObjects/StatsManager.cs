using System.Collections;
using UnityEngine;

public class StatsManager : NewsStoryManager
{
    #region REFERENCES

    private UIManager uiManager;

    [SerializeField]
    private SceneTypeObject sceneType;

    #endregion

    #region VARIABLES

    private bool isBroadcasting = false;
    private bool isUpdatingStat = false;  // Flag to control stat updates

    // Statistics
    [Header("Statistics")]
    [SerializeField]
    private int moneyStat;

    [SerializeField]
    private int viewerStat;

    [SerializeField]
    private int awarenessStat;

    // Maximum values for each statistic
    [Space(5)]
    [Header("Max Values")]
    [SerializeField]
    private int maxMoneyStat = 200;

    [SerializeField]
    private int maxViewerStat = 200;

    [SerializeField]
    private int maxAwarenessStat = 200;

    // Minimum values for each statistic
    private readonly int minMoneyStat = -100;
    private readonly int minViewerStat = 0;
    private readonly int minAwarenessStat = -100;

    // Viewer change rate
    [Space(5)]
    [Header("Decrease Rate")]
    [SerializeField]
    private float viewerChangeInterval = 2.5f;

    private float vidTime;

    #endregion

    private void Start()
    {
        // Get reference to the UIManager component
        uiManager = sceneType.Objects[0].GetComponent<UIManager>();
        Debug.Assert(uiManager != null);

        // Start the coroutine that decreases stats over time
        StartCoroutine(DecreaseStatsOverTime());
        StartCoroutine(SetBroadcasting());
    }

    protected override void OnNewsstoryReceived()
    {
        base.OnNewsstoryReceived();
        Debug.Log("Running Stats Script");

        int money = GetContent(news => news.money);
        int entertainment = GetContent(news => news.entertainment);
        int awareness = GetContent(news => news.awareness);

        UpdateStats(news);
    }

    public void UpdateStats(NS_Template news)
    {
        // Mark that we're currently updating stats
        isUpdatingStat = true;

        int money = news.money;
        int awareness = news.awareness;
        int entertainment = news.entertainment;

        // Update stat values
        UpdateStat(ref moneyStat, money, minMoneyStat, maxMoneyStat, uiManager.moneyStat, "Money");
        UpdateStat(ref awarenessStat, awareness, minAwarenessStat, maxAwarenessStat, uiManager.awarenessStat, "Awareness");
        UpdateStat(ref viewerStat, entertainment, minViewerStat, maxViewerStat, uiManager.viewerStat, "Viewers");

        // Stat update complete, unlock DecreaseStatsOverTime
        isUpdatingStat = false;
    }

    private void UpdateStat(ref int statValue, int changeAmount, int minValue, int maxValue, StatUI statUI, string statType)
    {
        int newStatValue = statValue + changeAmount;

        // Ensure the new stat value is within bounds
        newStatValue = Mathf.Clamp(newStatValue, minValue, maxValue);

        // Notify UIManager to update the display
        if (uiManager != null)
        {
            uiManager.UpdateStatDisplay(statUI, statValue, newStatValue, maxValue, statType);
        }

        // Update the internal stat value
        statValue = newStatValue;
    }

    private IEnumerator DecreaseStatsOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(viewerChangeInterval);

            // Only decrease stats if we're not updating them elsewhere
            if (!isUpdatingStat)
            {
                int viewerChangeAmount = Random.Range(-3, 2);
                if (!isBroadcasting) //If not broadcasting, viewers go down more (due to ads)
                    viewerChangeAmount *= 2;

                UpdateStat(ref viewerStat, viewerChangeAmount, minViewerStat, maxViewerStat, uiManager.viewerStat, "Viewers");
            }
        }
    }

    private IEnumerator SetBroadcasting()
    {
        isBroadcasting = true;

        yield return new WaitForSeconds(vidTime);

        isBroadcasting = false;
    }
}
