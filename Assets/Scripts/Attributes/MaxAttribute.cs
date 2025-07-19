using UnityEngine;

public class MaxAttribute : PropertyAttribute
{
    public float max;

    public MaxAttribute(float max)
    {
        this.max = max;
    }
}


