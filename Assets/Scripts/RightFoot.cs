using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightFoot : MonoBehaviour
{   
    public bool canMoveLeg;
    float speed = 1.0f;
    Vector3 pos;
    [SerializeField] Vector3 offset; 
    [SerializeField] Vector2 x;
    [SerializeField] Vector2 z;
    void Start()
    {
        
    }

    void Update()
    {
        if(!canMoveLeg){
            pos = transform.position;
        } else {
            Move();
        }
         
    }

    void Move(){
        float moveX = Input.GetAxis("Vertical") * (1.0f + Time.deltaTime);
        float moveZ = Input.GetAxis("Horizontal") * (1.0f + Time.deltaTime);

        MoveRightLeg(moveX, moveZ);
        //Debug.Log(transform.position.z);
    }

    void MoveRightLeg(float tempX, float tempZ){
        transform.position = pos + offset;

        //values in editor
        float moveX = map(tempX, -1, 1, x.x, x.y);
        float moveZ = map(tempZ, -1, 1, z.x, z.y);

        pos = new Vector3(moveX, 0, -moveZ);
        //Debug.Log(pos);
    }

    float map(float value, float minA, float maxA, float minB, float maxB){
        float range = maxA - minA; 
        float valuePercent = (value - minA) / range;

        float newRange = maxB - minB;
        
        return valuePercent * newRange + minB;
    }
}
