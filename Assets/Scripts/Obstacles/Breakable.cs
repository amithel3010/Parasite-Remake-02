using UnityEngine;

public class Breakable : MonoBehaviour
{
    public BreakableType _type;

    public void Break()
    {
        Destroy(this.gameObject);
    }
}

public enum BreakableType
{
    WoodenWall,
    WoodenFloor
}
