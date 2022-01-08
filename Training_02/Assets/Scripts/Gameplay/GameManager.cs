using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int scoreP1;
    public int scoreP2;
    public List<GameObject> player1LandTiles;
    public List<GameObject> player2LandTiles;
    public List<GameObject> player1OceanTiles;
    public List<GameObject> player2OceanTiles;

    public Transform islandP1;
    public Transform islandP2;

    public float timer;

    [Space]
    public Transform buildingContainer;
    public GameObject spawner;
    private int spawnPhase;

    PlayerInputManager playerSpawn;

    private void Awake()
    {
        gm = this;
        playerSpawn = GetComponent<PlayerInputManager>();
        
    }

    private void Start()
    {
        playerSpawn.playerPrefab.gameObject.transform.position = islandP1.position + 2 * Vector3.back + 1.55f * Vector3.up;
        playerSpawn.JoinPlayer(0);
        playerSpawn.playerPrefab.gameObject.transform.position = islandP2.position + 2 * Vector3.back + 1.55f * Vector3.up;

        if (InputSystem.devices.Count >= 2)
        {

            playerSpawn.JoinPlayer(1);
        }
        else
        {
            Debug.LogError("Cannot find P2 Controller");

            // Rajouter une Logique pour detecter connexion mannette 2 et spawn le P2

            // Quand les deux players sont spawns, press A to start pour deux players to start game
        }


        //When both players ready, start coroutine for countdown to start with following logic inside
        playerSpawn.playerPrefab.gameObject.transform.position = Vector3.zero;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<PlayerController>().haveControl = true;
        }
    }
    public void Update()
    {
        timer -= Time.deltaTime;
        switch (timer)
        {
            case float n when (timer <= 300 && timer > 210 ):
                spawner.GetComponent<SpawnerComponent>().spawningPhase = 1;
                break;
            case float n when (timer <= 210 && timer > 120):
                spawner.GetComponent<SpawnerComponent>().spawningPhase = 2;
                break;
            case float n when (timer <= 120 && timer > 0):
                spawner.GetComponent<SpawnerComponent>().spawningPhase = 3;
                break;
            case float n when timer <= 0:
                //ENDGAME + Victory Screen !!
                break;
        }

    }



}
