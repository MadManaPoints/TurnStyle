using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurnstile : MonoBehaviour
{
    //public float x, y, z; //this was to test rotation - use z to move new turnstile 
    public bool moveTurnstile;
    public bool stuck; 
    public float rotateSpeed = 0f;
    void Start()
    {
        
    }

    void Update()
    {
        if(moveTurnstile){
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            if(transform.rotation.z < -0.05f){
                moveTurnstile = false;
                stuck = true; 
            }
        } 
        //Debug.Log(transform.rotation.z);
    }
}
