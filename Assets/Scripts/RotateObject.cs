using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateObject : MonoBehaviour
{
    private List<Quaternion> previousRotations;

    private List<float> previousAngleDifferences; 


    public GameObject rotatingObject;

    public Slider waitTimeSlider;

    private enum Mode {STATIONARY,MOVING,RESETTING,MENU};
    private Mode currentMode = Mode.MENU;

    private float previousTime = 0;

    private float movingTime = 0;

    private float resetTime = 0;

    public float currentTransparency = 0;

    private Quaternion latestRotation = new Quaternion(0f,0f,0f,0f);

    // Start is called before the first frame update
    void Start()
    {
        previousAngleDifferences = new List<float>();
        previousRotations = new List<Quaternion>();
        previousRotations.Add(new Quaternion(0.0f,0.0f,0.0f,0.0f));
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

        previousRotations.Add(latestRotation);

        //calulate average change of angle
		float average = calculateAverageAngleChange();

        Debug.Log(average);

        switch (currentMode){
            
            case Mode.MENU:

                transform.localRotation = latestRotation;
            
                break;
            case Mode.STATIONARY:

                //check if user has started to move the object
                if(average > 0.01f){
                    currentMode = Mode.MOVING;
                    movingTime = 0;
                }

                break;

            case Mode.MOVING:

/*
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
*/

                //for testing
                transform.localRotation = latestRotation;

                //set transparency
                if(movingTime < 10){
                    currentTransparency = movingTime/10;
                    
                   // Debug.Log(currentTransparency);
                
                    setTransparency(currentTransparency);
                 
                    movingTime += Time.deltaTime;
                }else{
                       setTransparency(1);
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

            setTransparency(0);

        }
   }

   private void setTransparency(float t){

        GameObject[] parts =  GameObject.FindGameObjectsWithTag("part");

        foreach (GameObject part in parts)
        {
            Color color = part.GetComponent<MeshRenderer>().material.color ;
            color.a = t;
            part.GetComponent<MeshRenderer>().material.color = color ; 
        }
   }



}
