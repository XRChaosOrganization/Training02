using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateComponent : MonoBehaviour
{
    BuildingBehavior building;

    private void Awake()
    {
        building = GetComponentInParent<BuildingBehavior>(); 
    }


    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("OceanTile"))
            building.hasLanded = true;
    }
}
