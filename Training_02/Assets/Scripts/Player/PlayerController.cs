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
    [HideInInspector] public bool isPickUp = false;
    [HideInInspector] public bool isInteracting;
    [HideInInspector] public bool isDestroy;
    private bool isCarryingObject = false;

    //Inputs
    bool pickUpInput;

    //HoldObject
    public Transform objectAnchor;
    public BuildingBehaviour buildingHeld;
    public TileComponent targetedCrateWaterTile;

    //Anims 
    public Animator animator; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        DetectTile();
        DoPickUp();
    }

    private void FixedUpdate()
    {
        if (haveControl)
        {
            HandleMovement();
            FaceForward();
            
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

    void DoPickUp()
    {
        if (pickUpInput)
        {
            isPickUp = buildingHeld != null ? true : false;
            TileComponent selectedTileComponent = selectedTile.gameObject.GetComponent<TileComponent>();

            //Objet dans les mains && sur une case de terre sauf haricot
            if (isPickUp && selectedTile != null)
            {

                //si case vide
                if (selectedTileComponent.building == null)
                {
                    //si dans les mains j'ai un bucket qui n'est pas sous forme de caisse
                    if (buildingHeld.buildingData.buildingName == "Bucket" && !buildingHeld.isCrate)
                    {
                        //poser bucket
                        buildingHeld.tile = selectedTileComponent;
                        selectedTileComponent.building = buildingHeld;
                        buildingHeld.transform.SetParent(GameManager.gm.buildingContainer);
                        buildingHeld.transform.position = selectedTile.position + Vector3.up * 0.21f;
                        buildingHeld = null;
                        
                    }

                    //si dans les mains j'ai une caisse qui n'est pas un bucket
                    else if (buildingHeld.buildingData.buildingName != "Bucket" && buildingHeld.isCrate)
                    {
                        buildingHeld.Build(selectedTileComponent); //Surement A Modifier
                    }


                }

                //si case pas vide && caisse dans les mains du même type que le building sur la case
                else if (selectedTileComponent.building != null && selectedTileComponent.building.buildingData.buildingName == buildingHeld.buildingData.buildingName && buildingHeld.isCrate)
                {
                    selectedTileComponent.building.AddExp();
                    Destroy(buildingHeld.gameObject);
                    isPickUp = false;
                    pickUpInput = false;

                }




            }
            //si rien dans les mains
            else if (!isPickUp)
            {
                //si caisse dispo dans une case d'eau adjacente détectée
                if (targetedCrateWaterTile != null && selectedTile != null)
                {
                    TileComponent t = selectedTile.gameObject.GetComponent<TileComponent>();
                    bool b = false;

                    foreach (TileComponent _tile in t.adjTiles )
                    {
                        if (targetedCrateWaterTile == _tile)
                            b = true;
                    }

                    if (b)
                        PickUpItem(targetedCrateWaterTile);

                }

                //sinon, si building ramassable sur la case
                else if (selectedTileComponent.building != null && selectedTileComponent.building.isPickable)
                {
                    PickUpItem(selectedTileComponent);

                    
                }
            }

            pickUpInput = false;
        }
    }

    void PickUpItem(TileComponent _tile)
    {
        buildingHeld = _tile.building;
        buildingHeld.tile = null;
        buildingHeld.hasLanded = false;
        buildingHeld.transform.SetParent(objectAnchor);
        buildingHeld.transform.localPosition = Vector3.zero;
        buildingHeld.crateForm.transform.localPosition = Vector3.zero;
        Rigidbody rb = buildingHeld.gameObject.GetComponentInChildren<Rigidbody>();
        if(rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
            

        _tile.Clear();
        isPickUp = true;
        pickUpInput = false;
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
        pickUpInput = true;
        if (context.canceled)
            pickUpInput = false;
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
