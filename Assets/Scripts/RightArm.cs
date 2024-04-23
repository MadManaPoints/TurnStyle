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
    public bool finalPhase;
    bool inFinalPos;
    //Vector3 phonePosition; 
    float speed = 1.2f;
    //[SerializeField] Transform over;
    //[SerializeField] Vector3 armRotation;
    Vector3 offset = new Vector3(0.1f, 0, 0);
    Vector3 newOffset = new Vector3(0.3f, 0, 0);
    [SerializeField] Vector3 pos;
    Vector3 nextPos = new Vector3(1.309181f, 2.5f, 1.409181f);
    Vector3 finalPos = new Vector3(0,0,0);
    [SerializeField] Transform bar;
    [SerializeField] Vector3 barOffset; 
    
    void Start()
    {

    }

    void Update(){
        if(finalPhase && inFinalPos){
            TurnStylePosition();
        }
    }

    void LateUpdate()
    {
        //Debug.Log(canMoveArm);
        if(canMoveArm){
            if(!swapToPhone && !finalPhase){
                Move();
            } else if(finalPhase){
                if(!inFinalPos){
                    pos = finalPos;
                    inFinalPos = true;
                }
            } else if(swapToPhone){
                if(!inPhone){
                    pos = nextPos;
                    inPhone = true;
                } else {
                    Move();
                }
            }
        }

        //Debug.Log(swapToPhone);
    }

    void Move(){
        //this.transform.position = over.position;
        //this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x, over.transform.localEulerAngles.y, this.transform.eulerAngles.z);
        float moveZ = Input.GetAxis("Mouse X");
        float moveX = Input.GetAxis("Mouse Y");
        

        if(!swapToPhone){
            //Debug.Log(pos.z);
            SwipePosition(moveX, moveZ);
        } else if(!finalPhase) {
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

    void TurnStylePosition(){
        transform.position = bar.position + barOffset;
        //Debug.Log("YERR");
    }
}
