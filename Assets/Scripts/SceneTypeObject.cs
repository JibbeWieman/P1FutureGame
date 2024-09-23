using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scene Type", menuName = "ScriptableObjects/SceneType", order = 1)]
public class SceneTypeObject : ScriptableObject
{
    private List<GameObject> objects;
    
    public event Action onAdded;
    public event Action onRemoved;
    
    public void Add(GameObject obj)
    {
        objects.Add(obj);
        onAdded?.Invoke();
    }

    public void Remove(GameObject obj)
    {
        objects.Remove(obj);
        onRemoved?.Invoke();
    }

    public ReadOnlyCollection<GameObject> Objects => objects.AsReadOnly();
}
