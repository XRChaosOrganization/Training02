using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Transform selectedTile;


    //Movements
    Vector3 movement = Vector3.zero;
    [SerializeField][Range(1f,5f)] float moveSpeed;
    [SerializeField] float smoothTurnTime;
    float smoothTurnSpeed;

    //Logic
    [HideInInspector] public bool haveControl;
    [HideInInspector] public bool isPickUp;
    [HideInInspector] public bool isInteracting;
    [HideInInspector] public bool isDestroy;
    private bool isCarryingObject = false;

    //HoldObject
    public Transform objectAnchor;
    public BuildingBehaviour buildingHeld;

    //Anims 
    public Animator animator; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (haveControl)
        {
            HandleMovement();
            FaceForward();
            DetectTile();
        }      
    }


    void HandleMovement()
    {
        rb.MovePosition(transform.position + (3 * movement * moveSpeed * Time.fixedDeltaTime));

        if(movement != Vector3.zero)
            animator.SetBool("IsWalking", true);
        else 
            animator.SetBool("IsWalking", false);

        //Play Sound Effect ?
    }

    void FaceForward()
    {
        if (movement.magnitude >= 0.1f)
        {
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg, ref smoothTurnSpeed, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }       
    }

    void DetectTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, 2f, 1 << 8))
        {
            Transform tile = hit.collider.transform;


            if (selectedTile == null)
            {
                selectedTile = tile;
                selectedTile.gameObject.GetComponent<TileComponent>().player = this;
                tile.GetChild(0).gameObject.SetActive(true);
            }

            if (tile != selectedTile)
            {
                selectedTile.GetChild(0).gameObject.SetActive(false);
                selectedTile.gameObject.GetComponent<TileComponent>().player = null;
                tile.GetChild(0).gameObject.SetActive(true);
                selectedTile = tile;
                selectedTile.gameObject.GetComponent<TileComponent>().player = this;
            }
        }
        else
        {
            selectedTile.GetChild(0).gameObject.SetActive(false);
            selectedTile.gameObject.GetComponent<TileComponent>().player = null;
            selectedTile = null;
        }
    }


    #region Input Link
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        movement.x = input.x;
        movement.z = input.y;
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {

        TileComponent selectedTileComponent = selectedTile.gameObject.GetComponent<TileComponent>();
        if (isPickUp && buildingHeld != null && selectedTile != null)
        {

            if (selectedTileComponent.building == null)
            {
                if (buildingHeld.buildingData.buildingName == "Bucket" && !buildingHeld.isCrate)
                {
                    buildingHeld.tile = selectedTileComponent;
                    selectedTileComponent.building = buildingHeld;
                    buildingHeld.transform.SetParent(GameManager.gm.buildingContainer);
                    buildingHeld.transform.position = selectedTile.position + Vector3.up * 0.21f;
                    buildingHeld = null;
                    isPickUp = false;
                }

                if (buildingHeld.buildingData.buildingName != "Bucket" && buildingHeld.isCrate)
                {
                    buildingHeld.Build(selectedTileComponent); //Surement A Modifier
                }


            }
            else if (selectedTileComponent.building.buildingData.buildingName == buildingHeld.buildingData.buildingName && buildingHeld.isCrate)
            {
                selectedTileComponent.building.AddExp(); //Surement A Modifier
            }






            
        }
        if (selectedTileComponent.building != null && selectedTileComponent.building.isPickable)
        {
            isPickUp = true;
            buildingHeld = selectedTileComponent.building;
            buildingHeld.transform.SetParent(objectAnchor);
            buildingHeld.transform.localPosition = Vector3.zero;
            selectedTileComponent.Clear();
        }

    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        isInteracting = true;
        if (context.canceled)
            isInteracting = false;
    }

    public void OnDestroyInput(InputAction.CallbackContext context)
    {
        isDestroy = true;
        if (context.canceled)
            isDestroy = false;
    }
    public void Pause()
    {
        UIManagerComponent.uIm.Pause();
    }

    #endregion
}
