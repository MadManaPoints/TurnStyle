using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    float xRotation;
    float yRotation;
    float stopX = 40.0f; 
    //[SerializeField] float stopY = 175.0f; 
    [SerializeField] float sensitivity;
    public bool canMoveCam = true;
    public bool nextPos;
    public bool finalCamPos; 
    public bool endCamPos;
    [SerializeField] Vector3 phaseOnePos = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] Vector3 phaseTwoPos = new Vector3(0.0f, 0.0f, 0.0f); 
    [SerializeField] Vector3 phaseThreePos = new Vector3(0, 0, 0);
    Vector3 velocity = Vector3.zero;
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 newOffset;
    [SerializeField] Vector3 finalOffset;
    
    void Start()
    {
        
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
        transform.position = player.transform.position + offset;
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;

        //Ensures the player can't look 360 degrees up and down
        xRotation = Mathf.Clamp(xRotation, -stopX, 55);
        yRotation = Mathf.Clamp(yRotation, 30, 150);

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
        transform.localEulerAngles = Vector3.SmoothDamp(transform.localEulerAngles, phaseThreePos, ref velocity, 0.5f);
    }

    void EndCam(){
        transform.position = player.transform.position + offset;
        transform.localEulerAngles = Vector3.SmoothDamp(transform.localEulerAngles, new Vector3(0, 90f, 0), ref velocity, 0.3f);
    }
}
