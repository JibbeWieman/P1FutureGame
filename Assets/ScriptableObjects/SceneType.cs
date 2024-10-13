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

    private void Awake()
    {
        type.Add(gameObject);
    }

    private void OnDestroy()
    {
        type.Remove(gameObject);
    }
}