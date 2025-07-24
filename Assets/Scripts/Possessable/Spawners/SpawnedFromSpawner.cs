using UnityEngine;

public class SpawnedFromSpawner : MonoBehaviour
{
    private Spawner _spawnedFrom;

    public void Init(Spawner spawnedFrom)
    {
        _spawnedFrom = spawnedFrom;
        if (_spawnedFrom != null && !_spawnedFrom._spawnedObjectsList.Contains(this.gameObject))
        {
            _spawnedFrom._spawnedObjectsList.Add(this.gameObject);
        }
    }

    private void OnDisable()
    {
        if (_spawnedFrom == null || !_spawnedFrom._spawnedObjectsList.Contains(this.gameObject)) return;
        _spawnedFrom._spawnedObjectsList.Remove(this.gameObject);
    }
}
