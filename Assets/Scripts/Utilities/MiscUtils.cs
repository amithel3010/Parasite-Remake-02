using UnityEngine;

public static class MiscUtils
{
    public static void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
}
