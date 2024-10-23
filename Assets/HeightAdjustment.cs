using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightAdjustment : MonoBehaviour
{
    float direction = 0;
    [SerializeField] float speed;

    void Update()
    {
        transform.position += new Vector3 (0, direction * Time.deltaTime, 0);
        Debug.Log("dir " + direction);
    }

    public void GoUp(bool goUp)
    {
        if (goUp)
            direction = 1 * speed;
        else
            direction = 0;
    }

    public void GoDown(bool goDown)
    {
        if (goDown)
            direction = -1 * speed;
        else
            direction = 0;
    }    
}
