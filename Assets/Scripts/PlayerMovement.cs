using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody playerRb;
    float vInput;
    [SerializeField] float torque; 
    //float tempSpeed = -200.0f; 
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    Vector3 moveDirection;
    Vector3 pos;
    Vector3 stepBack;
    Vector3 changePos;
    Vector3 changeRotation;
    Animator anim;
    RotateTurnstile turnstile;
    public KeyCode esc = KeyCode.Escape;
    public bool stopAtTurnstile; 
    public bool phaseOne;
    public bool phaseTwo;
    public bool phaseThree;
    public bool newPosition; 
    public bool finalPos;
    public bool end;
    bool setFinalPos; 
    bool finalPosStop;
    bool allowFinalMovement; 
    bool newPosStop;
    public RigBuilder rig;
    Vector3 velocity = Vector3.zero;
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

        if(newPosition){
            if(!newPosStop){
                //for now will use damp to change position - it's at least better than snapping
               stepBack = new Vector3(transform.position.x - 0.38f, transform.position.y, transform.position.z);
               newPosStop = true;
            } else {
                transform.position = Vector3.SmoothDamp(transform.position, stepBack, ref velocity, 0.7f);
            }
            
            //newPosStop = true;
        }

        //Debug.Log(newPosition);

        if(finalPos && !allowFinalMovement){
            newPosition = false;
            if(!setFinalPos){
                changePos = new Vector3(1.4f, transform.position.y, 2.010f);
                changeRotation = new Vector3(transform.localEulerAngles.x, 5.8f, transform.localEulerAngles.z);
                setFinalPos = true;
            } else {
                //player clips through the ground for some reason when translating 
                //using force instead
                Vector3 direction = (changePos - transform.position).normalized;
                if(Vector3.Distance(transform.position, changePos) > 0.2f){
                    playerRb.AddForce(direction * (speed * 0.5f) * Time.deltaTime);
                } else {
                    playerRb.velocity = Vector3.zero;
                }

                //turn player
                if(transform.localEulerAngles.y > 5.8f){
                    playerRb.AddTorque(Vector3.up * -torque * Time.deltaTime, ForceMode.Impulse);
                } else {
                    pos = transform.position;
                    finalPosStop = true;
                }
                //Debug.Log(transform.localEulerAngles.y);
            }
        }

        if(finalPosStop){
            if(!end){
                allowFinalMovement = true;
                MovePlayerForward();
            } else {
                Win();
            }
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

    void Win(){
        playerRb.AddForce(Vector3.right * speed * Time.deltaTime); 

        if(playerRb.velocity.magnitude > maxSpeed){
            playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed); 
        }
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
        //doesn't work in fullscreen but it DOES work 
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
