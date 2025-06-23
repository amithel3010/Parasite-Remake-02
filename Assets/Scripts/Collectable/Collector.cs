using UnityEngine;

public class Collector : MonoBehaviour
{
    //class used to mark what collider can collect collectables.
    //on collecting, keep track of how many were collected.

    //kinda of a weird way to do it but
    //right now every should have a collector script that is disabled until possessed.
    //the static int Amount Collected will increment for every collector script because it is static

    public static int AmountCollected = 0;

    public void OnCollecting(Collectable collectable)
    {
        if (!enabled)
            return;

        Destroy(collectable.transform.parent.gameObject);
        AmountCollected++;
        Debug.Log(AmountCollected);
    }
}
