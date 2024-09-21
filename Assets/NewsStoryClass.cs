using UnityEngine;
using UnityEngine.Events;

public class NewsStoryClass : MonoBehaviour
{ 
    [SerializeField] private UnityEvent _updateStats;
    
    public void SendStats()
    {
        _updateStats.Invoke();
    }
}
