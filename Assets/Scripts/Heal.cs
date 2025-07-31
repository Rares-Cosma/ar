using UnityEngine;

public class Heal : MonoBehaviour
{
    public GameLogic manager;
    public GrassDetector detector;
    public float healthAdd = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(detector.inGrass)
            manager.healthSystem.health += healthAdd * Time.deltaTime;

    }
}
