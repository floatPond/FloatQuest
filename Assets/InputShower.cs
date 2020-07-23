using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BNG;
using TMPro;

public class InputShower : MonoBehaviour
{
    public GameObject Player;
    public GameObject cube;
    Vector3 CraftSpawnP;
    Quaternion CraftSpawnR;
    Quaternion CubeSpawnR;

    InputBridge ib;
    public TextMeshProUGUI LCP, LCR, RCP, RCR, cSpeed, cRotation;
    bool isJoystickEnabled;

    //Controller angles
    float degreePitch, degreeRoll, degreeYaw,degreeAcceleration;

    //Ship

    public GameObject craft;
    public float currentSpeed;

    void Start()
    {
        ib = Player.GetComponent<InputBridge>();
        CraftSpawnP = craft.transform.position;
        CraftSpawnR = craft.transform.rotation;
        CubeSpawnR = cube.transform.rotation;

        //Rotations - default is when controller is facing forward, as per normal holding
        //w is ?, defaults at 1
        //x is
        //y is ?, default is 0. Move controller left=+. Move controller right = -
        //z is roll, defaults at 0
    }

    // Update is called once per frame
    void Update()
    {
        //Get pitch
        degreePitch = ib.LeftControllerRotation.x;
        //Get roll
        degreeRoll = ib.LeftControllerRotation.z;
        //Get yaw
        //Not currently used

        //Get acceleration
        degreeAcceleration = ib.RightControllerRotation.x;

        LCP.text = "LCP-x:" + ib.LeftControllerPosition.x.ToString("F2")+",y:"+ib.LeftControllerPosition.y.ToString("F2") + ",z:" + ib.LeftControllerPosition.z.ToString("F2");
        RCP.text = "RCP-x:" + ib.RightControllerPosition.x.ToString("F2") + ",y:" + ib.RightControllerPosition.y.ToString("F2") + ",z:" + ib.RightControllerPosition.z.ToString("F2");

        LCR.text = "LCR-w:" + ib.LeftControllerRotation.w.ToString("F2") + ",x:" + ib.LeftControllerRotation.x.ToString("F2") + ",y:" + ib.LeftControllerRotation.y.ToString("F2") + ",z:" + ib.LeftControllerRotation.z.ToString("F2");
        RCR.text = "RCR-w:" + ib.RightControllerRotation.w.ToString("F2") + ",x:" + ib.RightControllerRotation.x.ToString("F2") + ",y:" + ib.RightControllerRotation.y.ToString("F2") + ",z:" + ib.RightControllerRotation.z.ToString("F2");
        cRotation.text = "Pitch:" + (degreePitch).ToString("F2") + ",Roll:" + (degreeRoll).ToString("F2");
        cSpeed.text = "Speed: " + (currentSpeed).ToString("F2");

        
        if (!isJoystickEnabled)
        {
            //Pitch craft //Vector3.left pitches craft
            if ((degreePitch > 0.05) || (degreePitch < -0.05))
                craft.transform.Rotate(Vector3.left * degreePitch * 100f * Time.deltaTime, Space.Self);
            //Roll craft
            if ((degreeRoll > 0.08) || (degreeRoll < -0.08))
                craft.transform.Rotate(Vector3.back * degreeRoll * 25f *Time.deltaTime, Space.Self);
            
            //Accelerate
            if ((degreeAcceleration > 0.05)|| (degreeAcceleration < -0.05))
            { 
                if (degreeAcceleration > 0)
                { 
                currentSpeed += degreeAcceleration * Time.deltaTime;
                }
                else //Breaking
                {
                    currentSpeed += degreeAcceleration * 2 * Time.deltaTime;
                }
            }
            //Speed
            if (currentSpeed < 0)
                   currentSpeed = 0;
               if (currentSpeed > 30)
                   currentSpeed = 30;
                //craft.transform.Translate(Vector3.forward * (degreeAcceleration) * 10f * Time.deltaTime);
            

            //Cube
            //cube.transform.Rotate(Vector3.left * ib.LeftControllerRotation.x * 100f * Time.deltaTime, Space.Self);
            //cube.transform.Rotate(Vector3.down * ib.LeftControllerRotation.y * 100f * Time.deltaTime, Space.Self);
            //cube.transform.Rotate(Vector3.back * ib.LeftControllerRotation.z * 100f * Time.deltaTime, Space.Self);

        }
        else
        {
            craft.transform.Rotate(Vector3.back * ib.LeftThumbstickAxis.x * 100 * Time.deltaTime, Space.Self);
            craft.transform.Rotate(Vector3.right * ib.LeftThumbstickAxis.y * 100f * Time.deltaTime, Space.Self);
            craft.transform.Translate(Vector3.forward * (ib.RightThumbstickAxis.y) * 10f * Time.deltaTime);
        }
        if (ib.AButtonDown == true)
            ResetCraftPosition();
        if (ib.BButtonDown == true)
            isJoystickEnabled = !isJoystickEnabled;
        craft.transform.Translate(Vector3.forward * (currentSpeed) * Time.deltaTime);
    }
    void ResetCraftPosition()
    {
        craft.transform.position = CraftSpawnP;
        craft.transform.rotation = CraftSpawnR;
        cube.transform.rotation = CubeSpawnR;
        currentSpeed = 0;
    }
}
