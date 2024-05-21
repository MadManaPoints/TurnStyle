using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    PlayerMovement player;
    PlayerCam playerCam;
    RightArm rightArm;
    RotateTurnstile turnstile;
    RightFoot rightFoot;
    FollowLeg leg;
    InputManager inputs;

    //these aren't used anymore - will delete when new text is set up
    [SerializeField] GameObject phaseOneText;
    [SerializeField] GameObject phaseTwoText;
    [SerializeField] GameObject phaseThreeText;
    //new text for Press B
    [SerializeField] GameObject newPhaseThreeText;
    bool grab;
    bool moveLeg;
    bool moveForward;
    public bool scanError;
    
    //this is for when I change text in code - for now I'm just disabling the gameObject
    //string phanseOneText = "Press X to Swipe MetroCard";
    
    //"Remember that Awake() is just like Start(), except it's called even before Start()" - Some guy
    void Awake(){
        inputs = new InputManager();

        //action map > action > .started || .performed || .canceled
        //Lambda expressions are kind of like mini functions
            //works by inserting a parameter on the left
        //inputs.Player.Phase1.performed += ctx => StartPhaseOne();
        //inputs.Player.Phase2.performed += ctx => StartPhaseTwo();
        //inputs.Player.Phase3.performed += ctx => StartPhaseThree();
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerCam = GameObject.Find("Main Camera").GetComponent<PlayerCam>();
        //the arm that we want the player to move
        rightArm = GameObject.Find("Right Hand_target").GetComponent<RightArm>();
        //turn the thing!!!!!!
        turnstile = GameObject.Find("Turn Thing").GetComponent<RotateTurnstile>();
        //should have called it right LEG
        rightFoot = GameObject.Find("Right Foot_target").GetComponent<RightFoot>();
        //detects the goal
        leg = GameObject.Find("Leg Collider").GetComponent<FollowLeg>();
        phaseOneText.SetActive(false); 
        phaseTwoText.SetActive(false);
        phaseThreeText.SetActive(false);
        newPhaseThreeText.SetActive(false);
    }

    void Update(){
        //grab = inputs.Player.Grab.IsInProgress();
        if(Input.GetAxisRaw("Grab") > 0){
            grab = true;
        } else {
            grab = false;
        }
        //Debug.Log(moveLeg);
        //moveLeg = inputs.Player.MoveLeg.IsInProgress();
        moveLeg = Input.GetButton("Move Leg");

        Phases();

        if(rightArm.swipes >= 3 && rightArm.swiped){
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

        if(rightArm.phoneSwapped && !player.finalPos){
            player.newPosition = true;
        }

        if(player.newPosition && scanError){
            phaseThreeText.SetActive(true);
            newPhaseThreeText.SetActive(true);
        }

        if(player.finalPos){
            phaseThreeText.SetActive(false);
            newPhaseThreeText.SetActive(false);
            //Debug.Log(grab);
            turnstile.moveTurnstile = grab;
            if(grab){
                rightFoot.canMoveLeg = moveLeg;
                player.movingLeg = moveLeg;
            }
            player.end = leg.screwTheMTA;
            playerCam.endCamPos = player.end;
        }

        if(player.canRestart && Input.GetButton("Restart")){
            Reset();
            Time.timeScale = 1;
        }

        QuitGame();
        //Debug.Log(grab);
    }

    void Phases(){
        if(Input.GetButtonDown("Phase 1")){
            StartPhaseOne();
        }

        if(Input.GetButtonDown("Phase 2")){
            StartPhaseTwo();
        }

        if(Input.GetButtonDown("Phase 3")){
            StartPhaseThree();
        }
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
        if(rightArm.swipes >= 3){
            rightArm.pressedY = true;
        }
    }

    void StartPhaseThree(){
        if(player.newPosition && scanError){
            //Debug.Log("THAT'S IT FOR NOW");
            player.finalPos = true;
            rightArm.finalPhase = true;
            playerCam.finalCamPos = true;
        }
    }

    void Reset(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    void QuitGame(){
        if (Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }
    }

    void OnEnable(){
        //inputs.Player.Enable();
    }

    void OnDisable(){
        //inputs.Player.Disable();
    } 

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            //Debug.Log("YERR");
            player.stopAtTurnstile = true;
            if(!player.finalPos){
                phaseOneText.SetActive(true);
            }
        }
    }
}
