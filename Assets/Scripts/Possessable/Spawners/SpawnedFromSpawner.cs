using UnityEngine;

public class SpawnedFromSpawner : MonoBehaviour
{
    private Spawner _spawnedFrom;

    public void Init(Spawner spawnedFrom)
    {
        _spawnedFrom = spawnedFrom;
        if (_spawnedFrom != null && !_spawnedFrom.SpawnedObjectsList.Contains(this.gameObject))
        {
            _spawnedFrom.SpawnedObjectsList.Add(this.gameObject);
            Debug.Log($"Added {this.gameObject} to {_spawnedFrom}'s list");
        }
    }

    private void OnDisable()
    {
        if (_spawnedFrom != null && _spawnedFrom.SpawnedObjectsList.Contains(this.gameObject))
        {
            _spawnedFrom.SpawnedObjectsList.Remove(this.gameObject);
            Debug.Log($"Removed {this.gameObject} from {_spawnedFrom}'s list");
        }
    }
}
