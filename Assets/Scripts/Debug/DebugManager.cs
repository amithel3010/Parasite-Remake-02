using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance { get; private set; }

    [SerializeField] Transform _playerTransform;
    [SerializeField] Health _playerHealth;
    //[SerializeField] GameObject _PossessablePrefab;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one Instance of DebugManager");
        }
        else
        {
            Instance = this;
        }
    }

    public void SpawnPossessable(GameObject PossessablePrefab)
    {
        Instantiate(PossessablePrefab, _playerTransform.position + _playerTransform.forward * 3f, Quaternion.identity); //TODO: Change this to work with a Spawn Manager
    }

    public void ParasiteBecomesInvincible()
    {
        _playerHealth.ToggleInvincible();
    }

}
