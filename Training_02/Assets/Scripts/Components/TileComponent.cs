using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public enum TileType { Land, Water}
    public TileType tileType;
    public bool isUpgraded;
    public bool haveBuilding;
    public BuildingBehavior building;


    //Liste de directions qui pointent vers l'eau ? pour l'orientation des pompes ?
    



    public List<TileComponent> adjTiles;


    private void Start()
    {
        DetectAdjacentTiles();

    }



    public void DetectAdjacentTiles()
    {
        adjTiles.Clear();
        Collider[] detectedTiles = Physics.OverlapSphere(transform.position, 1.2f, 1<<8);
        foreach (Collider col in detectedTiles)
        {
            TileComponent tile = col.GetComponent<TileComponent>();
            if (tile != null && col.gameObject != this.gameObject)
                adjTiles.Add(tile);
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 1.2f);
    }
}