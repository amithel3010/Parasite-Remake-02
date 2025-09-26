using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance { get; private set; }

    [SerializeField] private DebugStatsOverlay _statsOverlay;

    Transform _playerTransform;
    Health _playerHealth;

    private CinemachineOrbitalFollow _debugCam;

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

        Parasite playerObj = FindAnyObjectByType<Parasite>();
        _playerTransform = playerObj.transform;
        _playerHealth =  playerObj.GetComponent<Health>();
    }

    public void SpawnPossessable(GameObject PossessablePrefab)
    {
        Instantiate(PossessablePrefab, _playerTransform.position + _playerTransform.forward * 3f, Quaternion.identity); //TODO: Change this to work with a Spawn Manager
    }

    public void ParasiteBecomesInvincible()
    {
        _playerHealth.ToggleInvincible();
    }

    public void Restart()
    {
        GameManager.Instance.RestartFromCheckpoint();
    }

    public void ToggleDebugCam()
    {
        CameraManager.Instance.ToggleDebugCamera();
    }

    public void ToggleDebugStats()
    {
        _statsOverlay.ToggleDebugStats();
    }
}
