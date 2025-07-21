using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //TODO: this will be way beter with an object pooler.

    [SerializeField] private SpawnableType _selectedType;

    [SerializeField] private List<SpawnableData> _configurations;

    [SerializeField] private int _maxAmountToSpawn;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _spawnTime;

    private SpawnableData SelectedConfig =>
       _configurations.Find(cfg => cfg.type == _selectedType); //ChatGPT type code

    private float _timer;


    private GameObject _objToSpawn => SelectedConfig?.prefab;
    private Color _gizmoColor => SelectedConfig?.gizmoColor ?? Color.white;

    public List<GameObject> SpawnedObjectsList = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_objToSpawn == null)
        {
            Debug.LogWarning($"No Object to spawn was given to: {this.gameObject}, Destroying it!");
            Destroy(gameObject);
        }
        else
        {
            for (int i = 0; i < _maxAmountToSpawn; i++)
            {
                SpawnObj();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnedObjectsList.Count >= _maxAmountToSpawn) return;

        _timer += Time.deltaTime;

        if (_timer > _spawnTime)
        {
            SpawnObj();
            _timer = 0;
        }
    }

    private void SpawnObj()
    {

        Vector3 randomSpawnPoint = new Vector3(_spawnPoint.position.x + randomOffset(), transform.position.y, _spawnPoint.position.z + randomOffset()); //TODO: this is more than radius
        GameObject spawnedObj = Instantiate(_objToSpawn, randomSpawnPoint, Quaternion.identity);
        Debug.Log("Spawned " + spawnedObj);
        spawnedObj.AddComponent<SpawnedFromSpawner>().Init(this);

    }

    private float randomOffset()
    {
        return UnityEngine.Random.Range(-_spawnRadius, _spawnRadius);
    }

    private void OnDrawGizmos()
    {
        DebugUtils.DrawCircle(_spawnPoint.position, _spawnRadius, _gizmoColor);
    }
}


