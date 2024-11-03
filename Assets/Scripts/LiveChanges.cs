using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveChanges : MonoBehaviour
{
    [Header("Live Level Changes")]
    [SerializeField]
    private List<GameObject> liveChanges;

    void Start()
    {
        EventManager.AddListener<BroadcastStartEvent>(OnBroadcastStartEvent);
        EventManager.AddListener<BroadcastEndEvent>(OnBroadcastEndEvent);
    }

    /// <summary>
    /// Turn on all objects in live changes.
    /// </summary>
    private void OnBroadcastStartEvent(BroadcastStartEvent evt)
    {
        foreach (var obj in liveChanges) obj.SetActive(true);
    }

    /// <summary>
    /// Turn off all objects in live changes.
    /// </summary>
    private void OnBroadcastEndEvent(BroadcastEndEvent evt)
    {
        foreach (var obj in liveChanges) obj.SetActive(false);
    }
}
