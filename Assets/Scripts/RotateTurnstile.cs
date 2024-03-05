using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurnstile : MonoBehaviour
{
    public float x, y, z;
    public bool moveTurnstile; 
    float rotateSpeed = -0.3f;  
    void Start()
    {
        
    }

    void Update()
    {
        if(moveTurnstile){
            transform.Rotate(x, rotateSpeed, z); 
        } 
        
    }
}
