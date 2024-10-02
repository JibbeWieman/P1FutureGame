using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    bool canDestory = false;
    bool LeverOn = false;
    void Update()
    {
        if(canDestory && LeverOn)
        {
            Debug.Log("Destroyed");
            //Destroy(other);
        }
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
