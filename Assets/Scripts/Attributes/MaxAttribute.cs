using UnityEngine;

namespace Attributes
{
    public class MaxAttribute : PropertyAttribute
    {
        public float Max;

        public MaxAttribute(float max)
        {
            this.Max = max;
        }
    }
}


