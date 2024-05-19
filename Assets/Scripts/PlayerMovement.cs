using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody playerRb;
    RightArm rightArm; 
    [SerializeField] GameObject trainPass, phone;
    float hInput;
    float vInput;
    [SerializeField] float torque; 
    //float tempSpeed = -200.0f; 
    [SerializeField] float speed;
    [SerializeField] float pushSpeed;
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
    public bool movingLeg;
    public bool end;
    public bool canRestart;
    bool stepUp;
    bool setFinalPos; 
    bool finalPosStop;
    bool allowFinalMovement; 
    bool newPosStop;
    bool setFollow;
    [SerializeField] Transform follow; 
    //public RigBuilder rig;
    Vector3 velocity = Vector3.zero;
    static float t = 0.0f;
    static float tZ = 0.0f;
    static float tX = 0.0f;
    [SerializeField] GameObject drums; 
    Animator drummerAnim; 
    AudioSource audio;
    [SerializeField] AudioClip swipeAud, thump, turn, rotateOnce; 
    bool test;
    bool test2;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        turnstile = GameObject.Find("Turn Thing").GetComponent<RotateTurnstile>();

        //not using this anymore but I'll keep for now
        //rig = GetComponent<RigBuilder>();
        drummerAnim = GameObject.Find("drummer").GetComponent<Animator>();
        rightArm = GameObject.Find("Right Hand_target").GetComponent<RightArm>();
        audio = GetComponent<AudioSource>();
        phone.SetActive(false);
    }

    void Update()
    {
        //Debug.Log(transform.position.z);
        if(!stopAtTurnstile){
            MovePlayer();
        } else if(!newPosition){
            if(!stepUp){
                transform.position = new Vector3(transform.position.x + 0.20f, transform.position.y, transform.position.z);
                playerRb.isKinematic = true;
                stepUp = true;
            }
        }

        if(newPosition){
            if(!newPosStop){
                playerRb.isKinematic = false;
                //for now will use damp to change position - it's at least better than snapping
                playerRb.constraints = ~RigidbodyConstraints.FreezeRotationY;
                stepBack = new Vector3(transform.position.x - 0.18f, transform.position.y, transform.position.z);
                trainPass.SetActive(false);
                phone.SetActive(true);
                newPosStop = true;
            } else {
                transform.position = Vector3.Lerp(transform.position, stepBack, t);
                t += 0.25f * Time.deltaTime;
            }
            
            //newPosStop = true;
        }

        //Debug.Log(newPosition);

        if(finalPos && !allowFinalMovement){
            newPosition = false;
            if(!setFinalPos){
                //Debug.Log("STARTING MOVEMENT");
                changePos = new Vector3(1.35f, transform.position.y, 2.2f);
                changeRotation = new Vector3(transform.localEulerAngles.x, 5.8f, transform.localEulerAngles.z);
                phone.SetActive(false);
                setFinalPos = true;
            } else {
                //player clips through the ground for some reason when translating 
                //using force instead
                Vector3 direction = (changePos - transform.position).normalized;
                if(Vector3.Distance(transform.position, changePos) > 0.1f){
                    playerRb.AddForce(direction * speed * Time.deltaTime);
                } else {
                    transform.position = new Vector3(1.25f, transform.position.y, 1.9f);
                    playerRb.velocity = Vector3.zero;
                    //Debug.Log("STOPPED");
                }

                //turn player
                if(transform.localEulerAngles.y > 5.8f){
                    playerRb.AddTorque(Vector3.up * -torque * Time.deltaTime, ForceMode.Impulse);
                } else {
                    transform.position = new Vector3(1.25f, transform.position.y, 1.9f);
                    pos = transform.position;
                    finalPosStop = true;
                }
                //Debug.Log(transform.localEulerAngles.y);
            }
        }

        if(finalPosStop){
            //Debug.Log("YERR");
            if(!end){
                allowFinalMovement = true;
                playerRb.constraints = RigidbodyConstraints.FreezeRotation;
                if(movingLeg){
                    //Debug.Log("MOVING LEG");
                    MovePlayerForward();
                }
            } else {
                Win();
                drums.SetActive(true);
                //audio.Stop();
                drummerAnim.SetBool("Drumming", true);
            }
        }
        SoundEffects();
        SetAnim();
        LockMouse();
    }

    void FixedUpdate(){
        if(finalPosStop){
            if(!end && movingLeg){
                PushPlayer();
            }
        }
    }

    void MovePlayer(){
        //hInput = Input.GetAxisRaw("Horizontal"); 
        vInput = Input.GetAxisRaw("Vertical");
        playerRb.freezeRotation = true; 

        moveDirection = transform.forward * vInput;  //+ transform.right * hInput;

        if(vInput > 0){
            playerRb.AddForce(moveDirection.normalized * speed * Time.deltaTime); 

            if(playerRb.velocity.magnitude > maxSpeed){
                playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed); 
            }
        }
    }

    void MovePlayerForward(){
        //to move past the turnstile
        if(!setFollow){
            follow.position = transform.position;
            setFollow = true;
        } else {
            hInput = Input.GetAxis("Horizontal") * (1.0f + Time.deltaTime);
            vInput = Input.GetAxis("Vertical") * (1.0f + Time.deltaTime);

            float moveX = map(vInput, -1, 1, 0f, 2.0f);
            moveX = Mathf.Max(moveX, 1.1f);
            float moveH = Mathf.Lerp(follow.position.x, moveX, tX);
            tX += 0.5f * Time.deltaTime;

            float moveZ = map(-hInput, -1, 1, 0.6f, 2.6f);
            moveZ = Mathf.Max(moveZ, 1.8f);
            float moveV = Mathf.Lerp(follow.position.z, moveZ, tZ);
            tZ += 0.5f * Time.deltaTime;

            follow.position = new Vector3(moveH, follow.position.y, moveV);
            //Debug.Log(follow.transform.position);
            //Debug.Log(follow.transform.position);
        }
    }

    void PushPlayer(){
        if(Vector3.Distance(transform.position, follow.position) > 0.1f){
            Vector3 move = (follow.position - transform.position).normalized;
            playerRb.AddForce(move * pushSpeed, ForceMode.Impulse);
            //Debug.Log(follow.position + "   " + transform.position);
        } else {
            playerRb.velocity = Vector3.zero; 
        }
    }

    void Win(){
        playerRb.AddForce(Vector3.right * speed * Time.deltaTime); 

        if(playerRb.velocity.magnitude > maxSpeed){
            playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed); 
        }
    }

    void SoundEffects(){
        if(rightArm.swipe && !test){
            test = true;
            audio.PlayOneShot(swipeAud, 2f);
        } else if(!rightArm.swipe && test){
            test = false;
        }

        if(turnstile.turningSound && !test2){ 
            test2 = true; 
            audio.PlayOneShot(rotateOnce);
        } else if(!turnstile.turningSound && test2){
            test2 = false; 
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
