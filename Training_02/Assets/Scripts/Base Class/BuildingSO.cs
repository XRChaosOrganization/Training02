using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BuildingTier
{
    
    public int waterMax;
    public float tickDelay;
    public float cooldown;
    public int amountForUpgrade;  //put 0 to signify no next level and block upgrade process
}

[CreateAssetMenu(fileName = "_Data", menuName = "ScriptableObjects/BuildingData", order = 1)]
public class BuildingSO : ScriptableObject
{
    public Texture crateIcon;
    public enum BuildingType { Base, Industrial, Nature }

    [Space]
    [Space]
    [Space]

    public string buildingName;
    public BuildingType buildingType;

    [Space]
    [Header("Tiers")]
    [Space]

    [SerializeField]public List<BuildingTier> tierValues;


}
