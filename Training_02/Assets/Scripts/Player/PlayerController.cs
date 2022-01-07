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
                tile.GetChild(0).gameObject.SetActive(true);
            }

            if (tile != selectedTile)
            {
                selectedTile.GetChild(0).gameObject.SetActive(false);
                tile.GetChild(0).gameObject.SetActive(true);
                selectedTile = tile;
            }
        }
        else
        {
            selectedTile.GetChild(0).gameObject.SetActive(false);
            selectedTile = null;
        }
    }

    public void OnRainDrop(int _water)
    {
        //if holding bucket, call bucket.onraindrop
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
        isPickUp = true;
        if (context.canceled)
            isPickUp = false;
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
