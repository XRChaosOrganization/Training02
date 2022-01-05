using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{
    float waveRainSpawnRate;
    public float rainSpawnRate;
    public List<Transform> player1LandTiles;
    public List<Transform> player2LandTiles;
    public List<Transform> player1OceanTiles;
    public List<Transform> player2OceanTiles;
    
    void Start()
    {
        waveRainSpawnRate = 6 * rainSpawnRate;
    }

    
    void Update()
    {
        waveRainSpawnRate -= Time.deltaTime;
        if (waveRainSpawnRate < 0)
        {

        }
        SpawnRainDrops();
    }

    public void SpawnRainDrops()
    {
       
    }
}
