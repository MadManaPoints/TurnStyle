using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    float xRotation;
    float yRotation;
    float stopX = 55.0f; 
    [SerializeField] float sensitivity; 
    [SerializeField] Transform orientation; 
    void Start()
    {
        
    }

    void Update()
    {
        CameraControls(); 
    }

    void CameraControls(){
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity; 

        yRotation += mouseX; 
        xRotation -= mouseY;

        //Ensures the player can't look 360 degrees up and down
        xRotation = Mathf.Clamp(xRotation, -stopX, stopX);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); 
    }
}
