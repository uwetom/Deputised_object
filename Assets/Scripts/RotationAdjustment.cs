using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RotationAdjustment : MonoBehaviour
{
   
    public Slider sliderX;
    public Slider sliderY;
    public Slider sliderZ;
    public GameObject canvas;
    public GameObject rotatingObject;
    public bool mirrorMode = false;

    private bool UIVisible = true;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(UIVisible);

        /*
        if (mirrorMode)
        {
            transform.localScale = new Vector3(-1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        */
       
    }

    // Update is called once per frame
    void Update()
    {
        /*

        if (mirrorMode)
        {
            transform.localScale = new Vector3(-1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        */

        if (Input.GetKeyDown(KeyCode.Mouse0)){

            //show ui
            canvas.SetActive(true);

            UIVisible = true;

            rotatingObject.GetComponent<RotateObject>().ToggleMenu(true);

        }


        if(UIVisible){
            transform.rotation = Quaternion.Euler(sliderX.value, sliderY.value, sliderZ.value);
        }

    }

    public void CloseCanvas(){
        //show ui
        canvas.SetActive(false);

        UIVisible = false;

        rotatingObject.GetComponent<RotateObject>().ToggleMenu(false);

    }
}
