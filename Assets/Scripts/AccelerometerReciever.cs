using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class AccelerometerReciever : MonoBehaviour
{
    public OSCReceiver _receiver;
    public GameObject rotationObject;
    public float xRotVal = 0;
    public float yRotVal = 0;
    public float zRotVal = 0;
    public float wRotVal = 0;

    void Start()
    {
        _receiver.Bind("/pos/", MessageReceived);   

        _receiver.Bind("/posEuler/", EulerMessageReceived);  

        Application.targetFrameRate = 30;
    }

    protected void MessageReceived(OSCMessage message)
    {
        xRotVal = message.Values[0].FloatValue;
        yRotVal = message.Values[1].FloatValue;
        zRotVal = message.Values[2].FloatValue;
        wRotVal = message.Values[3].FloatValue;

        Quaternion newRotation = new Quaternion(-xRotVal,-zRotVal,-yRotVal,wRotVal);


        //rotationObject.GetComponent<RotateObject>().transform.localRotation= newRotation;

        rotationObject.GetComponent<RotateObject>().Rotate(newRotation);
       
    }

    protected void EulerMessageReceived(OSCMessage message){
        
        Debug.Log(message.Values[1].FloatValue);

        xRotVal = message.Values[0].FloatValue;
        yRotVal = message.Values[1].FloatValue;
        zRotVal = message.Values[2].FloatValue;

        Quaternion newRotation = Quaternion.Euler(xRotVal,yRotVal,zRotVal);
        
        //Quaternion newRotation = Quaternion.Euler(xRotVal,zRotVal,yRotVal);
        //Quaternion newRotation = Quaternion.Euler(yRotVal,zRotVal,xRotVal);
       // Quaternion newRotation = Quaternion.Euler(yRotVal,xRotVal,zRotVal);
        //Quaternion newRotation = Quaternion.Euler(zRotVal,yRotVal,xRotVal);
       // Quaternion newRotation = Quaternion.Euler(zRotVal,xRotVal,yRotVal);
    
        // Quaternion newRotation = Quaternion.Euler(yRotVal,0,0);
       


        rotationObject.GetComponent<RotateObject>().Rotate(newRotation);
       

    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape))
		Application.Quit();
    }

}
