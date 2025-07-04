using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //class responsible for setting the current active checkpoint
    //should trigger if parasite goes through it or a possessed possessable

    [Tooltip("if set to none, it uses the transform.position of this checkpoint")]
    [SerializeField] private Transform _respawnPoint;

    private bool _isActive = false;

    [Header("Debugging")]
    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }

    public void OnTriggered(Collider other)
    {
        if (_isActive) return;

        Parasite parasite = CheckpointManager.Instance.GetParasite();
        if (parasite.IsControlling(other.transform.parent.gameObject)) //TODO: player control check should be in game manager i think
        {
            Debug.Log("Checkpoint triggered by" + other.transform.parent.gameObject.name);
            CheckpointManager.Instance.SetActiveCheckpoint(this);
        }
    }

    public void SetActive(Color activeColor) //called from manager
    {
        _isActive = true;
        _renderer.material.SetColor("_BaseColor", activeColor);
    }

    public void SetInactive(Color inactiveColor) // called from manager
    {
        _isActive = false;
        _renderer.material.SetColor("_BaseColor", inactiveColor);
    }

    public Vector3 GetRespawnPoint()
    {
        return _respawnPoint != null ? _respawnPoint.position : transform.position;
    }
}
