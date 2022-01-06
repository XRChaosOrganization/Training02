using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateComponent : MonoBehaviour
{
    public bool isOnTile;
    public float crateLifeTime;
    public GameObject currentOceanTile;

    private void Update()
    {
        //Si player pick la crate

        //Si player ne pick pas la crate à ajouter :
        if (isOnTile == true)
        {
            crateLifeTime -= Time.deltaTime;
            if (crateLifeTime < 0)
            {

                Destroy(this.gameObject);
                currentOceanTile.GetComponent<TileComponent>().haveBuilding = false;
            }
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("OceanTile"))
        {
            isOnTile = true;
            currentOceanTile = col.collider.gameObject;
            
        }
    }
}
