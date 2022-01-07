using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampComponent : BuildingBehaviour
{
    [Space]
    [Header("Swamp")]
    [Space]
    public float tier1Delayreduction;
    public float tier2Delayreduction;
    public float tier3Delayreduction;

    public void EnhanceNearestNaturalBuildings()
    {
        List<BuildingBehaviour> mountains = new List<BuildingBehaviour>();
        this.tile.DetectAdjacentTiles();
        foreach (TileComponent _tile in tile.adjTiles)
        {
            if (_tile.building.buildingData.buildingName == "Mountain")
            {
                mountains.Add(_tile.building);
            }
        }
        foreach (BuildingBehaviour _mountain in mountains)
        {
            switch (buildingTier)
            {
                case 1:
                    _mountain.ReduceTickDelay(tier1Delayreduction, 0f);
                    break;
                case 2:
                    _mountain.ReduceTickDelay(tier2Delayreduction, 0f);
                    break;
                case 3:
                    _mountain.ReduceTickDelay(tier3Delayreduction, 0f);
                    break;
            }
        }
    }
}
