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
        //recive quaternion rotation
        _receiver.Bind("/pos/", MessageReceived);   

        // recieve euler rotation (for arduiono test, hopefully not needed)
        _receiver.Bind("/posEuler/", EulerMessageReceived);  

        Application.targetFrameRate = 50;
    }

    protected void MessageReceived(OSCMessage message)
    {
        xRotVal = message.Values[0].FloatValue;
        yRotVal = message.Values[1].FloatValue;
        zRotVal = message.Values[2].FloatValue;
        wRotVal = message.Values[3].FloatValue;

        Quaternion newRotation = new Quaternion(-xRotVal,-zRotVal,-yRotVal,wRotVal);
        
        rotationObject.GetComponent<RotateObject>().Rotate(newRotation);
       
    }

    protected void EulerMessageReceived(OSCMessage message)
    {
        xRotVal = message.Values[0].FloatValue;
        yRotVal = message.Values[1].FloatValue;
        zRotVal = message.Values[2].FloatValue;

        Quaternion newRotation = Quaternion.Euler(xRotVal,yRotVal,zRotVal);

        rotationObject.GetComponent<RotateObject>().Rotate(newRotation);
    }

    /**
    * Press excape to quit the application
    */
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape))
		Application.Quit();
    }

}
