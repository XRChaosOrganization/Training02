using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPumpComponent : BuildingBehaviour
{
    [Space]
    [Header("WaterPump")]
    [Space]
    // T1 = 6/9s ;T2 = 6/6s ;T3 =  6/3s
    public int water = 6;

    public override void OnTick()
    {
        if(!isCrate)
        GiveWaterToAlambics(water);
    }

    public override void OnCooldown()
    {
        hasTick = false;
    }

    public void GiveWaterToAlambics(int _water)
    {
        List<BuildingBehaviour> waterfilters = new List<BuildingBehaviour>();
        this.tile.DetectAdjacentTiles();
        foreach (TileComponent _tile in tile.adjTiles)
        {
            if (_tile.building.buildingData.buildingName == "WaterFilter")
            {
                waterfilters.Add(_tile.building);
            }
        }
        foreach (BuildingBehaviour _waterfilter in waterfilters)
        {
            _waterfilter.AddWater(_water / waterfilters.Count);
        }
    }
}
