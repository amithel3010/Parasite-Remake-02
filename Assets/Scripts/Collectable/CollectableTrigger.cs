using UnityEngine;

public class CollectableTrigger : MonoBehaviour
{
    Collectable _collectable;
    void Awake()
    {
        _collectable = GetComponentInParent<Collectable>();
    }

    void OnTriggerEnter(Collider other)
    {
        _collectable.OnTriggered(other);
    }


}
