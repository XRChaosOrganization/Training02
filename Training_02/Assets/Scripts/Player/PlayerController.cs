using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Transform selectedTile;
    TileComponent selectedTileComponent;


    //Movements
    Vector3 movement = Vector3.zero;
    [SerializeField][Range(1f,5f)] float moveSpeed;
    [SerializeField] float smoothTurnTime;
    float smoothTurnSpeed;

    //Logic
    public bool haveControl;
    public bool isPickUp = false;
    public bool isInteracting;
    public bool isDestroy;
    private bool isCarryingObject = false;

    //Inputs
    bool pickUpInput;
    public float destroyHoldTime;
    float holdTimer;

    //HoldObject
    public Transform objectAnchor;
    public BuildingBehaviour buildingHeld;
    public TileComponent targetedCrateWaterTile;

    //Anims 
    public Animator animator;
    Vector3 objAnchorRot = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        objAnchorRot = objectAnchor.transform.eulerAngles;
    }

    private void Update()
    {
        DetectTile();
        selectedTileComponent = selectedTile.gameObject.GetComponent<TileComponent>();
        DetectDestroy();
        DoPickUp();
        DoInteract();
        objectAnchor.transform.eulerAngles = objAnchorRot;
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
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, 3f, 1 << 8))
        {
            Transform tile = hit.collider.transform;


            if (selectedTile == null)
            {
                selectedTile = tile;
                selectedTile.gameObject.GetComponent<TileComponent>().player = this;
                tile.GetChild(0).gameObject.SetActive(true);
            }

            else if (tile != selectedTile)
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
            if (selectedTile != null)
            {
                selectedTile.GetChild(0).gameObject.SetActive(false);
                selectedTile.gameObject.GetComponent<TileComponent>().player = null;
                selectedTile = null;
            }

        }
    }

    void DetectDestroy()
    {
        if (isDestroy)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= destroyHoldTime)
            {
                DoDestroy();
                holdTimer = 0f;
            }
        }
        else holdTimer = 0f;
    }

    void DoDestroy()
    {
        

        if (isPickUp && buildingHeld != null && buildingHeld.isCrate && buildingHeld.buildingData.buildingName != "Bucket")
        {
            buildingHeld.DestroyBuilding();
            buildingHeld = null;
            isPickUp = false;
        }
        else if (!isPickUp && selectedTileComponent.building != null && !selectedTileComponent.building.isCrate && selectedTileComponent.building.buildingData.buildingName != "Bucket")
        {
            selectedTileComponent.building.DestroyBuilding();
            selectedTileComponent.building = null;
            selectedTileComponent.haveBuilding = false;

        }

        isDestroy = false;
    }

    void DoInteract()
    {
        if (isInteracting && selectedTileComponent.building != null)
        {
            selectedTileComponent.building.OnInteract(this);
        }

        
    }

    void DoPickUp()
    {
        if (pickUpInput)
        {
            isPickUp = buildingHeld != null ? true : false;
            

            //Objet dans les mains && sur une case de terre sauf haricot
            if (isPickUp && selectedTile != null)
            {
                //si case vide
                if (selectedTileComponent.building == null || selectedTileComponent.tileType == TileComponent.TileType.Bean)
                {
                    //si dans les mains j'ai un bucket qui n'est pas sous forme de caisse
                    if (buildingHeld.buildingData.buildingName == "Bucket" && !buildingHeld.isCrate)
                    {
                        //Anim
                        animator.SetTrigger("LandObject");
                        animator.SetBool("IsCarryingObject", false);

                        //poser bucket
                        buildingHeld.tile = selectedTileComponent;
                        selectedTileComponent.building = buildingHeld;

                        if(selectedTileComponent.tileType == TileComponent.TileType.Bean)
                            buildingHeld.transform.position = this.transform.position + Vector3.up * 0.21f;
                        else buildingHeld.transform.position = selectedTile.position + Vector3.up * 0.21f;
                        //
                        //
                        
                        buildingHeld.transform.SetParent(GameManager.gm.buildingContainer);
                        buildingHeld.transform.eulerAngles = Vector3.zero;
                        buildingHeld = null;
                        isPickUp = false;
                    }

                    //si dans les mains j'ai une caisse qui n'est pas un bucket
                    else if (buildingHeld.buildingData.buildingName != "Bucket" && buildingHeld.isCrate && selectedTileComponent.tileType != TileComponent.TileType.Bean)
                    {
                        animator.SetTrigger("LandObject");
                        animator.SetBool("IsCarryingObject", false);


                        buildingHeld.tile = selectedTileComponent;
                        selectedTileComponent.building = buildingHeld;
                        buildingHeld.transform.position = selectedTile.position + Vector3.up * 0.01f;
                        buildingHeld.transform.SetParent(GameManager.gm.buildingContainer);
                        buildingHeld.transform.eulerAngles = Vector3.zero;
                        
                        buildingHeld.Build();

                        buildingHeld = null;
                        isPickUp = false;
                        
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
        //Anims
        animator.SetTrigger("PickupObject");
        animator.SetBool("IsCarryingObject", true);

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

    #region Collisions


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bean"))
            other.gameObject.GetComponent<BeanComponent>().player = this;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bean"))
            other.gameObject.GetComponent<BeanComponent>().player = null;
    }

    #endregion
}
