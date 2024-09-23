using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneType : MonoBehaviour
{
    [SerializeField] private SceneTypeObject type;

    private void Awake()
    {
        type.Add(gameObject);
    }

    private void OnDestroy()
    {
        type.Remove(gameObject);
    }
}