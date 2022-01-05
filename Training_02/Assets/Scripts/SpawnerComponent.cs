using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{
    public float yOffset;
    public GameObject cratePrefab;
    public GameObject rainDropPrefab;
    float waveRainSpawnRate;
    float waveCrateSpawnRate;
    public float rainSpawnRate;
    public float crateSpawnRate;
    public List<Transform> player1LandTiles;
    public List<Transform> player2LandTiles;
    public List<Transform> player1OceanTiles;
    public List<Transform> player2OceanTiles;
    
    void Start()
    {
        waveRainSpawnRate = 2 * rainSpawnRate;
        waveCrateSpawnRate = 2 * crateSpawnRate;
    }

    
    void Update()
    {
        waveRainSpawnRate -= Time.deltaTime;
        if (waveRainSpawnRate < 0)
        {
            StartCoroutine(InstantiateRainDrops());
            waveRainSpawnRate = 6 * rainSpawnRate;
        }
        waveCrateSpawnRate -= Time.deltaTime;
        if (waveCrateSpawnRate < 0)
        {
            StartCoroutine(InstantiateCrates());
            waveCrateSpawnRate = 6 * crateSpawnRate;
        }
    }

    IEnumerator InstantiateRainDrops()
    {
        List<Transform> whereToSpawn = new List<Transform>();
        List<int> randomizator = new List<int>();
        while (randomizator.Count < 6)
        {
            int rand1 = Random.Range(0, 8);
            randomizator.Add(rand1);
        }
        int R = Random.Range(0, 2);
        if (R == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                whereToSpawn.Add(player1LandTiles[randomizator[i]]);
                i++;
                whereToSpawn.Add(player2LandTiles[randomizator[i]]);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                whereToSpawn.Add(player2LandTiles[randomizator[i]]);
                i++;
                whereToSpawn.Add(player1LandTiles[randomizator[i]]);
            }
        }

        for (int i = 0; i < 6; i++)
        {
            Vector3 pos = new Vector3(whereToSpawn[i].transform.position.x, whereToSpawn[i].transform.position.y + yOffset, whereToSpawn[i].transform.position.z);
            Instantiate(rainDropPrefab, pos, Quaternion.identity);
            yield return new WaitForSeconds(rainSpawnRate);
        }
    }
    IEnumerator InstantiateCrates()
    {
        List<Transform> whereToSpawn = new List<Transform>();
        List<int> randomizator = new List<int>();
        while (randomizator.Count < 6)
        {
            int rand1 = Random.Range(0, 12);
            randomizator.Add(rand1);
        }
        int R = Random.Range(0, 2);
        if (R == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                whereToSpawn.Add(player1OceanTiles[randomizator[i]]);
                i++;
                whereToSpawn.Add(player2OceanTiles[randomizator[i]]);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                whereToSpawn.Add(player2OceanTiles[randomizator[i]]);
                i++;
                whereToSpawn.Add(player1OceanTiles[randomizator[i]]);
            }
        }

        for (int i = 0; i < 6; i++)
        {
            Vector3 pos = new Vector3(whereToSpawn[i].transform.position.x, whereToSpawn[i].transform.position.y + yOffset, whereToSpawn[i].transform.position.z);
            Instantiate(cratePrefab, pos, Quaternion.identity);
            yield return new WaitForSeconds(crateSpawnRate);
        }
    }
}
