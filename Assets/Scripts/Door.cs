using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        // Ensure the animator is found
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
        }
    }

    // This will be called when any collider enters the trigger zone
    private void OnTriggerEnter(Collider collider)
    {
        // Debug to check if the trigger is working
        Debug.Log("OnTriggerEnter detected with: " + collider.gameObject.name);

        // Only proceed if we have a valid animator reference
        if (animator != null)
        {
            // Trigger the animation to open the door
            animator.SetBool("OpenDoor", true);
            StartCoroutine(CloseDoor());
        }
    }

    private IEnumerator CloseDoor()
    {
        // Wait for 2 seconds before closing the door
        yield return new WaitForSeconds(2);

        // Trigger the animation to close the door
        animator.SetBool("OpenDoor", false);
    }
}
