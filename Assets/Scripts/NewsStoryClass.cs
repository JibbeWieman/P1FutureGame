using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;

public class NewsStoryClass : MonoBehaviour
{
    #region REFERENCES

    [SerializeField, Tooltip("NS_Template reference stats will be read from")]
    private NS_Template template;

    private StatsManager statsManager;

    private TrendManager trendManager;

    #endregion

    #region VARIABLES

    public bool _used = false;

    private bool _called = false;

    [SerializeField]
    private Image _usedX;

    [SerializeField]
    private SceneTypeObject trendType;

    [SerializeField]
    private SceneTypeObject gameManagerType;

    [SerializeField]
    private float trendWait = 5f;

    #endregion
    #region EVENTS
    #endregion
    #region METHODS

    private void Start()
    {
        Debug.Assert(gameManagerType.Objects.Count > 0);
        statsManager = gameManagerType.Objects[0].GetComponent<StatsManager>();
        Debug.Assert(statsManager != null);



        Debug.Assert(gameManagerType.Objects.Count > 0);
        trendManager = gameManagerType.Objects[0].GetComponent<TrendManager>();
        Debug.Assert(statsManager != null);


        _usedX = GetComponentInChildren<Image>();
        if (_usedX != null)
        {
            //Debug.Log("x disabled");
            _usedX.gameObject.SetActive(false);
            //Debug.Log($"{_usedX.gameObject}");
        }
    }

    private void Update()
    {
        if (_used)
        {
            if (!_usedX.gameObject.activeSelf)
            {
                //Debug.Log("x enabled");
                _usedX?.gameObject.SetActive(true);
            }
        }

    }

    public void SendStats()
    {
        if (!_used)
        {
            statsManager.AssignNewsStory(template);

            DisableNewsList();
            Debug.Log("Sending Timer Signal");
            StartCoroutine(TrendSpawnCycle());
        }
    }

    private void DisableNewsList()
    {
        Debug.Log($"{trendType}");
        if (trendType != null)
        {
            //Debug.Log($"{trendType.Objects.Count}");
            foreach (GameObject gameObject in trendType.Objects)
            {
                //Debug.Log("FUCK");
                NewsStoryClass newsStory = gameObject.GetComponentInChildren<NewsStoryClass>();

                Debug.Assert(newsStory != null);

                if (newsStory != null && !newsStory._used)
                {
                    // Mark this news story as used
                    newsStory._used = true;
                    //Debug.Log($"{newsStory._used}");

                    //Image usedImage = newsStory.GetComponentInChildren<Image>();
                    //if (usedImage != null)
                    //{
                    //    usedImage.gameObject.SetActive(true); // Activate the Image for this newsStory
                    //    Debug.Log($"{usedImage.gameObject} activated");
                    //}
                }
            }
        }
    }
    private void SendSignal()
    {
        
    }

    #endregion

    #region COROUTINES
    private IEnumerator TrendSpawnCycle()
    {
        Debug.Log("Starting Timer");
        yield return new WaitForSeconds(trendWait);
        trendManager.GetRandomTrend();
        Debug.Log("Sending Spawn Signal");

    }
    #endregion
}
