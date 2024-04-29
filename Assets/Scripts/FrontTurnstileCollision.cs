using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTurnstileCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Player"){
            Debug.Log("HIT PLAYER"); 
        }
    }
}
