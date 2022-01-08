using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanComponent : MonoBehaviour
{
    public int playerId;
    private int _waterQty;
    
    public int waterQty
    {
        get { return _waterQty; }
        set
        {
            if (value >= requiredWater)
            {
                AddSection();
                _waterQty = value - requiredWater;
            } else _waterQty = value;
        }
    }

    public static int requiredWater = 6;
    public GameObject beanSectionPrefab;
    public PlayerController player;


    private void Update()
    {
        if (player != null)
        {
            if (player.isPickUp && player.buildingHeld != null && player.buildingHeld.buildingData.buildingName == "Bucket" && player.isInteracting)
            {
                PlayerInteract();
            }
        }
    }


    void PlayerInteract()
    {
        waterQty += player.buildingHeld.waterQty;
        player.buildingHeld.AddWater(-player.buildingHeld.waterQty);
    }

    void AddSection()
    {
        Vector3 spawnAnchor = transform.GetChild(transform.childCount - 1).Find("Anchor_NextSection").transform.position;
        GameObject beanTemp = Instantiate(beanSectionPrefab, spawnAnchor, transform.rotation, transform);
        switch (playerId)
        {
            case 1:
                GameManager.gm.scoreP1++;
                break;
            case 2:
                GameManager.gm.scoreP2++;
                break;
            default:
                Debug.LogError("Bean has Invalid Player ID");
                break;
        }

        player.isInteracting = false;

    }




}
