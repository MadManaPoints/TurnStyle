using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI signText;
    public float score = 290;
    int finalScore;
    float targetTime = 2.0f;
    bool changeText;

    void Start(){
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Update(){
        finalScore = Mathf.RoundToInt(score);

        if(player.end){
            scoreText.text = "Score: " + finalScore;
        } else if(player.stopAtTurnstile && !player.newPosition){
            targetTime -= Time.deltaTime;
            if(targetTime <= 0){
                TimerEnd();
            }

            if(!changeText){
                scoreText.text = "Press X";
                signText.text = "Press X";
            } else {
                scoreText.text = "All trains delayed";
                signText.text = "All trains delayed";
            }
            
        } else {
            scoreText.text ="All trains delayed";
            signText.text = "All trains delayed";
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
}
