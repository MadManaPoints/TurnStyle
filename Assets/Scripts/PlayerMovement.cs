using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody playerRb;
    float hInput;
    float vInput;
    [SerializeField] float speed = 5.0f;
    [SerializeField] float maxSpeed = 5.0f;
    Vector3 moveDirection;
    Animator anim;
    RotateTurnstile turnstile;
    public KeyCode esc = KeyCode.Escape;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        turnstile = GameObject.Find("Turn Thing").GetComponent<RotateTurnstile>();
    }

    void Update()
    {
        MovePlayer();
        SetAnim();
        LockMouse(); 
    }

    void MovePlayer(){
        hInput = Input.GetAxisRaw("Horizontal"); 
        vInput = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * vInput + transform.right * hInput;

        playerRb.AddForce(moveDirection.normalized * speed); 

        if(playerRb.velocity.magnitude > maxSpeed){
            playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed); 
        }
    }

    void SetAnim(){
        if(playerRb.velocity != Vector3.zero && playerRb.velocity.magnitude > 1.0f){
            anim.SetBool("Walking", true); 
        } else {
            anim.SetBool("Walking", false);
        }
    }

    void LockMouse(){
        if(Cursor.visible == false && Input.GetKeyDown(esc)){
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = true; 
        } else if(Cursor.visible == true && Input.GetMouseButtonDown(0)){
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = false; 
        }
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Turnstile"){
            turnstile.moveTurnstile = true; 
        }
    }

    void OnCollisionExit(Collision col){
        if(col.gameObject.tag == "Turnstile"){
            turnstile.moveTurnstile = false; 
        }
    }
}
