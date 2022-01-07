using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSensor : MonoBehaviour
{
    public PlayerController player;
    TileComponent targetTile;

    
    void Update()
    {
        Ray r = new Ray(transform.position, transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(r, 2f, 1<<8);
        List<TileComponent> tiles = new List<TileComponent>();

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                TileComponent tile = hit.collider.GetComponent<TileComponent>();
                if (tile != null && tile.tileType == TileComponent.TileType.Water && tile.haveBuilding)
                {
                    tiles.Add(tile);
                }

                if (tiles.Count > 0)
                {
                    float minDistance = (tiles[0].transform.position - transform.position).magnitude;
                    targetTile = tiles[0];
                    foreach (TileComponent _tile in tiles)
                    {
                        if ((_tile.transform.position - transform.position).magnitude < minDistance)
                        {
                            minDistance = (_tile.transform.position - transform.position).magnitude;
                            targetTile = _tile;
                        }

                    }
                }
                
            }
        }
        else targetTile = null;

        player.targetedCrateWaterTile = targetTile;
        

        
        


        

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = targetTile != null ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
}
