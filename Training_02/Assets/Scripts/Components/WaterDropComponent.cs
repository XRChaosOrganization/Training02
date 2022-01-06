using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropComponent : MonoBehaviour
{
    public int waterQty;
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("GroundTile"))
        {
            Destroy(this.gameObject);
        }
        //if la goutte touche un building qui peut collect
        //if la goutte touche le joueur qui a le seau en main
        
    }
}
