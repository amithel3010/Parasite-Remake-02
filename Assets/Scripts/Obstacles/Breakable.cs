using UnityEngine;

public class Breakable : MonoBehaviour
{
    public void Break()
    {
        Destroy(this.gameObject);
    }
}
