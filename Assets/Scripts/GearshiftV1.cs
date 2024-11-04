using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GearshiftV1 : MonoBehaviour
{
    
    public GameObject[] Collider;

    public UnityEvent<int> colNr;
    
    [SerializeField]
    private SceneTypeObject ST_GameManager;
    private StatsManager ST_StatManager;
    private void Start()
    {
        ST_StatManager = ST_GameManager.Objects[0].GetComponent<StatsManager>();
    }

        //depending on which gear the pointer is colliding with the event invokes its number
        void OnTriggerEnter(Collider other)
    {

        for(int i = 0; i < Collider.Length; i++)
        {
            if (other == Collider[i].GetComponent<Collider>())
            {
                colNr.Invoke(i);
                ST_StatManager.ChangeAdType(i);
            }
        }
    }

    //when exiting any collider it turn to -1 
    void OnTriggerExit(Collider other)
    {

        //colNr.Invoke(-1);

    }
}
