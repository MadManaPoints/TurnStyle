using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    PlayerMovement player;
    PlayerCam playerCam;
    RightArm rightArm;
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerCam = GameObject.Find("Main Camera").GetComponent<PlayerCam>();
        rightArm = GameObject.Find("Bone.003_R.001").GetComponent<RightArm>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            //Debug.Log("YERR");
            player.phaseOne = true; 
            playerCam.canMoveCam = false; 
            rightArm.canMoveArm = true;
        }
    }
}
