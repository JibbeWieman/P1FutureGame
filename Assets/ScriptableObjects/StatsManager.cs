using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
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

    readonly private int minMoneyStat = -100; // Allow stats to go negative to this value
    readonly private int minViewerStat = -100;
    readonly private int minAwarenessStat = -100;

    [Space(5)]
    [Header("Decrease Rate")]
    [SerializeField] private float decreaseInterval = 2.5f; // time in seconds for each stat decrease
    [SerializeField] private int decreaseAmount = 1; // Amount of decrease per tick
    #endregion

    private void Start()
    {
        // Start the coroutine that decreases stats over time
        StartCoroutine(DecreaseStatsOverTime());
    }

    public void UpdateStats(ScriptableObject script)
    {
        if (script is NS_Template nsTemplate)
        {
            // Update stat values
            UpdateStat(ref moneyStat, nsTemplate.money, minMoneyStat, maxMoneyStat, moneyStatText, moneyStatBar);
            UpdateStat(ref awarenessStat, nsTemplate.awareness, minAwarenessStat, maxAwarenessStat, awarenessStatText, awarenessStatBar);
            UpdateStat(ref viewerStat, nsTemplate.entertainment, minViewerStat, maxViewerStat, viewerStatText, viewerStatBar);
        }
        else
        {
            Debug.LogWarning("The provided ScriptableObject is not of type NS_Template.");
        }
    }

    private void UpdateStat(ref int statValue, int changeAmount, int minValue, int maxValue, TextMeshProUGUI statText, Image statBar)
    {
        int newStatValue = statValue + changeAmount;

        // Ensure the new stat value is within bounds
        newStatValue = Mathf.Clamp(newStatValue, minValue, maxValue);

        // Start the coroutine to handle both text and bar updates
        StartCoroutine(UpdateStatDisplay(statText, statBar, statValue, newStatValue, maxValue));

        // Update the internal stat value
        statValue = newStatValue;
    }

    private IEnumerator UpdateStatDisplay(TextMeshProUGUI statText, Image statBar, int startValue, int endValue, int maxValue)
    {
        float duration = Mathf.Max(0.5f, Mathf.Log10(Mathf.Abs(endValue - startValue) + 1)); // Adjust speed based on difference
        float elapsedTime = 0f;

        float startFillAmount = Mathf.Clamp01((float)startValue / maxValue);
        float endFillAmount = Mathf.Clamp01((float)endValue / maxValue);

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

    private IEnumerator DecreaseStatsOverTime()
    {
        // This coroutine will run indefinitely
        while (true)
        {
            yield return new WaitForSeconds(decreaseInterval); // Wait for the specified interval

            UpdateStat(ref viewerStat, -decreaseAmount, minViewerStat, maxViewerStat, viewerStatText, viewerStatBar);
        }
    }
}
