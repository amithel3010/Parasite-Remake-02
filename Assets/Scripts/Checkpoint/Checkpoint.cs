using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //class responsible for setting the current active checkpoint
    //should trigger if parasite goes through it or a possessed possessable

    [Tooltip("if set to none, it uses the transform.position of this checkpoint")]
    [SerializeField] private Transform _respawnPoint;

    private bool _isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isActive) return;
        
        Parasite parasite = CheckpointManager.Instance.GetParasite();
        if (parasite.IsControlling(other.transform.parent.gameObject))
        {
            Debug.Log("Checkpoint triggered by" + other.transform.parent.gameObject.name);
            CheckpointManager.Instance.SetActiveCheckpoint(this);
        }
    }

    public Vector3 GetRespawnPoint()
    {
        return _respawnPoint != null ? _respawnPoint.position : transform.position; 
    }
}
