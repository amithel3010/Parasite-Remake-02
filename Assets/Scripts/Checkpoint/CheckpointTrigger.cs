using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private Checkpoint _checkpoint;

    void Awake()
    {
        _checkpoint = GetComponentInParent<Checkpoint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _checkpoint.OnTriggered(other);
    }
}
