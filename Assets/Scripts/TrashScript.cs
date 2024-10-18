using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    bool canDestroy = false;
    bool LeverOn = false;
    bool trashIn = false;

    // List to store multiple trash GameObjects
    List<GameObject> trashList = new List<GameObject>();

    void Update()
    {
        if (canDestroy && LeverOn && trashIn)
        {
            // Loop through and destroy all trash GameObjects
            foreach (GameObject trash in trashList)
            {
                if (trash != null) // Ensure the object is not null before destroying
                {
                    Destroy(trash.gameObject);
                }
            }
            trashList.Clear(); // Clear the list after all trash is destroyed
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        trashIn = true;

        // Add the trash GameObject to the list
        if (!trashList.Contains(other.gameObject))
        {
            trashList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        trashIn = false;

        // Remove the trash GameObject from the list when it exits
        if (trashList.Contains(other.gameObject))
        {
            trashList.Remove(other.gameObject);
        }
    }

    public void TrashState(bool isClosed)
    {
        canDestroy = isClosed; // Set canDestroy based on whether it's closed
    }

    public void LeverInactive(bool isColA)
    {
        if (isColA)
        {
            LeverOn = false;
        }
    }

    public void LeverActive(bool isColB)
    {
        if (isColB)
        {
            LeverOn = true;
        }
    }
}
