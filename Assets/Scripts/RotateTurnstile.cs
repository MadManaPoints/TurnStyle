using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateTurnstile : MonoBehaviour
{
    RightArm rightArm;
    //public float x, y, z; //this was to test rotation - use z to move new turnstile 
    public bool moveTurnstile;
    public bool stuck;
    bool grabSwitch;
    bool [] positionSwitch = new bool [6]; 
    public float rotateSpeed = 3.0f;
    static float t = 0.0f;
    int index = 3;
    Vector3 velocity = Vector3.zero;
    Transform startPos;
    Quaternion initialPos;
    [SerializeField] float stopTurnstile;
    Vector3 rotateVel = Vector3.zero; 
    Vector3 rotateAcc;
    [SerializeField] Vector3 [] barPositions = new Vector3[6];
    bool canMove = true;
    [SerializeField] Transform follow; 
    Rigidbody rb;
    HingeJoint joint;
    void Start()
    {
        rightArm = GameObject.Find("Right Hand_target").GetComponent<RightArm>();
        //startPos = this.transform;
        //startPos.localEulerAngles = transform.localEulerAngles;
        //initialPos = this.transform.rotation;
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<HingeJoint>();
        
        //makes Vector3 for each position it can stop at 
        for(int i = 0; i < barPositions.Length; i++){
            barPositions[i] = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 60 * i); 
        }

        barPositions[0] = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 360.0f);
        transform.localEulerAngles = barPositions[index];
        follow.localEulerAngles = barPositions[index];
        //startPos.localEulerAngles = follow.localEulerAngles; 
        positionSwitch[index] = true;  
    }

    void Update()
    {
        if(moveTurnstile){
            SpinTurnstile(); 
        }
        PositionCheck();
    }

    void FixedUpdate(){
        if(moveTurnstile){
            SpinTurnstileHinge();
        }
    }

    void TorqueTest(){
       //Debug.Log(follow.rotation);
    }

    void SpinTurnstileHinge(){
        //OK THIS IS THE ONE!!
        //OMG IT'S ACTUALLY WORKING 
        //NOT EVEN USING THE HINGE :') 
        if (Vector3.Distance(transform.localEulerAngles, follow.position) > 0.2f){
                Vector3 turn = follow.localEulerAngles - transform.localEulerAngles;
                rb.angularVelocity = turn;
        } else {
            rb.angularVelocity = Vector3.zero;
        }
    }

    void SpinTurnstile(){
        //this works well enough but it will ignore collisions this way 
        float moveY = Input.GetAxis("Mouse Y") * (1.0f + Time.deltaTime);

        float rotateZ = map(moveY, -1, 1, 120, 240);
        //rotateZ = Mathf.Min(rotateZ, stopTurnstile);
        float rotateTurnstile = Mathf.Lerp(follow.localEulerAngles.z, rotateZ, t);
        t += 0.5f * Time.deltaTime;
        follow.localEulerAngles = new Vector3(follow.localEulerAngles.x, follow.localEulerAngles.y, -rotateTurnstile);
        //Debug.Log(moveY);
    }

    void PositionCheck(){
        //GAHHHHH
        for(int i = 0; i < positionSwitch.Length; i++){
            if(positionSwitch[i]){
                index = i;
            }
        }

        //returning to this later
        //Debug.Log(index); 
    }

    float map(float value, float minA, float maxA, float minB, float maxB){
        float range = maxA - minA; 
        float valuePercent = (value - minA) / range;

        float newRange = maxB - minB;
        
        return valuePercent * newRange + minB;
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Player"){
            Debug.Log("YERR");
        }
    }
}
