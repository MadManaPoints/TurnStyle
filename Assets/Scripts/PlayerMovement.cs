using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody playerRb;
    float hInput;
    float vInput;
    float speed = 5.0f;
    float maxSpeed = 5.0f; 
    Vector3 moveDirection;
    Animator anim;
    RotateTurnstile turnstile;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        turnstile = GameObject.Find("Turn Thing").GetComponent<RotateTurnstile>();
    }

    void Update()
    {
        MovePlayer();

        if(playerRb.velocity != Vector3.zero){
            anim.SetBool("Walking", true); 
        } else {
            anim.SetBool("Walking", false);
        }
    }

    void MovePlayer(){
        hInput = Input.GetAxis("Horizontal"); 
        vInput = Input.GetAxis("Vertical");

        moveDirection = transform.forward * vInput + transform.right * hInput;

        playerRb.AddForce(moveDirection.normalized * speed); 

        if(playerRb.velocity.magnitude > maxSpeed){
            playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed); 
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
