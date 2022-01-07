using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropComponent : MonoBehaviour
{
    public int waterQty;
    public GameObject shadow;
    public LayerMask hitMask;
    public float bonusScale;
    public Vector2 minMaxShadowScale;
    private float distanceFromGround;
    private RaycastHit hit;

    private void Update()
    {
        if (shadow == null) return;

        Ray ray = new Ray(this.transform.position, -this.transform.up);
        if (Physics.Raycast(ray, out hit, 200.0f, hitMask))
        {
            Vector3 point = hit.point;
            shadow.transform.position = new Vector3(point.x, point.y + 0.1f, point.z);
            distanceFromGround = Vector3.Distance(point, this.transform.position);
        }

        float currentScale = Mathf.Clamp((1 / distanceFromGround) * bonusScale, minMaxShadowScale.x, minMaxShadowScale.y);
        shadow.transform.localScale = new Vector3(currentScale, 1.0f, currentScale);

        if (distanceFromGround <= 0.5f)
            Destroy(shadow.gameObject);
    }

    private void OnCollisionEnter(Collision col)
    {
        TileComponent tile = col.gameObject.GetComponent<TileComponent>();
        if (tile != null && tile.tileType == TileComponent.TileType.Land)
        {
            tile.OnRainDrop(waterQty);
        }

        Destroy(this.gameObject);
    }
}
