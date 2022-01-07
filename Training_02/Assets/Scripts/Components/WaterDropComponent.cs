using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropComponent : MonoBehaviour
{
    public int waterQty;
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("GroundTile"))
        {
            Destroy(this.gameObject);
        }

        if (col.collider.CompareTag("Building"))
            col.gameObject.GetComponent<BuildingBehaviour>().OnRainDrop(waterQty);
        if (col.collider.CompareTag("Player"))
            col.gameObject.GetComponent<PlayerController>().OnRainDrop(waterQty);



    }
}
