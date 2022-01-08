using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuildingBehaviour : MonoBehaviour
{
    #region Data

    [Space]
    [Space]
    [Header("State")]
    [Space]
    public BuildingSO buildingData;
    public bool isPickable;
    public bool hasLanded;
    public TileComponent tile;
    public bool isCrate = true;
    public int buildingTier = 1;
    public GameObject crateForm;
    public float crateLifeTime = 4f;
    public GameObject meshes;

    
    public List<GameObject> meshesList = new List<GameObject>();
    int currentExp = 0;
    




    [Header("Water")]
    [Space]
    public bool rainCollect;

    public int waterMax;
    private int _waterQty;
    public int waterQty
    {
        get { return _waterQty; }
        set
        {

            if (value < 0)
                _waterQty = 0;
            else if (value > waterMax)
                _waterQty = waterMax;
            else _waterQty = value;
        }
    }

    public GameObject waterLevelGO;
    public float yNullWaterLevel;
    public float yMaxWaterLevel;



    [Header("Time")]
    [Space]

    public bool hasTick = true;
    float tickDelay;
    static float minTickTime = 1f;
    float delayReduction = 0f;
    bool isTicking;
    public bool hasCooldown;
    public float cooldown;
    bool isCd;

    #endregion

    #region Unity Loop

    private void Awake()
    {
        crateForm.GetComponent<Renderer>().materials[2].mainTexture = buildingData.crateIcon;
        waterMax = buildingData.tierValues[0].waterMax;

        for (int i = 0; i < meshes.transform.childCount; i++)
        {
            meshesList.Add(meshes.transform.GetChild(i).gameObject);
        }

        foreach (GameObject mesh in meshesList)
        {
            mesh.SetActive(false);
        }

        if (isCrate)
        {
            crateForm.SetActive(true);
            if (waterLevelGO != null)
                waterLevelGO.SetActive(false);

            
        }
        else meshesList[0].SetActive(true);

    }

    private void Update()
    {
        StartCoroutine(DoTicking());
        StartCoroutine(DoCooldown());
        CrateLifetime();
    }

    #endregion

    #region Timing

    IEnumerator DoTicking()
    {
        if (hasTick && !isTicking)
        {
            isTicking = true;
            yield return new WaitForSeconds(tickDelay - delayReduction >= minTickTime ? tickDelay - delayReduction : minTickTime);
            OnTick();  
            isTicking = false;
        }
    }

    public IEnumerator ReduceTickDelay(float amount, float time)
    {
        delayReduction = amount;
        yield return new WaitForSeconds(time);
        delayReduction = 0f;       
    }

    IEnumerator DoCooldown()
    {
        if (hasCooldown)
        {

            if (!isCd)
            {
                isCd = true;
                yield return new WaitForSeconds(cooldown);
                OnCooldown();
                isCd = false;
            }
        }
    }

    void CrateLifetime()
    {
        if (hasLanded && tile != null && tile.tileType == TileComponent.TileType.Water)
        {
            crateLifeTime -= Time.deltaTime;
            if (crateLifeTime < 0)
            {
                tile.SetBuilding(false);

                //Play Feedback for Sinking Crate

                Destroy(this.gameObject);
                
            }
        }
    }

    #endregion

    #region Building Management

    public void AddExp()
    {
        if (buildingData.tierValues[buildingTier - 1].amountForUpgrade > 0)
        {
            if (currentExp == buildingData.tierValues[buildingTier - 1].amountForUpgrade - 1 && buildingTier <= buildingData.tierValues.Count)
                UpgradeBuilding();
            else
            {
                currentExp++;

            }
        }



    }

    void UpgradeBuilding()
    {
        
        meshesList[buildingTier - 1].SetActive(false);
        meshesList[buildingTier].SetActive(true);
        waterMax = buildingData.tierValues[buildingTier].waterMax;
        tickDelay = buildingData.tierValues[buildingTier].tickDelay;
        cooldown = buildingData.tierValues[buildingTier].cooldown;

        //Play Upgrade Feedback
        

        currentExp = 0;
        buildingTier++;
    }

    public void AddWater(int _water)
    {
        waterQty += _water;
        SetWaterLevel(waterQty);
    }

    public void DestroyBuilding()
    {
        //Play Destroy FeedBack
        Destroy(this.gameObject);
    }

    public void Build()
    {
        waterMax = buildingData.tierValues[0].waterMax;
        tickDelay = buildingData.tierValues[0].tickDelay;
        cooldown = buildingData.tierValues[0].cooldown;
        isPickable = false;
        isCrate = false;

        crateForm.SetActive(false);
        meshesList[0].SetActive(true);
        waterLevelGO.SetActive(true);
    }
    #endregion

    #region Virtual Methods

    public virtual void OnTick()
    {
        //Do OnTick Action
    }
    public virtual void OnCooldown()
    {
        //Do OnCooldown Action
    }

    public virtual void OnRainDrop(int _water)
    {
        if (rainCollect)
        {
            AddWater(_water);

        }
            
    }

    public virtual void SetWaterLevel(int _level)
    {
        if (waterLevelGO != null)
        {
            Vector3 pos = new Vector3(waterLevelGO.transform.localPosition.x, 0f, waterLevelGO.transform.localPosition.z);
            waterLevelGO.transform.localPosition = pos + Vector3.up * (yNullWaterLevel + (yMaxWaterLevel - yNullWaterLevel) * waterQty / waterMax);
        }
        
    }

    public void OnInteract(PlayerController _player)
    {
        if (buildingData.buildingName != "Bucket")
        {
            if (_player.isPickUp && _player.buildingHeld != null && _player.buildingHeld.buildingData.buildingName == "Bucket" && !_player.buildingHeld.isCrate)
            {
                int i = _player.buildingHeld.waterMax - _player.buildingHeld.waterQty;
                if (waterQty <= i)
                {
                    _player.buildingHeld.AddWater(waterQty);
                    AddWater(-waterQty);
                }
                else
                {
                    _player.buildingHeld.AddWater(i);
                    AddWater(-i);
                }

            }
            else DoInteract(_player);
        }

        _player.isInteracting = false;
    }

    public virtual void DoInteract(PlayerController _player)
    {
        //Do Interact Action
    }

    

    #endregion




}
