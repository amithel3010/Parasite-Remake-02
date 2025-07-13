using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [SerializeField] Parasite _playerParasite;
    [SerializeField] private Transform _defaultRespawnPoint;

    [Header("Debugging")]
    [SerializeField] private Color _inactiveColor;
    [SerializeField] private Color _activeColor;

    private Checkpoint _currentActiveCheckpoint;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 instance of Checkpoint Manager");
        }
        else
        {
            Instance = this;
        }

        if (_defaultRespawnPoint == null)
        {
            Debug.LogError("please set default respawn point");
        }
    }

    public void SetActiveCheckpoint(Checkpoint checkpoint)
    {
        if (_currentActiveCheckpoint != null)
        {
            _currentActiveCheckpoint.SetInactive(_inactiveColor);
        }
        Debug.Log("Set the current active checkpoint to" + checkpoint);
        checkpoint.SetActive(_activeColor);
        _currentActiveCheckpoint = checkpoint;
    }

    public Vector3 GetRespawnPoint()
    {
        return _currentActiveCheckpoint != null ? _currentActiveCheckpoint.GetRespawnPoint() : _defaultRespawnPoint.position;
    }

    public void RespawnParasite()
    {
        if (_playerParasite == null || (_currentActiveCheckpoint == null && _defaultRespawnPoint == null))
        {
            Debug.LogError("Can't respawn. missing parasite or spawnpos reference");
            return;
        }

        _playerParasite.TeleportTo(GetRespawnPoint());
    }

    public Parasite GetParasite()
    {
        return _playerParasite;
    }
}
