using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropComponent : MonoBehaviour
{
    public int waterQty;
    private void OnCollisionEnter(Collision col)
    {
        TileComponent tile = col.gameObject.GetComponent<TileComponent>();
        if (tile != null && tile.tileType == TileComponent.TileType.Land)
        {
            tile.OnRainDrop(waterQty);
        }
            

        Destroy(this.gameObject);
            


    }
}
