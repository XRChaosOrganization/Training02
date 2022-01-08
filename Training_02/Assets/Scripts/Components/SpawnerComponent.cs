using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{
    public float yOffset;
    public GameObject buildingPrefab; // Supprimable
    public GameObject rainDropPrefab;
    public GameObject snowDropPrefab;
    float waveRainSpawnRate;
    float waveCrateSpawnRate;
    public float rainSpawnRate;
    public float crateSpawnRate;
    public GameObject rainDropContainer;
    public GameObject cratesContainer;
    public List<GameObject> earlySpawnerList;
    public List<GameObject> midSpawnerList;
    public List<GameObject> LateSpawnerList;
    public int spawningPhase ;

    
    
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
        List<GameObject> whereToSpawn = new List<GameObject>();
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
                whereToSpawn.Add(GameManager.gm.player1LandTiles[randomizator[i]]);
                i++;
                whereToSpawn.Add(GameManager.gm.player2LandTiles[randomizator[i]]);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                whereToSpawn.Add(GameManager.gm.player2LandTiles[randomizator[i]]);
                i++;
                whereToSpawn.Add(GameManager.gm.player1LandTiles[randomizator[i]]);
            }
        }
   
        for (int i = 0; i < 6; i++)
        {
            //if la goutte spawn au dessus d'une tile sans building OU d'un building qui n'est pas montagne
            if (whereToSpawn[i].GetComponent<TileComponent>().building == null || whereToSpawn[i].GetComponent<TileComponent>().building.buildingData.buildingName != "Mountain")
            {
                Vector3 pos = new Vector3(whereToSpawn[i].transform.position.x, whereToSpawn[i].transform.position.y + yOffset, whereToSpawn[i].transform.position.z);
                Instantiate(rainDropPrefab, pos, Quaternion.identity, rainDropContainer.transform);
            }
            else
            {
                Vector3 pos = new Vector3(whereToSpawn[i].transform.position.x, whereToSpawn[i].transform.position.y + yOffset, whereToSpawn[i].transform.position.z);
                Instantiate(snowDropPrefab, pos, Quaternion.identity, rainDropContainer.transform);
            }
            yield return new WaitForSeconds(rainSpawnRate);
        }
    }

    public void InstantiateCratesP1()
    {
        
        int randTile = Random.Range(0, 12);
        if (GameManager.gm.player1OceanTiles[randTile].GetComponent<TileComponent>().haveBuilding == false && spawningPhase == 1)
        {
            int randBuildIndex = Random.Range(0, 3);
            TileComponent tile = GameManager.gm.player1OceanTiles[randTile].GetComponent<TileComponent>();
            Vector3 pos = (tile.transform.position) + Vector3.up * yOffset;
            GameObject crateTemp = Instantiate(earlySpawnerList[randBuildIndex], pos, Quaternion.identity, cratesContainer.transform);
            tile.SetBuilding(true, crateTemp.GetComponent<BuildingBehaviour>());
        } 
        if (GameManager.gm.player1OceanTiles[randTile].GetComponent<TileComponent>().haveBuilding == false && spawningPhase == 2)
        {
            int randBuildIndex = Random.Range(0, 7);
            TileComponent tile = GameManager.gm.player1OceanTiles[randTile].GetComponent<TileComponent>();
            Vector3 pos = (tile.transform.position) + Vector3.up * yOffset;
            GameObject crateTemp = Instantiate(midSpawnerList[randBuildIndex], pos, Quaternion.identity, cratesContainer.transform);
            tile.SetBuilding(true, crateTemp.GetComponent<BuildingBehaviour>());
        } 
        if (GameManager.gm.player1OceanTiles[randTile].GetComponent<TileComponent>().haveBuilding == false && spawningPhase == 3)
        {
            int randBuildIndex = Random.Range(0, 7);
            TileComponent tile = GameManager.gm.player1OceanTiles[randTile].GetComponent<TileComponent>();
            Vector3 pos = (tile.transform.position) + Vector3.up * yOffset;
            GameObject crateTemp = Instantiate(LateSpawnerList[randBuildIndex], pos, Quaternion.identity, cratesContainer.transform);
            tile.SetBuilding(true, crateTemp.GetComponent<BuildingBehaviour>());
        }
    }

    public void InstantiateCratesP2()
    {
        int randTile = Random.Range(0, 12);
        if (GameManager.gm.player2OceanTiles[randTile].GetComponent<TileComponent>().haveBuilding == false && spawningPhase == 1)
        {
            int randBuildIndex = Random.Range(0, 3);
            TileComponent tile = GameManager.gm.player2OceanTiles[randTile].GetComponent<TileComponent>();
            Vector3 pos = (tile.transform.position) + Vector3.up * yOffset;
            GameObject crateTemp = Instantiate(earlySpawnerList[randBuildIndex], pos, Quaternion.identity, cratesContainer.transform);
            tile.SetBuilding(true, crateTemp.GetComponent<BuildingBehaviour>());
        }
        if (GameManager.gm.player2OceanTiles[randTile].GetComponent<TileComponent>().haveBuilding == false && spawningPhase == 2)
        {
            int randBuildIndex = Random.Range(0, 7);
            TileComponent tile = GameManager.gm.player2OceanTiles[randTile].GetComponent<TileComponent>();
            Vector3 pos = (tile.transform.position) + Vector3.up * yOffset;
            GameObject crateTemp = Instantiate(midSpawnerList[randBuildIndex], pos, Quaternion.identity, cratesContainer.transform);
            tile.SetBuilding(true, crateTemp.GetComponent<BuildingBehaviour>());
        }
        if (GameManager.gm.player2OceanTiles[randTile].GetComponent<TileComponent>().haveBuilding == false && spawningPhase == 3)
        {
            int randBuildIndex = Random.Range(0, 7);
            TileComponent tile = GameManager.gm.player2OceanTiles[randTile].GetComponent<TileComponent>();
            Vector3 pos = (tile.transform.position) + Vector3.up * yOffset;
            GameObject crateTemp = Instantiate(LateSpawnerList[randBuildIndex], pos, Quaternion.identity, cratesContainer.transform);
            tile.SetBuilding(true, crateTemp.GetComponent<BuildingBehaviour>());
        }
    }

   
}
