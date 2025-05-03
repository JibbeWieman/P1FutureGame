using UnityEngine;

public class NewsCaster : MonoBehaviour
{
    [SerializeField] private Transform targetObject;

    private void Update()
    {
        this.transform.LookAt(targetObject);
        this.transform.rotation = Quaternion.Euler(0, this.transform.eulerAngles.y, 0);
    }
}
