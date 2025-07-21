using UnityEngine;

[CreateAssetMenu(fileName = "SpawnableData", menuName = "Scriptable Objects/SpawnableData")]
public class SpawnableData : ScriptableObject
{
    public SpawnableTypeEnum type;
    public GameObject prefab;
    public Color gizmoColor = Color.white;
}
