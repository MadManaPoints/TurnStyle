using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passersby : MonoBehaviour
{
    [SerializeField] GameObject [] prefabs = new GameObject[4];
    [SerializeField] Transform [] spawns = new Transform[2];
    float targetTime = 5.0f;
    bool pick;
    int index;
    Quaternion direction;

    void Update()
    {
        targetTime -= Time.deltaTime;

        if(targetTime <= 0f){
            TimerEnd();
        }

        ChoosePerson();
    }

    void ChoosePerson(){
        if(pick){
            int picker = Random.Range(0, 4);
            index = picker;
            int chooseSpawn = Random.Range(0, 2);
            //Debug.Log(chooseSpawn);
            for(int i = 0; i < spawns.Length; i++){
                if(chooseSpawn == 1){
                    direction = Quaternion.Euler(spawns[chooseSpawn].localEulerAngles);
                } else {
                    direction = Quaternion.Euler(spawns[chooseSpawn].transform.localEulerAngles.x, 180f, spawns[chooseSpawn].transform.localEulerAngles.z);
                }
            }
            Instantiate(prefabs[index], spawns[chooseSpawn].position, direction);
            targetTime = 7.0f;
            pick = false;
        }
    }

    void TimerEnd(){
        pick = true;
    }
}
