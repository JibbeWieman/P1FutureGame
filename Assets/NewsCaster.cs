using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsCaster : MonoBehaviour
{
    [SerializeField] private Transform targetObject;

    private void Update()
    {
        //Vector3 targetPosition = new Vector3(this.transform.position.x, targetObject.position.y, this.transform.position.z);

        this.transform.LookAt(targetObject);
    }
}
