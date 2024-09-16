using System.Collections;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    #region VARIABLES
    [Header("References")]
    public TextMeshProUGUI moneyStatText;
    public TextMeshProUGUI viewerStatText;
    public TextMeshProUGUI awarenessStatText;
    [Space(5)]
    [Header("Statistics")]
    [SerializeField] private int moneyStat;
    [SerializeField] private int viewerStat;
    [SerializeField] private int awarenessStat;
    #endregion

    public void UpdateStats(ScriptableObject script)
    {
        // Cast the ScriptableObject to NS_Template to access its fields
        NS_Template nsTemplate = script as NS_Template;

        if (nsTemplate != null)
        {
            // Access the variables from the scriptable object
            int money = nsTemplate.money;
            int entertainment = nsTemplate.entertainment;
            int awareness = nsTemplate.awareness;

            // Increment stats based on scriptable object
            int newMoneyStat = moneyStat + money;
            int newAwarenessStat = awarenessStat + awareness;
            int newViewerStat = viewerStat + entertainment;

            // Start coroutines to gradually update the stat texts
            StartCoroutine(UpdateStatText(moneyStatText, moneyStat, newMoneyStat));
            StartCoroutine(UpdateStatText(awarenessStatText, awarenessStat, newAwarenessStat));
            StartCoroutine(UpdateStatText(viewerStatText, viewerStat, newViewerStat));

            // Update internal stats
            moneyStat = newMoneyStat;
            awarenessStat = newAwarenessStat;
            viewerStat = newViewerStat;
        }
        else
        {
            Debug.LogWarning("The provided ScriptableObject is not of type NS_Template.");
        }
    }

    // Coroutine to update the stat text gradually
    private IEnumerator UpdateStatText(TextMeshProUGUI statText, int startValue, int endValue)
    {
        float duration = Mathf.Max(0.5f, Mathf.Log10(Mathf.Abs(endValue - startValue) + 1)); // Adjust speed based on the difference
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;

            // Lerp between the startValue and endValue
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, percentageComplete));

            // Update the text with the current value
            statText.text = currentValue.ToString();

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final value is set
        statText.text = endValue.ToString();
    }
}