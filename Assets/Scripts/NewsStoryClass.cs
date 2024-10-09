using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewsStoryClass : MonoBehaviour
{
    #region REFERENCES

    [SerializeField, Tooltip("NS_Template reference stats will be read from")]
    private NS_Template template;

    private StatsManager statsManager;

    #endregion

    #region VARIABLES

    public bool _used = false;

    [SerializeField]
    private Image _usedX;

    [SerializeField] private TextMeshProUGUI display;

    [SerializeField]
    private SceneTypeObject stateType;

    [SerializeField]
    private SceneTypeObject trendType;

    #endregion

    #region METHODS

    private void Start()
    {
        Debug.Assert(stateType.Objects.Count > 0);
        statsManager = stateType.Objects[0].GetComponent<StatsManager>();
        Debug.Assert(statsManager != null);
        display.text = template.name;

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
        }
    }

    private void DisableNewsList()
    {
        //Debug.Log($"{trendType}");
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

    #endregion
}
