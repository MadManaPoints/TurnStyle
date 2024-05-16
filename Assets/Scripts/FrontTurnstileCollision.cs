using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTurnstileCollision : MonoBehaviour
{
    Score score;
    bool touching;

    void Start(){
        score = GameObject.Find("Score").GetComponent<Score>();
    }

    void Update(){
        if(touching){
            score.SubtractScore();
        }
    }
    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Player"){
            touching = true;
            //Debug.Log(score.score);
        }
    }

    void OnCollisionExit(Collision col){
        if(col.gameObject.tag == "Player"){
            touching = false;
        }
    }
}
