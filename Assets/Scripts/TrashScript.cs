using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    bool canDestory = false;
    bool LeverOn = false;
    bool trashIn = false;
    GameObject trash;

    void Update()
    {
        if(canDestory && LeverOn && trashIn)
        {
            //Debug.Log("Destroyed");
            Destroy(trash.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        trashIn = true;
        //Debug.Log("trashIn =" + trashIn);
        trash = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        trashIn = false;
        //Debug.Log("trashIn = " + trashIn);
    }
    public void TrashState(bool isClosed)
    {
        if (isClosed) 
        { 
            //Debug.Log("closed");
            canDestory = true;
        }
        if (!isClosed) 
        {
            //Debug.Log("open"); 
            canDestory = false;
        }
    }

    public void LeverInactive(bool isColA)
    {
        if(isColA) 
        { 
            //Debug.Log("A"); 
            LeverOn = false;
        }
    }

    public void LeverActive(bool isColB)
    {
        if(isColB) 
        { 
            //Debug.Log("B"); 
            LeverOn = true;
        }
    }
}
