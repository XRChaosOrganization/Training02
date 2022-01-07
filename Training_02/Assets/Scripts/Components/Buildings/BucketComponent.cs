using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketComponent : BuildingBehaviour
{
<<<<<<< Updated upstream
=======
    [Space]
    [Header("Bucket")]
    [Space]
    public GameObject waterLevel;
    public float yNullWaterLevel;
    public float yMaxWaterLevel;

    public override void SetWaterLevel(int _level)
    {
        waterQty = _level;
        Vector3 pos = new Vector3(waterLevel.transform.localPosition.x, 0f, waterLevel.transform.localPosition.z);
        waterLevel.transform.localPosition = pos + Vector3.up * (yNullWaterLevel + (yMaxWaterLevel - yNullWaterLevel) * waterQty/waterMax);
    }

>>>>>>> Stashed changes

}
