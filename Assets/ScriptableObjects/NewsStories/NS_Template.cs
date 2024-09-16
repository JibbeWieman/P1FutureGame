using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewsStory", menuName = "ScriptableObjects/NewsStory", order = 1)]
public class NS_Template : ScriptableObject
{

    [SerializeField] public int money;
    [SerializeField] public int entertainment;
    [SerializeField] public int awareness;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
