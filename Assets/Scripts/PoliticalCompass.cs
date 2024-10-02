using UnityEngine;

public class PoliticalCompass : MonoBehaviour
{
    // Political position represented as a Vector2
    public Vector2 politicalPosition; // (x: economic scale, y: social scale)

    // Define the quadrants
    public enum Quadrant
    {
        AuthoritarianLeft,
        AuthoritarianRight,
        LibertarianLeft,
        LibertarianRight
    }

    void Start()
    {
        // Initialize the political position
        politicalPosition = new Vector2(0f, 0f); // Center of the compass
    }

    void Update()
    {
        // Clamp the position to a specific range if necessary
        politicalPosition.x = Mathf.Clamp(politicalPosition.x, -10f, 10f);
        politicalPosition.y = Mathf.Clamp(politicalPosition.y, -10f, 10f);

        // Determine which quadrant the position falls into
        Quadrant currentQuadrant = GetCurrentQuadrant();
        Debug.Log($"Current Quadrant: {currentQuadrant}");
    }

    // Method to determine the current quadrant based on political position
    private Quadrant GetCurrentQuadrant()
    {
        if (politicalPosition.x < 0)
        {
            if (politicalPosition.y > 0)
                return Quadrant.LibertarianLeft;
            else
                return Quadrant.AuthoritarianLeft;
        }
        else
        {
            if (politicalPosition.y > 0)
                return Quadrant.LibertarianRight;
            else
                return Quadrant.AuthoritarianRight;
        }
    }
}
