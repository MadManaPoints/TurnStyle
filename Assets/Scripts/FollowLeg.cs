using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLeg : MonoBehaviour
{   
    [SerializeField] Transform leg;
    void Start()
    {
        
    }

    void Update()
    {
        this.transform.position = new Vector3(leg.position.x, 2f, leg.position.z); 
        this.transform.localEulerAngles = leg.transform.localEulerAngles;
    }
}
