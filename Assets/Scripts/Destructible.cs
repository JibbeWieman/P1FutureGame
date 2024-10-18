using UnityEngine;
using UnityEngine.SceneManagement;

public class Destructible : MonoBehaviour
{
    [SerializeField] 
    private GameObject destroyedVersion;

    [SerializeField, Range(1,10)]
    private float requiredBreakMagnitude = 5;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude >= requiredBreakMagnitude)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        // Find the scene
        Scene currentScene = gameObject.scene;

        if (currentScene.isLoaded)
        {
            // Instantiate the prefab in the target scene
            GameObject instantiatedObject = Instantiate(destroyedVersion, transform.position, transform.rotation);

            // Set the instantiated object's parent to match the current object's parent
            //instantiatedObject.transform.SetParent(transform.parent);

            // Optionally destroy the current object
            Destroy(gameObject);
        }
    }
}
