using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    PlayerMovement player;
    PlayerCam cam;
    [SerializeField] GameObject SliderObj; 
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI signText;
    [SerializeField] GameObject halt1, halt2; 
    public float score = 290;
    int finalScore;
    float targetTime = 2.0f;
    bool changeText;
    float sus; 

    void Start(){
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        cam = GameObject.Find("Main Camera").GetComponent<PlayerCam>();

        SliderObj.SetActive(false);
    }

    void Update(){
        if(player.finalPos && !player.end){
            SliderObj.SetActive(true);
        } else {
            SliderObj.SetActive(false);
        }

        slider.value = map(score, 290, 50, 0, 100); 
        //Debug.Log(slider.value);

        if(slider.value >= 100){
            player.lose = true;
            SliderObj.SetActive(false);
        }

        finalScore = Mathf.RoundToInt(score);
        if(player.end){
            halt2.SetActive(false);
            scoreText.text = "Score: " + finalScore;
        } else if(player.stopAtTurnstile && !player.newPosition && !player.finalPos){
            targetTime -= Time.deltaTime;
            if(targetTime <= 0){
                TimerEnd();
            }

            if(!changeText){
                scoreText.text = "Press X";
                signText.text = "Press X";
                halt1.SetActive(false);
                halt2.SetActive(false);
            } else {
                scoreText.text = "All trains delayed";
                signText.text = "All trains delayed";
                halt1.SetActive(true);
                halt2.SetActive(true);
            }
            
        } else if(cam.acab){
            scoreText.text = "ACAB";
            signText.text = "ACAB";
            halt1.SetActive(false);
            halt2.SetActive(false);
        } else {
            scoreText.text ="All trains delayed";
            signText.text = "All trains delayed";
            halt1.SetActive(true);
            halt2.SetActive(true);
        }
            
    }
    public void SubtractScore(){
        //Debug.Log(score);
        score -= 50 * Time.deltaTime;
    }

    void TimerEnd(){
        changeText = !changeText;
        targetTime = 2.0f; 
    }

    float map(float value, float minA, float maxA, float minB, float maxB){
        float range = maxA - minA; 
        float valuePercent = (value - minA) / range;

        float newRange = maxB - minB;
        
        return valuePercent * newRange + minB;
    }
}
