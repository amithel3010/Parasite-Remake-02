using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [Header("DefaultRespawnPoint")]
    [SerializeField] private Transform _defaultRespawnPoint;

    [Header("Debugging")]
    [SerializeField] private Color _inactiveColor;
    [SerializeField] private Color _activeColor;

    Parasite _playerParasite;
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

        _playerParasite = FindAnyObjectByType<Parasite>();
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

    private Vector3 GetRespawnPoint()
    {
        return _currentActiveCheckpoint != null ? _currentActiveCheckpoint.GetRespawnPoint() : _defaultRespawnPoint.position;
    }

    public void RespawnParasite()
    {
        if (_playerParasite == null || (_currentActiveCheckpoint == null && _defaultRespawnPoint == null))
        {
            Debug.LogError("Can't respawn. missing parasite or spawn pos reference");
            return;
        }

        _playerParasite.TeleportTo(GetRespawnPoint());
    }

    public Parasite GetParasite()
    {
        return _playerParasite;
    }
}
