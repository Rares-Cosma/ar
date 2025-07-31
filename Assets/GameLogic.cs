using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    public Health healthSystem;

    public GameObject sahur;  

    public float spawnRange;
    public float spawnDelay;
    public int killCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn()
    {
        while(true)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;
            Vector3 spawnPos = new Vector3(dir.x,0,dir.y).normalized;
            Instantiate(sahur,spawnPos,Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
