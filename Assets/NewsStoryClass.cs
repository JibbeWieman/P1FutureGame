using UnityEngine;
using UnityEngine.Events;

public class NewsStoryClass : MonoBehaviour
{ 
    [SerializeField] private UnityEvent _updateStats;
    private bool _used;

    public void SendStats()
    {
        if (!_used)
        {
            _updateStats.Invoke();
            _used = true;
        }
    }
}
