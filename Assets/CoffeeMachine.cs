using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    [SerializeField] 
    private GameObject newPrefab;       // Assign this prefab in the Inspector

    private GameObject mugInTrigger;    // Holds the current Mug object in the trigger

    private void OnTriggerEnter(Collider other)
    {
        // Set the reference if a Mug enters and no other Mug is tracked
        if (mugInTrigger == null && other.CompareTag("Mug"))
        {
            mugInTrigger = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Clear reference if the exiting object is the tracked Mug
        if (other.gameObject == mugInTrigger)
        {
            mugInTrigger = null;
        }
    }

    // Call this function through a Unity Event
    public void FillCoffeeMug()
    {
        if (mugInTrigger != null)
        {
            // Store the position and rotation of the mug
            Vector3 position = mugInTrigger.transform.position;
            Quaternion rotation = mugInTrigger.transform.rotation;

            // Destroy the Mug object and instantiate a new one
            Destroy(mugInTrigger);
            Instantiate(newPrefab, position, rotation);

            // Clear the reference
            mugInTrigger = null;
        }
    }
}
