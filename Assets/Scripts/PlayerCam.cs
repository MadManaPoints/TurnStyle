using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    float xRotation;
    float yRotation;
    float stopX = 55.0f; 
    [SerializeField] float stopY = 200.0f; 
    [SerializeField] float sensitivity;
    public bool canMoveCam = true;
    Vector3 phaseOnePos = new Vector3(36.0f, 25.0f, 5.0f);
    Vector3 velocity = Vector3.zero;
    //[SerializeField] Transform orientation;
    void Start()
    {
        
    }

    void Update()
    {   
        if(canMoveCam){
            CameraControls();
        } else {
            PhaseOneCamPos();
        }
         
    }

    void CameraControls(){
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity; 

        yRotation += mouseX; 
        xRotation -= mouseY;

        //Ensures the player can't look 360 degrees up and down
        xRotation = Mathf.Clamp(xRotation, -stopX, stopX);
        yRotation = Mathf.Clamp(yRotation, 15, 120);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void PhaseOneCamPos(){
        transform.localEulerAngles = Vector3.SmoothDamp(transform.localEulerAngles, phaseOnePos, ref velocity, 0.3f);
    }
}
