using UnityEngine;
using System.Collections;
using UnityEditor;

public class DestructibleObject : MonoBehaviour
{
    #region Fields and Properties
    [Header("Destructible Settings")]
    [SerializeField]
    private GameObject destroyedVersion;

    [SerializeField, Range(1, 10)]
    private float requiredBreakMagnitude = 7f;

    [Header("Self-Destruct Settings")]
    [SerializeField]
    private bool enableSelfDestruct = false;
    public bool EnableSelfDestruct { get => enableSelfDestruct; set => enableSelfDestruct = value; }

    // Use properties to access private fields in editor
    [SerializeField, Tooltip("Destroy the main object after all children are destroyed"), HideInInspector]
    private bool destroyRoot = false;
    public bool DestroyRoot { get => destroyRoot; set => destroyRoot = value; }

    [SerializeField, Range(1f, 20f), HideInInspector]
    private float minDestroyTime = 3f;
    public float MinDestroyTime { get => minDestroyTime; set => minDestroyTime = value; }

    [SerializeField, Range(1f, 20f), HideInInspector]
    private float maxDestroyTime = 10f;
    public float MaxDestroyTime { get => maxDestroyTime; set => maxDestroyTime = value; }

    private Rigidbody rb;
    private bool hasCollided = false;
    #endregion

    #region Unity Methods
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool _used = GetComponentInChildren<NewsStoryClass>()._used;

        if (!hasCollided && rb.velocity.magnitude >= requiredBreakMagnitude && _used)
        {
            hasCollided = true;
            DestroyObject();
        }
    }
    #endregion

    #region Destruction Logic
    private void DestroyObject()
    {
        if (!gameObject.scene.isLoaded || destroyedVersion == null) return;

        var instantiatedObject = Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);

        if (EnableSelfDestruct)
        {
            StartCoroutine(SelfDestructRoutine(instantiatedObject));
        }
    }

    private IEnumerator SelfDestructRoutine(GameObject targetObject)
    {
        Transform[] children = targetObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child != targetObject.transform)
            {
                Destroy(child.gameObject, Random.Range(minDestroyTime, maxDestroyTime));
            }
        }

        if (destroyRoot)
        {
            yield return new WaitForSeconds(maxDestroyTime);
            Destroy(targetObject);
        }
    }
    #endregion
}

#if UNITY_EDITOR
#region Custom Inspector for DestructibleObject
[CustomEditor(typeof(DestructibleObject))]
public class DestructibleObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DestructibleObject destructibleObject = (DestructibleObject)target;

        // Draw default inspector fields
        DrawDefaultInspector();

        // Only show self-destruct fields if enableSelfDestruct is true
        if (destructibleObject.EnableSelfDestruct)
        {
            destructibleObject.DestroyRoot = EditorGUILayout.Toggle("Destroy Root", destructibleObject.DestroyRoot);
            destructibleObject.MinDestroyTime = EditorGUILayout.FloatField("Min Destroy Time", destructibleObject.MinDestroyTime);
            destructibleObject.MaxDestroyTime = EditorGUILayout.FloatField("Max Destroy Time", destructibleObject.MaxDestroyTime);
        }

        // Apply changes made in the custom inspector to the target object
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endregion
#endif