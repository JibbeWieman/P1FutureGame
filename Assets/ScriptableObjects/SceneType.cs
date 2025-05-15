using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneType : MonoBehaviour
{
    [SerializeField] private SceneTypeObject type;

    public SceneTypeObject GetSceneTypeObject()
    {
        return type;
    }

    private void Start()
    {
        if (type == null)
        {
            Debug.LogError($"{name} has no SceneTypeObject assigned!");
        }
        else
        {
            Debug.Log($"SceneTypeObject assigned: {type.name}");
            type.Add(gameObject);
        }
    }


    private void OnDestroy()
    {
        type.Remove(gameObject);
    }
}