using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrankComponent : BuildingBehaviour
{
    [Space]
    [Header("Krank")]
    [Space]
    public float tier1Delayreduction;
    public float tier2Delayreduction;
    public float tier3Delayreduction;

    //Tant qu'on mash , ou pour une certaine durée selon le mash (dans ce cas utiliser Le deuxieme argument de la fct  ReducetickDelay call dans la boucle switch ci dessous (remplacer 0f) ):
    //Call la fonction si dessous:
    public void EnhanceNearestIndustrialBuildings()
    {
        List<BuildingBehaviour> waterpumps = new List<BuildingBehaviour>();
        this.tile.DetectAdjacentTiles();
        foreach (TileComponent _tile in tile.adjTiles)
        {
            if (_tile.building.buildingData.buildingName == "WaterPump")
            {
                waterpumps.Add(_tile.building);
            }
        }
        foreach (BuildingBehaviour _waterpump in waterpumps)
        {
            switch (buildingTier)
            {
                case 1:
                    _waterpump.ReduceTickDelay(tier1Delayreduction, 0f);
                    break;
                case 2:
                    _waterpump.ReduceTickDelay(tier2Delayreduction, 0f);
                    break;
                case 3:
                    _waterpump.ReduceTickDelay(tier3Delayreduction, 0f);
                    break;
            }
        }
    }
}
