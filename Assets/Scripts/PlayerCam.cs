using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    float xRotation;
    float yRotation;
    float targetTime = 2.0f; 
    //[SerializeField] float stopY = 175.0f; 
    [SerializeField] float sensitivity;
    public bool canMoveCam = true;
    public bool nextPos;
    public bool finalCamPos; 
    public bool endCamPos;
    public bool acab;
    bool setFinalPos;
    [SerializeField] Vector3 phaseOnePos = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] Vector3 phaseTwoPos = new Vector3(0.0f, 0.0f, 0.0f); 
    [SerializeField] Vector3 lookAtCop;
    [SerializeField] Vector3 phaseThreePos = new Vector3(0, 0, 0);
    Vector3 velocity = Vector3.zero;
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 newOffset;
    [SerializeField] Vector3 finalOffset;
    
    void Start()
    {
        transform.localEulerAngles = new Vector3(0, 90f, 0);
    }

    void Update()
    {   
        if(canMoveCam){
            CameraControls();
        } else if(endCamPos){
            EndCam();
        } else if(finalCamPos){
            PhaseThreeCamPos();
        } else if(nextPos){
            PhaseTwoCamPos();
        } else {
            PhaseOneCamPos();
        }
    }

    void CameraControls(){
        transform.localEulerAngles = new Vector3(0, 90, 0);
        transform.position = player.transform.position + offset;
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;

        //Ensures the player can't look 360 degrees up and down
        //limiting movement further until I can figure out why the camera spins when looking too far up
        xRotation = Mathf.Clamp(xRotation, -15, 20.5f);
        yRotation = Mathf.Clamp(yRotation, 40, 150);
        //Debug.Log(transform.localEulerAngles);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void PhaseOneCamPos(){
        transform.position = player.transform.position + offset;
        transform.localEulerAngles = Vector3.SmoothDamp(transform.localEulerAngles, phaseOnePos, ref velocity, 0.3f);
    }

    void PhaseTwoCamPos(){
        transform.position = player.transform.position + newOffset;
        transform.localEulerAngles = Vector3.SmoothDamp(transform.localEulerAngles, phaseTwoPos, ref velocity, 0.3f);
    }

    void PhaseThreeCamPos(){
        transform.position = player.transform.position + finalOffset;
        if(!setFinalPos){
            acab = true;
            transform.localEulerAngles = Vector3.SmoothDamp(transform.localEulerAngles, lookAtCop, ref velocity, 0.3f);
            if(Vector3.Distance(transform.localEulerAngles, lookAtCop) < 0.2f){
                setFinalPos = true;
            }
        } else {
            transform.localEulerAngles = Vector3.SmoothDamp(transform.localEulerAngles, phaseThreePos, ref velocity, 0.3f);
            if(targetTime > 0f){
                targetTime -= Time.deltaTime;
            } else {
                targetTime = 0f;
                acab = false;
            }
        }
        
    }

    void EndCam(){
        transform.position = player.transform.position + offset;
        transform.localEulerAngles = Vector3.SmoothDamp(transform.localEulerAngles, new Vector3(0, 90f, 0), ref velocity, 0.3f);
    }
}
