using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArm : MonoBehaviour
{
    public bool canMoveArm;
    float speed = 10.0f;
    Vector3 moveArm; 

    void Start()
    {
        
    }

    void Update()
    {
        if(canMoveArm){
            Move(); 
        }
    }

    void Move(){
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseZ = Input.GetAxisRaw("Mouse Y");

        moveArm = new Vector3(-mouseX * speed * Time.deltaTime, 0, mouseZ * speed * Time.deltaTime);
        transform.Translate(moveArm);
    }
}
