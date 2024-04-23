using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLeg : MonoBehaviour
{   
    [SerializeField] Transform leg;
    public bool screwTheMTA; 
    void Start()
    {
        
    }

    void Update()
    {
        this.transform.position = new Vector3(leg.position.x + 0.1f, leg.position.y - 0.1f, leg.position.z); 
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "End"){
            screwTheMTA = true; 
        }
    }
}
