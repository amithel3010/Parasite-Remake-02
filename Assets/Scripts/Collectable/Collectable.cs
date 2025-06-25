using UnityEngine;

public class Collectable : MonoBehaviour
{
    //should be able to be collected by parasite,
    //or parasite controlled possessable.
    //also, total amount in scene should be tracked somewhere
    //and collected amount


    void OnEnable()
    {
        if (CollectableManager.Instance != null)
        {
            CollectableManager.Instance.InitCollectable(this);
        }
        else
        {
            Debug.LogError("Collectable manager instance is null when trying to init collectable");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<ICollector>(out var collector))
        {
            collector.Collect(this);
            CollectableManager.Instance.MarkAsCollected(this);
            Destroy(this.transform.parent.gameObject);
        }
    }
}
