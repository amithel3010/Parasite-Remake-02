using UnityEngine;

public class SpawnablesCatalog : MonoBehaviour
{
    public static SpawnablesCatalog instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More Than One Spawnables Catalog in Scene!");
        }
    }

    public GameObject ParasitePrefab;
    [SerializeField] GameObject FlyPrefab;
}
