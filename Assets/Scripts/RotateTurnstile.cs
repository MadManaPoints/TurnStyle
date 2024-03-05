using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurnstile : MonoBehaviour
{
    //public float x, y, z; this was to test rotation 
    public bool moveTurnstile;
    float rotateSpeed = -150.0f;
    void Start()
    {
        
    }

    void Update()
    {
        if(moveTurnstile){
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0); 
        } 

        //Debug.Log(transform.rotation.y); 
        
    }
}
