using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct StatUI
{
    public TextMeshProUGUI statText;
    public Image posStatBar; // Bar for positive values
    public Image negStatBar; // Bar for negative values
}

public class UIManager : Manager
{
    [Header("Stat UI Elements")]
    public StatUI moneyStat;
    public StatUI viewerStat;
    public StatUI awarenessStat;

    public override void Start()
    {
        base.Start();
    }

    public override void Pause()
    {
        throw new System.NotImplementedException();
    }

    // Method to update stat display
    public void UpdateStatDisplay(StatUI statUI, int startValue, int endValue, int maxValue, string statType)
    {
        StartCoroutine(UpdateStatDisplayCoroutine(statUI, startValue, endValue, maxValue, statType));
    }

    private IEnumerator UpdateStatDisplayCoroutine(StatUI statUI, int startValue, int endValue, int maxValue, string statType)
    {
        float duration = Mathf.Max(0.5f, Mathf.Log10(Mathf.Abs(endValue - startValue) + 1));
        float elapsedTime = 0f;

        (float startPosFill, float startNegFill) = GetFillAmounts(startValue, maxValue);
        (float endPosFill, float endNegFill) = GetFillAmounts(endValue, maxValue);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;

            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, percentageComplete));
            float currentPosFillAmount = Mathf.Lerp(startPosFill, endPosFill, percentageComplete);
            float currentNegFillAmount = Mathf.Lerp(startNegFill, endNegFill, percentageComplete);

            // Update UI elements
            UpdateUI(statUI, currentValue, currentPosFillAmount, currentNegFillAmount, statType);

            yield return null;
        }

        // Ensure final values are set
        UpdateUI(statUI, endValue, endPosFill, endNegFill, statType);
    }

    // Tuple for storing the fill amounts
    private (float positiveFill, float negativeFill) GetFillAmounts(int value, int maxValue)
    {
        float positiveFill = value < 0 ? 0 : Mathf.Clamp01((float)value / maxValue);
        float negativeFill = value < 0 ? Mathf.Clamp01((float)value / -maxValue) : 0;
        return (positiveFill, negativeFill);
    }

    private void UpdateUI(StatUI statUI, int currentValue, float currentPosFillAmount, float currentNegFillAmount, string statType)
    {
        statUI.statText.text = $"{statType}: {currentValue}";
        statUI.posStatBar.fillAmount = currentPosFillAmount; // Positive fill
        statUI.negStatBar.fillAmount = currentNegFillAmount; // Negative fill
    }
}
