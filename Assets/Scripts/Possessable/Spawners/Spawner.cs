using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //TODO: this will be way beter with an object pooler.

    [SerializeField] private SpawnableType _selectedType;

    [SerializeField] private List<SpawnableData> _configurations;

    [SerializeField] private int _maxAmountToSpawn = 2;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnRadius = 2f;
    [SerializeField] private float _timeBetweenSpawns = 5f;

    [SerializeField] private SpriteRenderer _timerVisualizer;
    private float _timerVisualizerMaxYSize;

    public List<GameObject> SpawnedObjectsList = new();

    private SpawnableData SelectedConfig =>
       _configurations.Find(cfg => cfg.type == _selectedType); //ChatGPT type code, i don't like it

    private GameObject _objToSpawn => SelectedConfig?.prefab;
    private Color _gizmoColor => SelectedConfig?.gizmoColor ?? Color.white;

    private float _timer;

    void Start()
    {     
        if (_objToSpawn == null)
        {
            Debug.LogWarning($"No Object to spawn was given to: {this.gameObject}, Destroying it!");
            Destroy(gameObject);
        }
        else
        {
            _timerVisualizer.color = _gizmoColor;
            _timerVisualizerMaxYSize = _timerVisualizer.size.y;
            _timerVisualizer.enabled = false;
            for (int i = 0; i < _maxAmountToSpawn; i++)
            {
                SpawnObj();
            }
        }
        
    }

    void Update()
    {
        if (SpawnedObjectsList.Count >= _maxAmountToSpawn) return;

        _timer += Time.deltaTime;
        if(_timerVisualizer.enabled == false)
        {
            _timerVisualizer.enabled = true;
        }
        _timerVisualizer.size = new Vector2(_timerVisualizer.size.x, Mathf.Lerp(0, _timerVisualizerMaxYSize, _timer / _timeBetweenSpawns));

        if (_timer > _timeBetweenSpawns)
        {
            SpawnObj();
            _timer = 0;
            _timerVisualizer.enabled = false;
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


