using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : NewsStoryManager
{
    #region VARIABLES
    [Header("References")]
    public TextMeshProUGUI moneyStatText;
    public TextMeshProUGUI viewerStatText;
    public TextMeshProUGUI awarenessStatText;

    public Image moneyStatBar;
    public Image viewerStatBar;
    public Image awarenessStatBar;

    [Space(5)]
    [Header("Statistics")]
    [SerializeField] private int moneyStat;
    [SerializeField] private int viewerStat;
    [SerializeField] private int awarenessStat;

    [Space(5)]
    [Header("Max Values")]
    [SerializeField] private int maxMoneyStat = 100;
    [SerializeField] private int maxViewerStat = 100;
    [SerializeField] private int maxAwarenessStat = 100;
    #endregion

    protected override void OnNewsstoryReceived()
    {
        base.OnNewsstoryReceived();
        Debug.Log("Running Stats Script");
        int money = GetContent(news => news.money);
        int entertainment = GetContent(news => news.entertainment);
        int awareness = GetContent(news => news.awareness);
        Debug.Log($"Money: {money}, Entertainment: {entertainment}, Awareness: {awareness}");
        UpdateStat(ref moneyStat, money, maxMoneyStat, moneyStatText, moneyStatBar);
        UpdateStat(ref awarenessStat, awareness, maxAwarenessStat, awarenessStatText, awarenessStatBar);
        UpdateStat(ref viewerStat, entertainment, maxViewerStat, viewerStatText, viewerStatBar);

    }
    private void UpdateStat(ref int statValue, int changeAmount, int maxValue, TextMeshProUGUI statText, Image statBar)
    {
        int newStatValue = statValue + changeAmount;

        // Ensure the new stat value is within bounds
        newStatValue = Mathf.Clamp(newStatValue, 0, maxValue);

        // Start the coroutine to handle both text and bar updates
        StartCoroutine(UpdateStatDisplay(statText, statBar, statValue, newStatValue, maxValue));

        // Update the internal stat value
        statValue = newStatValue;
    }

    private IEnumerator UpdateStatDisplay(TextMeshProUGUI statText, Image statBar, int startValue, int endValue, int maxValue)
    {
        float duration = Mathf.Max(0.5f, Mathf.Log10(Mathf.Abs(endValue - startValue) + 1)); // Adjust speed based on difference
        float elapsedTime = 0f;

        float startFillAmount = (float)startValue / maxValue;
        float endFillAmount = (float)endValue / maxValue;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;

            // Interpolate both text and bar value based on percentageComplete
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, percentageComplete));
            float currentFillAmount = Mathf.Lerp(startFillAmount, endFillAmount, percentageComplete);

            // Update text and bar
            statText.text = currentValue.ToString();
            statBar.fillAmount = currentFillAmount;

            yield return null;
        }

        // Ensure final values are set
        statText.text = endValue.ToString();
        statBar.fillAmount = endFillAmount;
    }
}