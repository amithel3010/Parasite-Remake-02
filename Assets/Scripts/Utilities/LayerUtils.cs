using UnityEngine;

public static class LayerUtils
{
    public static readonly int PlayerControlledLayer = LayerMask.NameToLayer("Player Controlled");
    public static readonly int PlayerControlledLayerMask = 1 << PlayerControlledLayer;

    public static readonly int PossessableLayer = LayerMask.NameToLayer("Possessable");
    public static readonly int PossessableLayerMask = 1 << PossessableLayer;
}
