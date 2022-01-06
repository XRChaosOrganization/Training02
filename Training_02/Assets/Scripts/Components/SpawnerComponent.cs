using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{
    public float yOffset;
    public GameObject buildingPrefab;
    public GameObject rainDropPrefab;
    float waveRainSpawnRate;
    float waveCrateSpawnRate;
    public float rainSpawnRate;
    public float crateSpawnRate;
    public GameObject rainDropContainer;
    public GameObject cratesContainer;

    
    
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
            int R = Random.Range(0, 2);
            if (R == 0)
            {
                InstantiateCratesP1();
                InstantiateCratesP2();
            }
            else
            {
                InstantiateCratesP2();
                InstantiateCratesP1();
            }
            waveCrateSpawnRate = 2 * crateSpawnRate;
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
                whereToSpawn.Add(GameManager.gm.player1LandTiles[randomizator[i]].transform);
                i++;
                whereToSpawn.Add(GameManager.gm.player2LandTiles[randomizator[i]].transform);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                whereToSpawn.Add(GameManager.gm.player2LandTiles[randomizator[i]].transform);
                i++;
                whereToSpawn.Add(GameManager.gm.player1LandTiles[randomizator[i]].transform);
            }
        }
   
        for (int i = 0; i < 6; i++)
        {
            //if la goutte spawn au dessus d'une montagne
            Vector3 pos = new Vector3(whereToSpawn[i].transform.position.x, whereToSpawn[i].transform.position.y + yOffset, whereToSpawn[i].transform.position.z);
            Instantiate(rainDropPrefab, pos, Quaternion.identity,rainDropContainer.transform);
            yield return new WaitForSeconds(rainSpawnRate);
        }
    }

    public void InstantiateCratesP1()
    {
        
        int r = Random.Range(0, 12);

        if (GameManager.gm.player1OceanTiles[r].GetComponent<TileComponent>().haveBuilding == false)
        {
            TileComponent tile = GameManager.gm.player1OceanTiles[r].GetComponent<TileComponent>();
            tile.SetBuilding(true, buildingPrefab.GetComponent<BuildingBehavior>());
            Vector3 pos = (tile.transform.position) + Vector3.up * yOffset;
            Instantiate(buildingPrefab, pos, Quaternion.identity, cratesContainer.transform);
        }
    }

    public void InstantiateCratesP2()
    {

        int r = Random.Range(0, 12);

        if (GameManager.gm.player2OceanTiles[r].GetComponent<TileComponent>().haveBuilding == false)
        {
            TileComponent tile = GameManager.gm.player2OceanTiles[r].GetComponent<TileComponent>();
            tile.SetBuilding(true, buildingPrefab.GetComponent<BuildingBehavior>());
            Vector3 pos = (tile.transform.position) + Vector3.up * yOffset;
            Instantiate(buildingPrefab, pos, Quaternion.identity, cratesContainer.transform);
        }
    }

   
}
