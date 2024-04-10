using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightShoulder : MonoBehaviour
{
    RightArm rightArm;
    float speed = 50.0f;
    [SerializeField] Transform over; 
    [SerializeField] Vector3 shoulderRotation;
    Vector3 offset = new Vector3(0, 0.1f, 0); 
    void Start()
    {
        rightArm = GameObject.Find("Bone.003_R.002").GetComponent<RightArm>();
    }

    void LateUpdate()
    {
        if(rightArm.canMoveArm){
            Move();
        } else {
            over.position = this.transform.position;
        }
    }

    void Move(){
        this.transform.position = over.position;
        //this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        float move = Input.GetAxis("Mouse Y"); 
        over.Rotate(new Vector3(0, move, 0) * speed * Time.deltaTime);
    }
}
