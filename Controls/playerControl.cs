using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class playerControl : MonoBehaviour
{
    public Rigidbody2D rBody;
    public Rigidbody2D cam;
    public SpriteRenderer sprite;
    public Animator anim;
    public float speed = 1F;
    public const float speedC = 1F;
    private Vector2 movement;
    float x;
    int y;
    public PlayerControls playerController;
    private bool camMoved;
    [SerializeField] bool notAnimated;
  


    public static playerControl instance;

    public event Action OnConfirmKey;
    public event Action OnInvKey;
    public event Action OnExitKey;


 
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("multiple playercontrol scripts");
        }
        instance = this;
    }
    


    void Start()
    {
        movement = Vector3.zero;
        playerController = new PlayerControls();
        setUpControls();

        playerController.Player.Confirm.performed += InvokeConfirm;
        playerController.Player.Cancel.performed += InvokeExit;
        playerController.Player.Inventory.performed += InvokeInv;



    }
    public IEnumerator MoveCamera(Vector2 point, bool moved)
    {
        if (moved)
        {
            camMoved = moved;
        }
        
        if(cam.position.y > point.y)
        {
            cam.velocity = new Vector2(0, -1);
            while (cam.position.y > point.y)
            {
                yield return new WaitForSeconds(0.1F);
            }
            cam.velocity = new Vector2(0, 0);
        }
        else if(cam.position.y < point.y)
        {
            cam.velocity = new Vector2(0, 1);
            while (cam.position.y < point.y)
            {
                yield return new WaitForSeconds(0.1F);
            }
            cam.velocity = new Vector2(0, 0);
        }
        camMoved = moved;
    }

    
    public void setUpControls()
    {
        playerController.Player.Enable();

    }
    public void DisableControls()
    {

        playerController.Player.Disable();
    }

    void InvokeConfirm(InputAction.CallbackContext context)
    {
        try 
        {
            OnConfirmKey.Invoke();
        }
        catch
        {
            Debug.Log("not in range of any items");
        }
        
    }
    void InvokeInv(InputAction.CallbackContext context)
    {
        
        try
        {
            OnInvKey.Invoke();
        }
        catch
        {
            Debug.Log("not in range of any items");
        }
    }
    void InvokeExit(InputAction.CallbackContext context)
    {
        try
        {
            OnExitKey.Invoke();
        }
        catch
        {
            Debug.Log("not in range of any items");
        }
    }
    private void Update()
    {
        movement = playerController.Player.Move.ReadValue<Vector2>();
        
    }
    private void FixedUpdate()
    {

        rBody.velocity = movement * speed;
        

        x = movement.x;
        if(x < 0)
        {
            sprite.flipX = false;
       
        }
        if (x > 0)
        {
            sprite.flipX = true;
         
        }


        if (!notAnimated)
        {
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
            if (speed == 0)
            {
                anim.SetFloat("Speed", 0);
            }
            else
            {
                anim.SetFloat("Speed", movement.sqrMagnitude);
            }


            if (movement.y == 1)
            {
                anim.SetFloat("Direction", 4);
            }
            else if (movement.y == -1)
            {
                anim.SetFloat("Direction", 1);
            }
            else if (movement.x == 1)
            {
                anim.SetFloat("Direction", 3);
            }
            else if (movement.x == -1)
            {
                anim.SetFloat("Direction", 2);
            }
        }
        
        


    }
}

    