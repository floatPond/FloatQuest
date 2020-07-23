using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using TMPro;
public class ControlledShip : MonoBehaviour
{
    //Text
    public TextMeshProUGUI textSpeed,textPitch,textRoll,X,Z,d2g;

    //For input
    public GameObject Player;
    InputBridge ib;

    //Controls
    float degreePitch, degreeRoll, degreeYaw, degreeAcceleration,degreeX,degreeZ;
    bool lockedPitch, lockedRoll, lockedAcceleration;
    //Ship
    public float currentSpeed;
    public float maximumSpeed = 100;
    public float distanceToGround;
    Vector3 CraftSpawnP;
    Quaternion CraftSpawnR;

    void Start()
    {
        ib = Player.GetComponent<InputBridge>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ray
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 10000))
        {
            //Debug.Log("Hit "+hitInfo.transform.gameObject.name+","+Vector3.Distance(ray.origin, hitInfo.point));
            distanceToGround = Vector3.Distance(ray.origin, hitInfo.point);
            d2g.text = distanceToGround.ToString("F2");
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red,2.0f);
        }
        else
        {
            Debug.Log("Miss!");
            //Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green, 2.0f);
        }

        //Locking
        if (ib.LeftGrip > 0.5)
        {
            lockedPitch = true;
            lockedRoll = true;
        }
        else
        {
            lockedPitch = false;
            lockedRoll = false;
        }
        if (ib.RightGrip > 0.5)
        {
            lockedAcceleration = true;
        }
        else
        {
            lockedAcceleration = false;
        }

        //Calculating rotation and speed
        //Get pitch
        if (!lockedPitch)
        { 
            degreePitch = ib.LeftControllerRotation.x;
            degreePitch = Deadzone(degreePitch, 0.08f);
        }
        textPitch.text = "Pitch: " + degreePitch.ToString("F2");

        //Pitch text colour
        if (ib.LeftControllerRotation.x > 0.05)
        {
            textPitch.color = Color.green;
        }
        else if (ib.LeftControllerRotation.x < 0.05)
        {
            textPitch.color = Color.red;
        }
        else
        {
            textPitch.color = Color.white;
        }

        //Get roll
        if (!lockedRoll)
        { 
            degreeRoll = ib.LeftControllerRotation.z;
            degreeRoll = Deadzone(degreeRoll, 0.08f);
        }
        textRoll.text = "Roll: " + degreeRoll.ToString("F2");
        //Get acceleration
        degreeAcceleration = ib.RightControllerRotation.x;
        degreeAcceleration = Deadzone(degreeAcceleration, 0.1f);

        
        transform.Rotate(Vector3.left * degreePitch * 33f * Time.deltaTime, Space.Self); //Pitch pp and down
        transform.Rotate(Vector3.back * degreeRoll * 100f * Time.deltaTime, Space.Self); //Roll left and right

        //Accelerate
        if (!lockedAcceleration)
        { 
            if (degreeAcceleration > 0)
            {
                currentSpeed += degreeAcceleration * 5 * Time.deltaTime;
            }
            else //Breaking
            {
                currentSpeed += degreeAcceleration * 10 * Time.deltaTime;
            }
        }
        
        //Speed
        if (currentSpeed < 0)
            currentSpeed = 0;
        if (currentSpeed > maximumSpeed)
            currentSpeed = maximumSpeed;

        //Buttons
        if (ib.AButtonDown == true)
            ResetShip();

        //Update speed
        transform.Translate(Vector3.forward * (currentSpeed) * Time.deltaTime);
        textSpeed.text = "Speed: " + currentSpeed.ToString("F1");

        //Update X and Z text
        degreeX = transform.eulerAngles.x;
        if (degreeX > 270)
        {
            degreeX = degreeX - 360;
            degreeX *= -1;
        }
        degreeZ = transform.eulerAngles.z;
        if (degreeZ > 270)
        {
            degreeZ = degreeZ - 360;
            degreeZ *= -1;
        }

        X.text = "X:" + degreeX.ToString("F2");
        Z.text = "Z:" + degreeZ.ToString("F2");

    }

    void ResetShip()
    {
        transform.position = CraftSpawnP;
        transform.rotation = CraftSpawnR;
        currentSpeed = 0;
    }
    float Deadzone (float number, float extent)
    {
        if (number > extent) //e.g 0.6 > 0.5
        {
            number -= extent;
            return number;
        }
        else if (number < (extent *= -1))
        {
            number += extent;
            return number;
        }
        else
        {
            number = 0;
            return number;
        }
    }
}
