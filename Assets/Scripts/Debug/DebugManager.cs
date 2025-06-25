using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance { get; private set; }

    [SerializeField] Transform _playerTransform;
    [SerializeField] InputHandler _playerInput;
    [SerializeField] Canvas DebugUI;
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
        Instantiate(PossessablePrefab, _playerTransform.position + Vector3.forward * 3f, Quaternion.identity);
    }

    private void ToggleDebugMenu(bool debugInputPressed)
    {
        if (debugInputPressed)
        {

        }
    }
}
