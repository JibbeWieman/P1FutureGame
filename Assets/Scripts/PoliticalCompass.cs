using UnityEngine;

public class PoliticalCompass : MonoBehaviour
{
    #region VARIABLES
    // Political position represented as a Vector2
    public Vector2 politicalPosition; // (x: economic scale, y: social scale)

    // Array of prefabs to toggle based on quadrant
    [SerializeField]
    private GameObject[] cityVariants; // 0: Neutral, 1: Cyber, 2: Plant, 3: Fire, 4: Smoky

    // Reference to the Material that will change its base map
    /*public Material targetMaterial;

     Sprites for each quadrant and neutral position
    public Sprite authoritarianLeftSprite;
    public Sprite authoritarianRightSprite;
    public Sprite libertarianLeftSprite;
    public Sprite libertarianRightSprite;
    public Sprite neutralSprite; */

    /// <summary>
    /// Defines the political quadrants.
    /// </summary>
    public enum Quadrant
    {
        AuthoritarianLeft,      // Pro Corporate Pro Ecology    Cyber
        AuthoritarianRight,     // Pro Corporate Anti Ecology   Fire
        LibertarianLeft,        // Anti Corporate Pro Ecology   Plant
        LibertarianRight,       // Anti Corporate Anti Ecology  Smoky
        Neutral
    }
    #endregion

    #region METHODS
    private void Start()
    {
        // Initialize the political position to the center of the compass (Neutral)
        politicalPosition = new Vector2(0f, 0f);
        UpdateCityscape(Quadrant.Neutral);
        //UpdateMaterialBaseMap(Quadrant.Neutral);
    }

    /// <summary>
    /// Clamps the political position to a specific range.
    /// </summary>
    private void ClampPoliticalPosition()
    {
        politicalPosition.x = Mathf.Clamp(politicalPosition.x, -5f, 5f);
        politicalPosition.y = Mathf.Clamp(politicalPosition.y, -5f, 5f);
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
        UpdateCityscape(currentQuadrant);
        //UpdateMaterialBaseMap(currentQuadrant);
    }

    /// <summary>
    /// Activates the prefab associated with the current quadrant and deactivates others.
    /// </summary>
    /// <param name="quadrant">The quadrant the political position falls into.</param>
    private void UpdateCityscape(Quadrant quadrant)
    {
        // Deactivate all prefabs initially
        foreach (var prefab in cityVariants)
        {
            prefab.SetActive(false);
        }

        // Activate the prefab based on the quadrant
        switch (quadrant)
        {
            case Quadrant.Neutral:
                cityVariants[0].SetActive(true); // Activate Neutral
                break;
            case Quadrant.AuthoritarianLeft:
                cityVariants[1].SetActive(true); // Activate Cyber
                break;
            case Quadrant.LibertarianLeft:
                cityVariants[2].SetActive(true); // Activate Plant
                break;
            case Quadrant.AuthoritarianRight:
                cityVariants[3].SetActive(true); // Activate Fire
                break;
            case Quadrant.LibertarianRight:
                cityVariants[4].SetActive(true); // Activate Smoky
                break;
        }
    }

    /// <summary>
    /// Updates the material's base map (texture) based on the current quadrant.
    /// </summary>
    /// <param name="quadrant">The quadrant the political position falls into.</param>
    /* private void UpdateMaterialBaseMap(Quadrant quadrant)
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
    } */
    #endregion
}
