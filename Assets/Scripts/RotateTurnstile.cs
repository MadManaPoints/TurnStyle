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
    public float rotateSpeed = 3.0f;
    static float t = 0.0f;
    Transform startPos;
    [SerializeField] float stopTurnstile;
    void Start()
    {
        rightArm = GameObject.Find("Right Hand_target").GetComponent<RightArm>();
        startPos = this.transform;
        startPos.localEulerAngles = transform.localEulerAngles;
    }

    void Update()
    {
        if(moveTurnstile){
            SpinTurnstile();
        }
    }

    void SpinTurnstile(){
        float moveY = Input.GetAxis("Mouse Y") * (1.0f + Time.deltaTime);

        float rotateZ = map(moveY, -1, 1, -60, 60);
        rotateZ = Mathf.Min(rotateZ, stopTurnstile);
        float rotateTurnstile = Mathf.Lerp(startPos.localEulerAngles.z, rotateZ, t);
        startPos.localEulerAngles = new Vector3(startPos.localEulerAngles.x, startPos.localEulerAngles.y, -rotateTurnstile);
        //Debug.Log(moveY);
        t += 0.5f * Time.deltaTime;
    }

    float map(float value, float minA, float maxA, float minB, float maxB){
        float range = maxA - minA; 
        float valuePercent = (value - minA) / range;

        float newRange = maxB - minB;
        
        return valuePercent * newRange + minB;
    }
}
