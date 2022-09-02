using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private List<Quaternion> previousRotations;

    private List<float> previousAngleDifferences; 

    public Material stationaryMaterial;
    public Material movingMaterial;
    public GameObject rotatingObject;

    private enum Mode {STATIONARY,MOVING,RESETTING,MENU};
    private Mode currentMode = Mode.MENU;

    private float previousTime = 0;

    private float resetTime = 0;

    private float lastTime = 0;


    private Quaternion latestRotation = new Quaternion(0f,0f,0f,0f);


    // Start is called before the first frame update
    void Start()
    {
        previousAngleDifferences = new List<float>();
        previousRotations = new List<Quaternion>();
        previousRotations.Add(new Quaternion(0.0f,0.0f,0.0f,0.0f));

        lastTime = Time.fixedDeltaTime;
    }

    public void Rotate(Quaternion newRotation){
        latestRotation = newRotation;
    }

    void Update(){

        Quaternion previousRotation = previousRotations[previousRotations.Count-1];
            
        float angle = Quaternion.Angle(previousRotation, latestRotation);

        previousAngleDifferences.Add(angle);

        if(previousAngleDifferences.Count >= 300){
            previousAngleDifferences.RemoveAt(0);
        }
    
        //calulate average change of angle
		float average = calculateAverageAngleChange();

        switch (currentMode){
            
            case Mode.MENU:

                transform.localRotation = latestRotation;

                break;
            case Mode.STATIONARY:

                //check if user has started to move the object
                if(average > 0.01f){
                    currentMode = Mode.MOVING;
                    rotatingObject.GetComponent<Renderer>().material = movingMaterial;
                }

                break;

            case Mode.MOVING:

                //check if user has stopped moving the object
                if(average <= 0.01f){
                    currentMode = Mode.RESETTING;
                }

                previousRotations.Add(latestRotation);

                previousTime += Time.deltaTime;

                if(previousTime > (Time.deltaTime * 2)){
                    transform.localRotation = previousRotations[0];
                    previousRotations.RemoveAt(0);
                    previousTime = 0;
                }

                break;

            case Mode.RESETTING:
                
                /*Debug.Log(resetTime);

                if(resetTime <= 1){

                    resetTime += Time.deltaTime;

                    transform.localRotation  = Quaternion.Lerp(previousRotations[0], previousRotations[previousRotations.Count-1],resetTime);

                }else{
                    */

                    resetTime = 0;

                    currentMode = Mode.STATIONARY;
                    rotatingObject.GetComponent<Renderer>().material = stationaryMaterial;

                    Quaternion mostRecentRotation = previousRotations[previousRotations.Count-1];
                    
                    transform.localRotation = mostRecentRotation;

                    previousRotations.Clear();
                    previousRotations.Add(mostRecentRotation);

              //  }


                
                break;
        }

    }


   private float calculateAverageAngleChange(){

        float total = 0;

        for(int i = 0; i< previousAngleDifferences.Count; i++){
            total += previousAngleDifferences[i];
        }

        float average = total/previousAngleDifferences.Count;

        return average;
   }


/**
* If the menu is visible, the object should not have any delay
**/
   public void ToggleMenu(bool inMenu){

        if(inMenu){
            currentMode = Mode.MENU;
        }else{
            currentMode = Mode.STATIONARY;
        }
   }



}
