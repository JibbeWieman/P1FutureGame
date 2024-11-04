using UnityEngine;

public class TutorialFlags : MonoBehaviour
{
    #region Event 1
    [SerializeField, Range(5f, 10f)]
    private float raycastRange = 8f;

    [SerializeField]
    private LayerMask targetLayer; // Layer mask for the target object
    #endregion

    #region Event 2
    [SerializeField]
    private Collider stage;
    #endregion

    // Events
    private TutTurnedAroundEvent evt1 = Events.TutTurnedAroundEvent;
    private TutCoffeeDeliveredEvent evt2 = Events.TutCoffeeDeliveredEvent;

    private void Awake()
    {
        EventManager.AddListener<TutStatusEvent>(SetFlags);
    }

    void Update()
    {
        // Perform raycast if the step is not yet completed
        if (!evt1.StepCompleted)
        {
            // Use Camera.main to get the position and direction for the raycast
            Vector3 rayOrigin = Camera.main.transform.position;
            Vector3 rayDirection = Camera.main.transform.forward;

            if (Physics.Raycast(rayOrigin, rayDirection, raycastRange, targetLayer, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("Hit target in specified layer");
                EventManager.Broadcast(evt1);
                evt1.StepCompleted = true;
            }
        }

        // Check for collision between stage and an object with "mugFilled" tag
        if (!evt2.StepCompleted && evt1.StepCompleted)
        {
            Collider[] colliders = Physics.OverlapBox(stage.bounds.center, stage.bounds.extents, stage.transform.rotation);
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("MugFilled"))
                {
                    // Collision with "mugFilled" object detected
                    Debug.Log("Stage collided with a mugFilled object.");
                    EventManager.Broadcast(evt2); // Uncomment if needed
                    evt2.StepCompleted = true;

                    GetNewsStoryEvent getNewsStory = Events.GetNewsStoryEvent;
                    EventManager.Broadcast(getNewsStory);
                    break;
                }
            }
        }
    }

    private void SetFlags(TutStatusEvent evt)
    {
        if (evt.TutorialFinished)
        {
            evt1.StepCompleted = true;
            evt2.StepCompleted = true;
        }
    }

    // Draw Gizmos in the Scene view to visualize the raycast range and OverlapBox
    private void OnDrawGizmos()
    {
        if (Camera.main != null)
        {
            // Visualize the raycast range
            Gizmos.color = Color.red;
            Vector3 rayOrigin = Camera.main.transform.position;
            Vector3 rayDirection = Camera.main.transform.forward;

            Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection * raycastRange);
            Gizmos.DrawSphere(rayOrigin + rayDirection * raycastRange, 0.2f);
        }

        if (stage != null)
        {
            // Visualize the overlap box area as a wireframe
            Gizmos.color = Color.green;
            Gizmos.matrix = Matrix4x4.TRS(stage.bounds.center, stage.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, stage.bounds.size);
        }
    }
}
