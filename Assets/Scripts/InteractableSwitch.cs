using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InteractableSwitch : MonoBehaviour
{
    
    public GameObject ColliderA;
    public GameObject ColliderB;

    private bool isColA = false;
    public UnityEvent<bool> colA;

    private bool isColB = false;
    public UnityEvent<bool> colB;

    //at start both bools have to be false
    void Start()
    {
        colA.Invoke(isColA);
        colB.Invoke(isColB);
    }

    //depending on which side the lever is turned the respective bool is turned true
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("yes");
        if(other == ColliderA.GetComponent<Collider>())
        {
            //Debug.Log("0");
            isColA = true;
            colA.Invoke(isColA);
        }
        else if(other == ColliderB.GetComponent<Collider>())
        {
            //Debug.Log("1");
            isColB = true;
            colB.Invoke(isColB);
        }
    }

    //when exiting any collider turn the bools false
    private void OnTriggerExit(Collider other)
    {
        isColA = false;
        isColB = false;

        colA.Invoke(isColA);
        colB.Invoke(isColB);
    }
}
