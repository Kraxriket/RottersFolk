using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//kNOWN ISSUES
//Joystick error causes camera to stray when at resting position
//camera collision computation snaps to player model at high speeds

public class CameraController : MonoBehaviour
{

    public Transform target;
    public float cameraSensitivity = 15f;
    public float cameraRange = 10f;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float rotationSmoothTime = 0.2f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float cameraDistance = 10f;
    float yaw;
    float pitch;

    void Update()
    {
        yaw -= CrossPlatformInputManager.GetAxis("Joystick X") * cameraSensitivity;
        pitch -= CrossPlatformInputManager.GetAxis("Joystick Y") * cameraSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = new Vector3(pitch, yaw, 0.0f);
        transform.eulerAngles = currentRotation;

        RaycastHit hit;
        if (Physics.Raycast(target.transform.position, Camera.main.transform.forward * -1, out hit, cameraRange))
        {
            if (hit.transform.tag == "Safe")
            {
                cameraDistance = cameraRange;
            }
            else
            {

                cameraDistance = hit.distance * 0.9f;
            }
        }
        
        else
        {
            cameraDistance = cameraRange;

        }




        transform.position = target.position - transform.forward * cameraDistance;
    }

}

