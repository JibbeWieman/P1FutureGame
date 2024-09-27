using UnityEngine;

[CreateAssetMenu(fileName = "NewsStory", menuName = "ScriptableObjects/NewsStory", order = 1)]
public class NS_Template : ScriptableObject
{
    //Public cuz code otherwise not work, prob change later :D
    public int money;
    public int entertainment;
    public int awareness;

    public bool isTrending;
}
