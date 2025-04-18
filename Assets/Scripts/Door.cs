using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    readonly BroadcastStartEvent evt = Events.BroadcastStartEvent;

    private void Start()
    {
        if (!TryGetComponent<Animator>(out animator))
        {
            Debug.LogError("Animator component not found on the GameObject.");
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("OnTriggerEnter detected with: " + collider.gameObject.name);

        if (animator != null && !FMVPlayer.isBroadcasting)
        {
            // Trigger the animation to open the door
            animator.SetBool("OpenDoor", true);
            StartCoroutine(CloseDoor());
        }
    }

    private IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(2);

        // Trigger the animation to close the door
        animator.SetBool("OpenDoor", false);
    }
}
