using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody playerRb;
    float vInput;
    //float tempSpeed = -200.0f; 
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    Vector3 moveDirection;
    Vector3 pos;
    Animator anim;
    RotateTurnstile turnstile;
    public KeyCode esc = KeyCode.Escape;
    public bool stopAtTurnstile; 
    public bool phaseOne;
    public bool phaseTwo;
    public bool phaseThree;
    public bool newPosition; 
    public bool finalPos;
    bool finalPosStop;
    bool newPosStop;
    public RigBuilder rig;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        turnstile = GameObject.Find("Turn Thing").GetComponent<RotateTurnstile>();

        //not using this anymore but I'll keep for now
        rig = GetComponent<RigBuilder>();
    }

    void Update()
    {
        //phaseThree = true;
        //Debug.Log(playerRb.velocity.magnitude);
        if(!stopAtTurnstile){
            MovePlayer();
        }

        if(newPosition && !newPosStop){
            transform.position = new Vector3(transform.position.x - 0.4f, transform.position.y, transform.position.z);
            newPosStop = true;
        }

        if(finalPos && !finalPosStop){
            transform.position = new Vector3(1.4f, transform.position.y, 2.010f);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 5.8f, transform.localEulerAngles.z);
            pos = transform.position;
            finalPosStop = true;
        }

        if(finalPosStop){
            MovePlayerForward();
        }

        SetAnim();
        LockMouse();
    }

    void MovePlayer(){
        //hInput = Input.GetAxisRaw("Horizontal"); 
        vInput = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * vInput;  //+ transform.right * hInput;

        playerRb.AddForce(moveDirection.normalized * speed * Time.deltaTime); 

        if(playerRb.velocity.magnitude > maxSpeed){
            playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed); 
        }
    }

    void MovePlayerForward(){
        //to move past the turnstile
        transform.position = pos;
        float hInput = Input.GetAxis("Horizontal") * (1.0f + Time.deltaTime);
        vInput = Input.GetAxis("Vertical") * (1.0f + Time.deltaTime);

        float moveX = map(vInput, -1, 1, 0f, 2.0f);
        moveX = Mathf.Max(moveX, 1.2f);

        float moveZ = map(-hInput, -1, 1, 2.0f, 2.2f);
        moveZ = Mathf.Max(moveZ, 2.010f);
        pos = new Vector3(moveX, pos.y, moveZ);
    }

    void SetAnim(){
        if(phaseOne){
            anim.SetBool("Phase 1", true);
            //anim.enabled = false; 
            //Debug.Log("Phase 1");
        } else if(phaseTwo){
            anim.SetBool("Phase 2", true);
        } else if(phaseThree){
            //anim.SetBool("Phase 3", true);
        } else if(playerRb.velocity != Vector3.zero && playerRb.velocity.magnitude > 0.08f){
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
        if(col.gameObject.tag == "Turnstile" && !turnstile.stuck){
            //turnstile.rotateSpeed = tempSpeed; 
            //turnstile.moveTurnstile = true; 
        }
    }

    void OnCollisionExit(Collision col){
        if(col.gameObject.tag == "Turnstile" && turnstile.stuck){
        }
    }

    float map(float value, float minA, float maxA, float minB, float maxB){
        float range = maxA - minA; 
        float valuePercent = (value - minA) / range;

        float newRange = maxB - minB;
        
        return valuePercent * newRange + minB;
    }
}
