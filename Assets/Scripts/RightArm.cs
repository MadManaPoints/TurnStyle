using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
public class RightArm : MonoBehaviour
{
    public bool canMoveArm;
    public bool pressedY;
    public bool swiped;
    public bool swapToPhone;
    bool inPhone; 
    public bool phoneSwapped;
    public bool finalPhase;
    public bool swipe; 
    public int swipes = 0;
    public bool letGo;
    bool yPos;
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
    [SerializeField] Vector3 angleOffset;
    [SerializeField] GameObject funds;
    [SerializeField] TextMeshProUGUI fundsText;
    Vector3 velocity = Vector3.zero;
    void Start()
    {
        funds.SetActive(false);
    }

    void Update(){
        //Debug.Log(swipes);
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
        if(!yPos){
            pos.y = 2.5f;
            yPos = true;
        } else {
            transform.position = pos + offset;
            pos += new Vector3(tempX * speed * Time.deltaTime, 0, 0);//-tempZ * speed * Time.deltaTime);

            if(pos.x > 1.82f){
                pos.x = 1.82f;
                //swiped = true;
            } else if(pos.x < 1.4f){
            pos.x = 1.4f;
            }

        //if(pos.z > 1.7f){
        //    pos.z = 1.7f;
        //} else if (pos.z < 1.2f){
        //    pos.z = 1.2f;
        //}
            pos.z = 1.37341f;
            //Debug.Log(pos.y);

            if(pos.x > 1.75f){
                //Debug.Log("YERR");
                if(pos.y < 2.58f && tempX != 0){
                    pos.y += Time.deltaTime/2;
                } else {
                    if(!swipe && Input.GetAxis("Mouse Y") > 0.5f){
                        swipes += 1;
                        swipe = true;
                        if(swipes < 3){
                            fundsText.text = "Please Swipe Again";
                        } else {
                            fundsText.text = "Insufficient Funds";
                        }
                        funds.SetActive(true);
                    } else {
                        pos.y = 2.58f;
                    } 
                }
            } else if(pos.x < 1.5f && tempX != 0){
                if(pos.y > 2.5f){
                    if(swipes < 3){
                        funds.SetActive(false);
                    } else {
                        fundsText.text = "Press Y";
                    }
                    pos.y -= Time.deltaTime/2;
                    swipe = false;
                } else {
                    pos.y = 2.5f;
                }
            }
        }
        
        
    }

    void HoldingPhone(float tempY, float tempZ){
        funds.SetActive(false);
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
        if(letGo){
            //Debug.Log(letGo);
            transform.position = Vector3.SmoothDamp(transform.position, bar.position + barOffset, ref velocity, 0.2f);
            if(Vector3.Distance(transform.position, bar.position + offset) < 0.2f){
                letGo = false;
            }
        } else {
            transform.position = bar.position + barOffset;
        }
        
        transform.localEulerAngles = angleOffset; 
        //Debug.Log("YERR");
    }
}
