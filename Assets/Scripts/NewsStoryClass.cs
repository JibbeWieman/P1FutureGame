using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class NewsStoryClass : MonoBehaviour
{ 
    //[SerializeField] private UnityEvent _updateStats;
    public bool _used;

    [SerializeField]
    private SceneTypeObject stateType;

    [SerializeField]
    private NS_Template template;

    private StatsManager statsManager;
    [SerializeField] private TrendDisable trendDisable;
    private void Start()
    {
        Debug.Assert(stateType.Objects.Count > 0);
        statsManager = stateType.Objects[0].GetComponent<StatsManager>();
        Debug.Assert(statsManager != null);
    }

    public void SendStats()
    {
        if (!_used)
        {
            //_updateStats.Invoke();
            statsManager.AssignNewsStory(template);
            trendDisable.DisableOthers();
            _used = true;

        }
    }

}
