using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsScreen : MonoBehaviour
{
    public Material[] materials;

    public void AdOn (int adNumber)
    {
        //Debug.Log(adNumber);
        GetComponent<Renderer>().material = materials[adNumber];
    }
}
