using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateComponent : MonoBehaviour
{
    BuildingBehaviour building;

    private void Awake()
    {
        building = GetComponentInParent<BuildingBehaviour>(); 
    }


    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("OceanTile"))
            building.hasLanded = true;
    }
}
