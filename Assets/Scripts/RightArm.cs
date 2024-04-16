using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class RightArm : MonoBehaviour
{
    public bool canMoveArm;
    public bool pressedY;
    public bool swiped;
    public bool swapToPhone;
    bool inPhone; 
    public bool phoneSwapped;
    //Vector3 phonePosition; 
    float speed = 1.0f;
    //[SerializeField] Transform over; 
    //[SerializeField] Vector3 armRotation;
    Vector3 offset = new Vector3(0.1f, 0, 0);
    Vector3 newOffset = new Vector3(0.3f, 0, 0);
    [SerializeField] Vector3 pos;
    Vector3 nextPos = new Vector3(1.309181f, 2.5f, 1.409181f);
    
    void Start()
    {

    }

    void LateUpdate()
    {
        //Debug.Log(canMoveArm);
        if(canMoveArm){
            if(!swapToPhone){
                Move();
            } else if(swapToPhone){
                //Debug.Log(swapToPhone);
                if(!inPhone){
                    pos = nextPos;
                    inPhone = true;
                } else {
                    Move();
                }
            }
        }
    }

    void Move(){
        //this.transform.position = over.position;
        //this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x, over.transform.localEulerAngles.y, this.transform.eulerAngles.z);
        float moveZ = Input.GetAxis("Mouse X");
        float moveX = Input.GetAxis("Mouse Y");
        

        if(!swapToPhone){
            SwipePosition(moveX, moveZ);
        } else {
            Debug.Log(pos.z);
            HoldingPhone(moveX, moveZ);
        }
        //over.Rotate(new Vector3(0, move, 0) * speed * Time.deltaTime);
    }

    void SwipePosition(float tempX, float tempZ){
        transform.position = pos + offset;
        pos.y = 2.5f;
        pos += new Vector3(tempX * speed * Time.deltaTime, 0, -tempZ * speed * Time.deltaTime);

        if(pos.x > 2.0f){
            pos.x = 2.0f;
            swiped = true;
        } else if(pos.x < 1.6f){
            pos.x = 1.6f;
        }

        if(pos.z > 1.8f){
            pos.z = 1.8f;
        } else if (pos.z < 1.0f){
            pos.z = 1.0f;
        }
    }

    void HoldingPhone(float tempY, float tempZ){
        transform.position = pos + newOffset;
        phoneSwapped = true;
        pos += new Vector3(0, tempY * speed * Time.deltaTime, -tempZ * speed * Time.deltaTime);

        if(pos.y > 2.5f){
            pos.y = 2.5f;
        } else if(pos.y < 2.2f){
            pos.y = 2.2f;
        }

        if(pos.z > 1.65f){
            pos.z = 1.65f;
        } else if(pos.z < 1.08f){
            pos.z = 1.08f;
        }
    }
}
