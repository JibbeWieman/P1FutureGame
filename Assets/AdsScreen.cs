using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsScreen : MonoBehaviour
{
    //public Material[] materials;
    public Texture[] image;

    public void AdOn (int adNumber)
    {
        //Debug.Log(adNumber);
        //GetComponent<Renderer>().material = materials[adNumber];
        GetComponent<RawImage>().texture = image[adNumber];
        
        
    }
}
