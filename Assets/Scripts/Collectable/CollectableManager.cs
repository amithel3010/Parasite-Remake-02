using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CollectableManager : MonoBehaviour
{
    public static CollectableManager Instance { get; private set; }

    private readonly HashSet<Collectable> _allCollectables = new(); //readonly makes sure there is only one assignment
    private readonly HashSet<Collectable> _collected = new(); //needed? i only need the number i think

    public int TotalCollecteblesInScene => _allCollectables.Count;
    public int TotalCollected => _collected.Count;

    public event Action<int, int> OnCollectionProgressChanged;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one Collectable manager in scene!");
        }
        else
        {
            Instance = this;
        }
    }

    public void InitCollectable(Collectable collectable)
    {
        if (_allCollectables.Add(collectable))
        {
            OnCollectionProgressChanged?.Invoke(TotalCollected, TotalCollecteblesInScene);
        }
    }

    public void MarkAsCollected(Collectable collectable)
    {
        //should I remove it from _allCollectibles?

        if (_collected.Add(collectable))
        {
            //update UI?
            OnCollectionProgressChanged?.Invoke(TotalCollected, TotalCollecteblesInScene);
            Debug.Log($"Collected {_collected.Count} out of {_allCollectables.Count}");
        }
    }
}
