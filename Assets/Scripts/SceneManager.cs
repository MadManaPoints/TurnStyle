using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    PlayerMovement player;
    PlayerCam playerCam;
    RightArm rightArm;
    InputManager inputs;
    [SerializeField] GameObject phaseOneText; 
    //this is for when I change text in code - for now I'm just disabling the gameObject 
    //string phanseOneText = "Press X to Swipe MetroCard"; 
    
    //"Remember that Awake() is just like Start(), except it's called even before Start()" - Some guy 
    void Awake(){
        inputs = new InputManager(); 

        //action map > action > .started || .performed || .canceled 
        //Lambda expressions are kind of like mini functions 
            //works by inserting a parameter on the left

        inputs.Player.Phase1.performed += ctx => StartPhaseOne(); 
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerCam = GameObject.Find("Main Camera").GetComponent<PlayerCam>();
        //the arm that we want the player to move
        rightArm = GameObject.Find("Bone.003_R.002").GetComponent<RightArm>();
        phaseOneText.SetActive(false); 
    }

    void StartPhaseOne(){
        if(player.stopAtTurnstile){
            player.phaseOne = true;
            rightArm.canMoveArm = true;
            playerCam.canMoveCam = false;
            //Debug.Log(player.phaseOne);
            phaseOneText.SetActive(false); 
        }
    }

    void OnEnable(){
        inputs.Player.Enable(); 
    }

    void OnDisable(){
        inputs.Player.Disable(); 
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            //Debug.Log("YERR");
            player.stopAtTurnstile = true;
            phaseOneText.SetActive(true);
        }
    }
}
