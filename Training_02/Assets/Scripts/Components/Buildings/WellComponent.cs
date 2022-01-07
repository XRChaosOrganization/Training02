using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellComponent : BuildingBehaviour
{
    [Space]
    [Header("Well")]
    [Space]
    public int waterRate = 3; // T1 = 3/9s ;T2 = 3/6s ;T3 =  3/3s 

    public override void OnTick()
    {
        AddWater(waterRate);
    }
}
