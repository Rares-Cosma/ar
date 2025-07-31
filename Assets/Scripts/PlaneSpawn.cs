using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class PlaneSpawner : MonoBehaviour
{
    private ARPlaneManager planeManager;

    public List<GameObject> prefabsToSpawn;
    public int maxSpawns = 10;

    private int spawnedCount = 0;

    void Awake()
    {
        planeManager = FindObjectOfType<ARPlaneManager>();
    }

    void OnEnable()
    {
        if (planeManager != null)
            planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        if (planeManager != null)
            planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            if (spawnedCount >= maxSpawns || prefabsToSpawn.Count == 0)
                return;

            int index = Random.Range(0, prefabsToSpawn.Count);
            Instantiate(prefabsToSpawn[index], plane.center, Quaternion.identity);
            spawnedCount++;
        }
    }
}
