using UnityEngine;

public class PoliticalCompass : MonoBehaviour
{
    #region VARIABLES

    // Political position represented as a Vector2
    public Vector2 politicalPosition; // (x: economic scale, y: social scale)

    #endregion

    #region ENUMERATIONS

    /// <summary>
    /// Defines the political quadrants.
    /// </summary>
    public enum Quadrant
    {
        AuthoritarianLeft,
        AuthoritarianRight,
        LibertarianLeft,
        LibertarianRight
    }

    #endregion

    #region METHODS

    private void Start()
    {
        // Initialize the political position to the center of the compass
        politicalPosition = new Vector2(0f, 0f);
    }

    private void Update()
    {
        ClampPoliticalPosition();

        // Determine which quadrant the position falls into
        Quadrant currentQuadrant = GetCurrentQuadrant();
        Debug.Log($"Current Quadrant: {currentQuadrant}");
    }

    /// <summary>
    /// Clamps the political position to a specific range.
    /// </summary>
    private void ClampPoliticalPosition()
    {
        politicalPosition.x = Mathf.Clamp(politicalPosition.x, -10f, 10f);
        politicalPosition.y = Mathf.Clamp(politicalPosition.y, -10f, 10f);
    }

    /// <summary>
    /// Determines the current quadrant based on the political position.
    /// </summary>
    /// <returns>The current quadrant of the political compass.</returns>
    private Quadrant GetCurrentQuadrant()
    {
        if (politicalPosition.x < 0)
        {
            return politicalPosition.y > 0 ? Quadrant.LibertarianLeft : Quadrant.AuthoritarianLeft;
        }
        else
        {
            return politicalPosition.y > 0 ? Quadrant.LibertarianRight : Quadrant.AuthoritarianRight;
        }
    }

    #endregion
}
