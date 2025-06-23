using UnityEngine;

public class Collectable : MonoBehaviour
{
    //should be able to be collected by parasite,
    //or parasite controlled possessable.
    //also, total amount in scene should be tracked somewhere
    //and collected amount

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<Collector>(out var collector))
        {

            //collect
            collector.OnCollecting(this);
            Debug.Log($"{collector.name}" + " triggered" + this);
        }
    }
}
