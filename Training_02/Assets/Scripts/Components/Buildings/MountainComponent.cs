using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainComponent : BuildingBehaviour
{
    [Space]
    [Header("Mountain")]
    [Space]
    public bool isCollecting = true;
    public int waterRate = 4; // T1 = 4/9s ;T2 = 4/6s ;T3 =  4/3s 
    public int waterStack;
    public int waterTransferRate = 8;
    public int lakeTransferRate = 8;
    

    public override void OnCooldown()
    {
        isCollecting = !isCollecting;
    }
    public override void OnTick()
    {
        if (isCollecting )
        {
            waterStack += waterRate;
        }
        else
        {
            waterStack -= waterTransferRate;
            AddWater(waterTransferRate);
            List<BuildingBehaviour> lakes = new List<BuildingBehaviour>();
            this.tile.DetectAdjacentTiles();
            foreach (TileComponent _tile in tile.adjTiles)
            {
                if (_tile.building.buildingData.buildingName == "Lake")
                {
                    lakes.Add(_tile.building);
                }
            }
            foreach (BuildingBehaviour _lake in lakes)
            {
                _lake.AddWater(lakeTransferRate / lakes.Count);
                AddWater(-lakeTransferRate);
            }
        }
    }
    
    public override void OnRainDrop(int _water)
    {
        //Methode qui change raindrop en flocon a mettre en spawner(Game Manager) ==> pour feedback la collect phase
        waterStack += _water;
    }
}
