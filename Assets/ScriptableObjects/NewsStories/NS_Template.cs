using UnityEngine;

public enum PoliticalAlignment
{
    AuthoritarianLeft,
    AuthoritarianRight,
    LibertarianLeft,
    LibertarianRight
}
/* public enum Categories
{
    Climate,
    Drama,
    Entertainment,
    Migration,
} */


[CreateAssetMenu(fileName = "NewsStory", menuName = "ScriptableObjects/NewsStory", order = 1)]
public class NS_Template : ScriptableObject
{
    //Public cuz code otherwise not work, prob change later :D
    public int money;
    public int entertainment;
    public int awareness;

    public PoliticalAlignment alignment;  // Political alignment of the news story
}