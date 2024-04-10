using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RightArm : MonoBehaviour
{
    public bool canMoveArm;
    float speed = 50.0f;
    [SerializeField] Transform over; 
    [SerializeField] Vector3 armRotation;
    Vector3 offset = new Vector3(0.3f, 0, 0); 
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if(canMoveArm){
            Move();
        } else {
            over.position = this.transform.position; 
        }
    }

    void Move(){
        this.transform.position = over.position + offset;
        this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x, over.transform.localEulerAngles.y, this.transform.eulerAngles.z);
        float move = Input.GetAxis("Mouse X"); 
        over.Rotate(new Vector3(0, move, 0) * speed * Time.deltaTime);
    }
}
