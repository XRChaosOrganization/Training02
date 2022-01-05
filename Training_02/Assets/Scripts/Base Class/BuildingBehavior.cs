using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehavior : MonoBehaviour
{
    #region Data

    [Space]
    [Space]
    public BuildingSO buildingData;
    public bool isPickable;
    int buildingTier;
    Mesh mesh;
    int currentExp;




    [Header("Water")]
    [Space]
    public bool rainCollect;

    int waterMax;
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

    private void Update()
    {
        DoTicking();
        DoCooldown();
    }

    #endregion

    #region Ticking & Cooldown

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
        mesh = buildingData.tierValues[buildingTier].mesh;
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

    #endregion




}
