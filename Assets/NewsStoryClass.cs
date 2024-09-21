using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NewsStoryClass : MonoBehaviour
{
  
    [SerializeField] private UnityEvent _updateStats;
    // Start is called before the first frame update
    public void SendStats()
    {
        _updateStats.Invoke();
    }
}
