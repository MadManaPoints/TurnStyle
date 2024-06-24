using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using TMPro;
using UnityEngine;

public class RotateTurnstile : MonoBehaviour
{
    RightArm rightArm;
    Score score;
    bool touching;
    //public float x, y, z; //this was to test rotation - use z to move new turnstile 
    public bool moveTurnstile;
    public bool stuck;
    public bool waitForRotation;
    bool grabSwitch;
    public float rotateSpeed = 3.0f;
    static float t = 0.0f;
    float rotateZ = 180.0f;
    float backward = 120.0f;
    float forward = 240.0f;
    float noReturn = 125.0f;
    float targetTime = 1.0f;
    bool noReturnSwitch;
    bool motorOn;
    bool turning;
    [SerializeField] bool playerControl;
    Vector3 velocity = Vector3.zero;
    Vector3 startPos;
    Quaternion initialPos;
    [SerializeField] float stopTurnstile;
    [SerializeField] Transform follow; 
    Rigidbody rb;
    HingeJoint joint;
    JointLimits limits;
    [SerializeField] TextMeshProUGUI adText, thumbStick;
    [SerializeField] GameObject ad;
    [SerializeField] Transform initialTransform;
    public bool resetRot = false;
    [SerializeField] GameObject bar;
    [SerializeField] Material barMat;
    [SerializeField] GameObject stepOne, stepTwo; 
    bool startBarEmission;
    public bool turningSound; 
    public bool turnSoundSwitch; 
    public bool thumpSound;
    void Start()
    {
        rightArm = GameObject.Find("Right Hand_target").GetComponent<RightArm>();
        score = GameObject.Find("Score").GetComponent<Score>();
        //startPos = this.transform;
        //startPos.localEulerAngles = transform.localEulerAngles;
        //initialPos = this.transform.rotation;
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<HingeJoint>();
        
        startPos = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 180.0f);
        transform.localEulerAngles = startPos;
        follow.localEulerAngles = startPos;

        limits = joint.limits;

        initialTransform.position = transform.position;
        initialTransform.rotation = transform.rotation;

        //hide the directions in the beginning
        stepOne.SetActive(false);
        stepTwo.SetActive(false);
    }

    void Update()
    {
        if(moveTurnstile){
            playerControl = true;
            bar.GetComponent<Renderer>().material = barMat;
            stepOne.SetActive(true);
        } else {
            playerControl = false;
        }

        SpinTurnstile();

        if(touching){
            score.SubtractScore();
        }

        MotorBehavior();
  
        if(rightArm.finalPhase){
            stepOne.SetActive(true);
        }
    }

    void FixedUpdate(){
        SpinTurnstileHinge();
    }

    void MotorBehavior(){
        if(noReturnSwitch && !moveTurnstile){
            turning = true;
            playerControl = false; 
            noReturnSwitch = false;
        }
    }

    void SpinTurnstileHinge(){
        //OK THIS IS THE ONE!!
        if(turning){
            joint.useMotor = true;
            rightArm.letGo = true;
            turningSound = true;
            targetTime -= Time.deltaTime;
            if(targetTime <= 0){
                targetTime = 1.0f;
                joint.useMotor = false;
                turningSound = false; 
                transform.position = initialTransform.position;
                transform.rotation = initialTransform.rotation;
                playerControl = true;
                follow.localEulerAngles = new Vector3(follow.localEulerAngles.x, follow.localEulerAngles.y, 180.0f);
                turning = false; 
            }
        } else if(Vector3.Distance(transform.localEulerAngles, follow.position) > 0.2f){
                Vector3 turn = follow.localEulerAngles - transform.localEulerAngles;
                rb.angularVelocity = turn * 2.0f;
                //Debug.Log(rb.angularVelocity.magnitude);
        }
    }

    void SpinTurnstile(){
        float moveY = Input.GetAxis("Mouse Y") * (1.0f + Time.deltaTime);
        if(!playerControl){
            moveY = 0f;
        }
            rotateZ = map(moveY, -1, 1, backward, forward);

        //Debug.Log(rotateZ);
        if(moveY < -0.3){
            stepTwo.SetActive(true);
        }

        ThumpAudio(moveY);

        if(noReturnSwitch){
            rotateZ = Mathf.Min(rotateZ, noReturn);
        } else {
            rotateZ = Mathf.Min(rotateZ, stopTurnstile);
        }

        float rotateTurnstile = Mathf.Lerp(follow.localEulerAngles.z, rotateZ, t);
        t += 0.5f * Time.deltaTime;
        if(rotateTurnstile < noReturn && !noReturnSwitch){
            noReturnSwitch = true; 
        }
        //Debug.Log(backward + "  " + rotateTurnstile + "  " + forward + "  " + noReturn + "  " + stopTurnstile + " NO RETURN:  " + noReturnSwitch + " PLAYER INPUT: " + playerControl);
        follow.localEulerAngles = new Vector3(follow.localEulerAngles.x, follow.localEulerAngles.y, -rotateTurnstile);
        //Debug.Log(moveY);
    }
    
    void ThumpAudio(float tempMoveY){
        if(!noReturnSwitch && rotateZ > stopTurnstile){
            if(tempMoveY > 0.1f){
                thumpSound = true;
            }
        } else if(rotateZ < stopTurnstile){
            thumpSound = false;
        }

        if(noReturnSwitch && rotateZ > noReturn){
            if(tempMoveY > 0.1f){
                thumpSound = true;
            }
        } else if(rotateZ < noReturn){
            thumpSound = false;
        }
    }

    float map(float value, float minA, float maxA, float minB, float maxB){
        float range = maxA - minA; 
        float valuePercent = (value - minA) / range;

        float newRange = maxB - minB;
        
        return valuePercent * newRange + minB;
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Player"){
            touching = true;
            //Debug.Log(score.score);
        }
    }

    void OnCollisionExit(Collision col){
        if(col.gameObject.tag == "Player"){
            touching = false;
        }
    }
}
