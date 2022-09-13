using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateObject : MonoBehaviour
{
    private List<Quaternion> previousRotations;

    private List<float> previousAngleDifferences;

    public Slider waitTimeSlider;

    private enum ObjectMode { INHAND, PUTDOWN }; //is the object being held or has it been put down
    private ObjectMode currentObjectMode = ObjectMode.PUTDOWN;

    private enum Mode { STATIONARY, MOVING, DORMANT, MENU };

    private Mode currentMode = Mode.MENU;

    private enum FadeMode { FADEIN, FADEOUT, NONE };
    private FadeMode currentFadeMode = FadeMode.NONE;

    private float previousTime = 0;

    private float currentTransparency = 1;
    private float targetTransparency = 1;

    //public int fadeOutTime = 20;
    public Slider FadeOutSlider;
    //private int fadeInTime = 60;

    public Slider FadeInSlider;
    private float transparencySpeed;

    


    private Quaternion latestRotation = new Quaternion(0f, 0f, 0f, 0f);

    private Quaternion previousRotation = new Quaternion(0f, 0f, 0f, 0f);


    public Slider LowestTransparencySlider;

    public Toggle mouse_toggle;

    // Start is called before the first frame update
    void Start()
    {
        previousAngleDifferences = new List<float>();
        previousRotations = new List<Quaternion>();
       
        transparencySpeed = 1.0f / (FadeOutSlider.value * 30.0f);
    }

    public void Rotate(Quaternion newRotation)
    {
        latestRotation = newRotation;
    }

    void Update()
    {
        
        if(mouse_toggle.isOn){
            CheckMousePosition();
        }

        //Debug.Log(previousRotations.Count);

        if ( currentMode != Mode.DORMANT && currentMode != Mode.MENU)
        {
            previousRotations.Add(latestRotation);

            //maximum of 15 minutes remebered movement
            if (previousRotations.Count >= (Application.targetFrameRate * (60 * 15)))
            {
                Debug.Log("PROBLEM - need to stop remembering rotations after a certain period");
            }
        }


        float angle = Quaternion.Angle(Quaternion.Normalize(previousRotation), Quaternion.Normalize(latestRotation));

        previousRotation = latestRotation;

        previousAngleDifferences.Add(angle);
       
        

        if (previousAngleDifferences.Count >= (Application.targetFrameRate * waitTimeSlider.value))
        {
            previousAngleDifferences.RemoveAt(0);
        }





        //calulate average change of angle
        float average = calculateAverageAngleChange();

        //Debug.Log(average);

        //determine if object is beign held or put down

        // if(currentObjectMode == ObjectMode.PUTDOWN)
        //private ObjectMode currentObjectMode = ObjectMode.PUTDOWN;
        if (average > 0.01f)
        {
            currentObjectMode = ObjectMode.INHAND;
        }
        else
        {
            currentObjectMode = ObjectMode.PUTDOWN;
        }

        switch (currentMode)
        {
            case Mode.MENU:
                transform.localRotation = latestRotation;
                break;
            case Mode.STATIONARY:

                if (currentObjectMode == ObjectMode.INHAND)
                {
                    //object is current being held
                    currentMode = Mode.MOVING;
                  
                }
                else if (currentObjectMode == ObjectMode.PUTDOWN)
                {
                    //object is currently not being held

                    
                    //play back at half speed
                    previousTime += Time.deltaTime;

                    if (previousTime > (Time.deltaTime * 2))
                    {
                        transform.localRotation = previousRotations[0];
                        previousRotations.RemoveAt(0);
                        previousTime = 0;
                    }
                }

                //animate transparency to 0
                if (currentFadeMode != FadeMode.FADEOUT)
                {
                    targetTransparency = LowestTransparencySlider.value;
                    transparencySpeed = (float)(1.0f / (FadeOutSlider.value * 30.0f));
                    currentFadeMode = FadeMode.FADEOUT;
                }

                break;

            case Mode.MOVING:

                if (currentObjectMode == ObjectMode.INHAND)
                {
                    //object is current being held
                    // transform.localRotation = latestRotation;
                }
                else if (currentObjectMode == ObjectMode.PUTDOWN)
                {
                    //object is currently not being held
                    currentMode = Mode.STATIONARY;
                }


        

                //fade the model in
                if (currentFadeMode != FadeMode.FADEIN)
                {
                    targetTransparency = 1;
                    transparencySpeed = (float)(1.0f / (FadeInSlider.value * 30.0f));
                    currentFadeMode = FadeMode.FADEIN;
                }


                //play back at half speed
                previousTime += Time.deltaTime;

                if (previousTime > (Time.deltaTime * 2))
                {
                    transform.localRotation = previousRotations[0];
                    previousRotations.RemoveAt(0);
                    previousTime = 0;
                }


                break;
            case Mode.DORMANT:
               
                if (currentObjectMode == ObjectMode.INHAND)
                {
                    //object is current being held
                    currentMode = Mode.MOVING;

                    previousTime = 0;
                    
                    previousRotations.Add(latestRotation);

                }

                 transform.localRotation = latestRotation;
                
                break;
        }
                

        if (currentTransparency != targetTransparency)
        {

            // Debug.Log(transparencySpeed);
            if (currentTransparency > targetTransparency)
            {
                currentTransparency -= transparencySpeed;
            }
            else
            {
                currentTransparency += transparencySpeed;
            }

           

            //force current to be target if close
            if (Mathf.Abs(currentTransparency - targetTransparency) < (transparencySpeed * 2))
            {
                currentTransparency = targetTransparency;
                currentFadeMode = FadeMode.NONE;


                //if lowest transparency, clear previous rotations and set to dormant mode
                if(currentTransparency == LowestTransparencySlider.value){
                    
                    previousRotations.Clear();
                    Debug.Log("reset list");
                   
                    currentMode =  Mode.DORMANT;
                }
            }

             setTransparency(currentTransparency);




        }


    }

    /**
    * 
    * Return float average angle change
    */
    private float calculateAverageAngleChange()
    {
        float total = 0;

        for (int i = 0; i < previousAngleDifferences.Count; i++)
        {
            total += previousAngleDifferences[i];
        }

        float average = total / previousAngleDifferences.Count;

        return average;
    }

    /**
    * called when the menu is opened and closed, sets the relevant 
    * mode so the user can edit the object with out a delay or any
    * transparency
    **/
    public void ToggleMenu(bool inMenu)
    {
        if (inMenu)
        {
            currentMode = Mode.MENU;
            targetTransparency = 1;
        }
        else
        {
            currentMode = Mode.STATIONARY;
            targetTransparency = LowestTransparencySlider.value;
        }
    }

    /**
    * set the transparency of the material on all the elements of the object
    */
    private void setTransparency(float t)
    {

        GameObject[] parts = GameObject.FindGameObjectsWithTag("part");

        foreach (GameObject part in parts)
        {
            Color color = part.GetComponent<MeshRenderer>().material.color;
            color.a = t;
            part.GetComponent<MeshRenderer>().material.color = color;
        }
    }



    private void CheckMousePosition()
    {   
       
        float mouseX = -Input.GetAxis("Mouse X") * 3;
        float mouseY = Input.GetAxis("Mouse Y") * 3;

        float wheel = Input.mouseScrollDelta.y * 3;

     
        Vector3 euler_angles = latestRotation.eulerAngles;
        euler_angles.x += mouseX;
        euler_angles.y += mouseY;
        euler_angles.z += wheel;

        latestRotation.eulerAngles = euler_angles;

        Debug.Log(euler_angles.x);


    }




}
