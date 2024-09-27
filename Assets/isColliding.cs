using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class isColliding : MonoBehaviour
{

    public GameObject Pointer;
    public UnityEvent<bool> isCol;

    void Start()
    {
        isCol.Invoke(false);
    }
    void OnTriggerEnter(Collider other)
    {
     if(other == Pointer.GetComponent<Collider>())
        {
            isCol.Invoke(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isCol.Invoke(false);
    }
}
