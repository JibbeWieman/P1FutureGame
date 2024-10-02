using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct StatUI
{
    public TextMeshProUGUI statText;
    public Image posStatBar;  // Bar for positive values
    public Image negStatBar;  // Bar for negative values
}

public class UIManager : Manager
{
    [Header("Stat UI Elements")]
    public StatUI moneyStat;
    public StatUI viewerStat;
    public StatUI awarenessStat;

    // Dictionary to store update flags for each stat type
    private Dictionary<string, bool> isUpdatingFlags;

    public override void Start()
    {
        base.Start();
        // Initialize the dictionary with default values
        isUpdatingFlags = new Dictionary<string, bool>
        {
            { "Money", false },
            { "Viewers", false },
            { "Awareness", false }
        };
    }

    public override void Pause()
    {
        throw new System.NotImplementedException();
    }

    // Method to update stat display; only starts if the respective stat is not being updated
    public void UpdateStatDisplay(StatUI statUI, int startValue, int endValue, int maxValue, string statType, bool forceUpdate = false)
    {
        if (!isUpdatingFlags[statType] || forceUpdate)
        {
            StartCoroutine(UpdateStatDisplayCoroutine(statUI, startValue, endValue, maxValue, statType));
        }
    }

    // Coroutine to handle stat lerp animation
    private IEnumerator UpdateStatDisplayCoroutine(StatUI statUI, int startValue, int endValue, int maxValue, string statType)
    {
        // Mark the respective stat as updating
        isUpdatingFlags[statType] = true;

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

        // Mark the update as complete
        isUpdatingFlags[statType] = false;
    }

    // Helper method to calculate the positive and negative fill amounts
    private (float positiveFill, float negativeFill) GetFillAmounts(int value, int maxValue)
    {
        float positiveFill = value < 0 ? 0 : Mathf.Clamp01((float)value / maxValue);
        float negativeFill = value < 0 ? Mathf.Clamp01((float)value / -maxValue) : 0;
        return (positiveFill, negativeFill);
    }

    // Method to update the UI elements
    private void UpdateUI(StatUI statUI, int currentValue, float currentPosFillAmount, float currentNegFillAmount, string statType)
    {
        statUI.statText.text = $"{statType}: {currentValue}";
        statUI.posStatBar.fillAmount = currentPosFillAmount;  // Positive fill
        statUI.negStatBar.fillAmount = currentNegFillAmount;  // Negative fill
    }
}
