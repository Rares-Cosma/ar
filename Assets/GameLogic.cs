using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    public Health healthSystem;

    public List<GameObject> sahurPrefabs;

    public float spawnRange;
    public float spawnDelay;
    public int killCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn()
{
    while (true)
    {
        // Get AR camera position
        Vector3 camPos = Camera.main.transform.position;

        // Pick a random direction and position in XZ plane
        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 offset = new Vector3(dir.x, 0, dir.y) * spawnRange;

        // Set spawn Y position slightly below camera to keep it grounded
        Vector3 spawnPos = new Vector3(camPos.x + offset.x, camPos.y - 0.1f, camPos.z + offset.z);

        if (sahurPrefabs.Count > 0)
        {
            int index = Random.Range(0, sahurPrefabs.Count);
            Instantiate(sahurPrefabs[index], spawnPos, Quaternion.identity);
        }

        yield return new WaitForSeconds(spawnDelay);
    }
}
}
