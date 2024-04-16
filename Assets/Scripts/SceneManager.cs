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
    [SerializeField] GameObject phaseTwoText;
    [SerializeField] GameObject phaseThreeText;
    bool grab;
    bool moveLeg;
    bool moveForward;
    
    //this is for when I change text in code - for now I'm just disabling the gameObject
    //string phanseOneText = "Press X to Swipe MetroCard";
    
    //"Remember that Awake() is just like Start(), except it's called even before Start()" - Some guy
    void Awake(){
        inputs = new InputManager();

        //action map > action > .started || .performed || .canceled
        //Lambda expressions are kind of like mini functions
            //works by inserting a parameter on the left
        inputs.Player.Phase1.performed += ctx => StartPhaseOne();
        inputs.Player.Phase2.performed += ctx => StartPhaseTwo();
        inputs.Player.Phase3.performed += ctx => StartPhaseThree();
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerCam = GameObject.Find("Main Camera").GetComponent<PlayerCam>();
        //the arm that we want the player to move
        rightArm = GameObject.Find("Right Hand_target").GetComponent<RightArm>();
        phaseOneText.SetActive(false); 
        phaseTwoText.SetActive(false);
        phaseThreeText.SetActive(false);
    }

    void Update(){
        if(rightArm.swiped){
                phaseTwoText.SetActive(true);
            }

         if(rightArm.pressedY){
            player.phaseTwo = true;
            phaseTwoText.SetActive(false);
            player.phaseOne = false;
            playerCam.nextPos = true; 
            rightArm.swiped = false;
            rightArm.pressedY = false;
            rightArm.swapToPhone = true;
        }

        if(rightArm.phoneSwapped){
            player.newPosition = true;
            phaseThreeText.SetActive(true);
        }

        //Debug.Log(moveForward);

        grab = inputs.Player.Grab.IsInProgress();
        moveLeg = inputs.Player.MoveLeg.IsInProgress();
        moveForward = inputs.Player.MoveForward.IsInProgress();
    }

    void StartPhaseOne(){
        if(player.stopAtTurnstile && !playerCam.nextPos){
            player.phaseOne = true;
            rightArm.canMoveArm = true;
            playerCam.canMoveCam = false;
            //Debug.Log(player.phaseOne);
            phaseOneText.SetActive(false);
        }
    }

    void StartPhaseTwo(){
        if(rightArm.swiped){
            rightArm.pressedY = true;
        }
    }

    void StartPhaseThree(){
        if(player.newPosition){
            Debug.Log("THAT'S IT FOR NOW");
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
