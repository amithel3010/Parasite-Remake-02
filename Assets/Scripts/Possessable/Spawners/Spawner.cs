using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    //TODO: this will be way better with an object pooler.

    [SerializeField] private SpawnableTypeEnum _selectedType;

    [SerializeField] private List<SpawnableData> _configurations;

    [SerializeField] private int _maxAmountToSpawn = 2;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnRadius = 2f;
    [SerializeField] private float _timeBetweenSpawns = 5f;

    [SerializeField] private SpriteRenderer _timerVisualizer;
    private float _timerVisualizerMaxYSize;

    [FormerlySerializedAs("SpawnedObjectsList")] public List<GameObject> _spawnedObjectsList = new();

    private SpawnableData SelectedConfig =>
       _configurations.Find(cfg => cfg.type == _selectedType); //ChatGPT type code, I don't like it

    private GameObject ObjToSpawn => SelectedConfig?.prefab;
    private Color GizmoColor => SelectedConfig?.gizmoColor ?? Color.white;

    private float _timer;

    void Start()
    {     
        if (ObjToSpawn == null)
        {
            Debug.LogWarning($"No Object to spawn was given to: {this.gameObject}, Destroying it!");
            Destroy(gameObject);
        }
        else
        {
            _timerVisualizer.color = GizmoColor;
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
        if (_spawnedObjectsList.Count >= _maxAmountToSpawn) return;

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

        Vector3 randomSpawnPoint = new Vector3(_spawnPoint.position.x + RandomOffset(), transform.position.y, _spawnPoint.position.z + RandomOffset()); //TODO: this is more than radius
        GameObject spawnedObj = Instantiate(ObjToSpawn, randomSpawnPoint, Quaternion.identity);
        spawnedObj.AddComponent<SpawnedFromSpawner>().Init(this);

    }

    private float RandomOffset()
    {
        return Random.Range(-_spawnRadius, _spawnRadius);
    }

    private void OnDrawGizmos()
    {
        DebugUtils.DrawCircle(_spawnPoint.position, _spawnRadius, GizmoColor);
    }
}


