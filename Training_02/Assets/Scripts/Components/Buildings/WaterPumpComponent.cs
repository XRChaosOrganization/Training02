using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPumpComponent : BuildingBehaviour
{


    // T1 = 6/9s ;T2 = 6/6s ;T3 =  6/3s
    public int water = 1;

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
            if (_tile.building != null &&_tile.building.buildingData.buildingName == "WaterFilter")
            {
                waterfilters.Add(_tile.building);
            }
        }
        foreach (BuildingBehaviour _waterfilter in waterfilters)
        {
            _waterfilter.AddWater(_water / waterfilters.Count);
        }
    }

    public override void DoInteract(PlayerController _player)
    {
        hasTick = true;
    }
}
