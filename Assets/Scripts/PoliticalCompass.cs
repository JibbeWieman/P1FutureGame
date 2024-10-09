using UnityEngine;
using static PoliticalCompass;

public class PoliticalCompass : MonoBehaviour
{
    #region VARIABLES

    // Political position represented as a Vector2
    public Vector2 politicalPosition; // (x: economic scale, y: social scale)

    // Reference to the Material that will change its base map
    public Material targetMaterial;

    // Sprites for each quadrant and neutral position
    public Sprite authoritarianLeftSprite;
    public Sprite authoritarianRightSprite;
    public Sprite libertarianLeftSprite;
    public Sprite libertarianRightSprite;
    public Sprite neutralSprite;

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
        LibertarianRight,
        Neutral
    }

    #endregion

    #region METHODS

    private void Start()
    {
        // Initialize the political position to the center of the compass (Neutral)
        politicalPosition = new Vector2(0f, 0f);
        UpdateMaterialBaseMap(Quadrant.Neutral);
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
        // Check for neutral position (close to 0,0)
        if (Mathf.Abs(politicalPosition.x) < 0.1f && Mathf.Abs(politicalPosition.y) < 0.1f)
        {
            return Quadrant.Neutral;
        }

        if (politicalPosition.x < 0)
        {
            return politicalPosition.y > 0 ? Quadrant.LibertarianLeft : Quadrant.AuthoritarianLeft;
        }
        else
        {
            return politicalPosition.y > 0 ? Quadrant.LibertarianRight : Quadrant.AuthoritarianRight;
        }
    }

    public void UpdatePoliticalPosition(NS_Template politicalPos)
    {
        switch (politicalPos.alignment)
        {
            case PoliticalAlignment.AuthoritarianLeft:
                // Decrement the x value (left) and y value (down) to move toward Authoritarian Left
                politicalPosition.x -= 1;
                politicalPosition.y -= 1;
                break;
            case PoliticalAlignment.AuthoritarianRight:
                // Increment the x value (right) and decrement the y value (down) to move toward Authoritarian Right
                politicalPosition.x += 1;
                politicalPosition.y -= 1;
                break;
            case PoliticalAlignment.LibertarianLeft:
                // Decrement the x value (left) and increment the y value (up) to move toward Libertarian Left
                politicalPosition.x -= 1;
                politicalPosition.y += 1;
                break;
            case PoliticalAlignment.LibertarianRight:
                // Increment the x value (right) and y value (up) to move toward Libertarian Right
                politicalPosition.x += 1;
                politicalPosition.y += 1;
                break;
            default:
                politicalPosition = new Vector2(0f, 0f);    // Neutral (center position)
                break;
        }

        // Clamp the values to ensure they stay within bounds (e.g., between -10 and 10)
        ClampPoliticalPosition();

        // Update the base map based on the new political position
        Quadrant currentQuadrant = GetCurrentQuadrant();
        UpdateMaterialBaseMap(currentQuadrant);
    }

    /// <summary>
    /// Updates the material's base map (texture) based on the current quadrant.
    /// </summary>
    /// <param name="quadrant">The quadrant the political position falls into.</param>
    private void UpdateMaterialBaseMap(Quadrant quadrant)
    {
        Sprite selectedSprite = null;

        // Select the sprite based on the quadrant
        switch (quadrant)
        {
            case Quadrant.AuthoritarianLeft:
                selectedSprite = authoritarianLeftSprite;
                break;
            case Quadrant.AuthoritarianRight:
                selectedSprite = authoritarianRightSprite;
                break;
            case Quadrant.LibertarianLeft:
                selectedSprite = libertarianLeftSprite;
                break;
            case Quadrant.LibertarianRight:
                selectedSprite = libertarianRightSprite;
                break;
            case Quadrant.Neutral:
                selectedSprite = neutralSprite;
                break;
        }

        // If a sprite is selected, update the material's base map
        if (selectedSprite != null && targetMaterial != null)
        {
            targetMaterial.SetTexture("_BaseMap", selectedSprite.texture);
        }
    }

    #endregion
}
