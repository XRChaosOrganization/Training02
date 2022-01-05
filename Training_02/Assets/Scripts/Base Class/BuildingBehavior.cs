using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehavior : MonoBehaviour
{
    
    public enum BuildingType { Base, Industrial, Nature}

    [Header("Building")]
    public BuildingType buildingType;
    [Range(1, 3)] public int buildingTier = 1;

    [Header("Water")]
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
    public bool hasTick = true;
    public float tickDelay = 3f;
    static float minTickTime = 1f;
    [HideInInspector] public float delayReduction = 0f;
    bool isTicking;
    public bool hasCooldown;
    public float cooldown;
    float cdTimer;

    private void Update()
    {
        DoTicking();
    }


    #region Ticking

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

    public virtual void OnTick()
    {
        //Do OnTick Action
    }

    public IEnumerator ReduceTickDelay(float amount, float time)
    {
        delayReduction = amount;
        yield return new WaitForSeconds(time);
        delayReduction = 0f;       
    }

    #endregion






}
