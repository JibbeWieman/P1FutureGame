using UnityEngine;

public class HeightAdjustment : MonoBehaviour
{
    float direction = 0;

    [SerializeField]
    float speed;

    [SerializeField]
    private Transform cameraOffset;

    private float minHeight = 0.1f;
    private float maxHeight = 3.4f;

    void Update()
    {
        if (cameraOffset.position.y > minHeight && cameraOffset.position.y < maxHeight)
        {
            cameraOffset.position += new Vector3(0, direction * Time.deltaTime, 0);
        }
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
