using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    List<GameObject> meshesList = new List<GameObject>();
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

        for (int i = 0; i < meshes.transform.childCount -1; i++)
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
            crateForm.GetComponent<Renderer>().materials[2] = buildingData.crateIcon;
        }
        else meshesList[0].SetActive(true);
        


        crateForm.GetComponent<Renderer>().materials[2] = buildingData.crateIcon;
    }

    private void Update()
    {
        DoTicking();
        DoCooldown();
        CrateLifetime();
    }

    #endregion

    #region Timing

    IEnumerator DoTicking()
    {
        if (hasTick)
        {

            if (!isTicking)
            {
                isTicking = true;
                yield return new WaitForSeconds(tickDelay - delayReduction >= minTickTime ? tickDelay - delayReduction : minTickTime);
                OnTick();
                isTicking = false;
            }
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
        if (hasLanded && tile.tileType == TileComponent.TileType.Water)
        {
            crateLifeTime -= Time.deltaTime;
            if (crateLifeTime < 0)
            {
                tile.SetBuilding(false, null);

                //Play Feedback for Sinking Crate

                Destroy(this.gameObject);
                
            }
        }
    }

    #endregion

    #region Building Management

    public void AddExp()
    {
        if (buildingData.tierValues[buildingTier - 1].amountForUpgrade <= 0)
        {
            //Play Cannont Upgrade Further Feedback
        }
        else
        {
            if (currentExp == buildingData.tierValues[buildingTier - 1].amountForUpgrade - 1)
                UpgradeBuilding();
            else
            {
                currentExp++;
                //Play XP Up Feedback
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

    public void DestroyBuilding()
    {
        //Play Destroy FeedBack
        Destroy(this.gameObject);
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

    public virtual void OnRainDrop(int water)
    {
        if(rainCollect)
            waterQty += water;
    }

    public virtual void SetWaterLevel(int level)
    {
        //Change depending on building
    }

    #endregion




}
