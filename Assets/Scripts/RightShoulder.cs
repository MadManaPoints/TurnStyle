using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightShoulder : MonoBehaviour
{
    RightArm rightArm;
    [SerializeField] Transform over; 
    Vector3 offset = new Vector3(-0.1f, 0, 0); 
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
        this.transform.position = over.position + offset;
    }
}
