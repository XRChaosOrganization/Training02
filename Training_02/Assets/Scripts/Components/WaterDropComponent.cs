using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropComponent : MonoBehaviour
{
    public int waterQty;
    private void OnCollisionEnter(Collision col)
    {

        if (col.collider.CompareTag("Building"))
        {
            BuildingBehaviour building = col.gameObject.GetComponentInParent<BuildingBehaviour>();
            building.OnRainDrop(waterQty);
        }
            
        if (col.collider.CompareTag("Player"))
            col.gameObject.GetComponent<PlayerController>().OnRainDrop(waterQty);

        Destroy(this.gameObject);

    }
}
