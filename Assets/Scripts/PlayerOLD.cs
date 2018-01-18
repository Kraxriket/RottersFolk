using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//TODO
//Keep player character from sliding or floating down angular terrain due to gravity. Some research required.
//camera collision and 
//Set joystick float epsilon for precise controls

public class PlayerOLD : MonoBehaviour {

    [SerializeField] float Speed = 10f;
    public float turnSpeed = 1;

    float xThrow, zThrow;
    public Animator animator;
    Transform cameraT;
    public float height = 0.5f;
    public float heightPadding = 0.05f;
    public LayerMask ground;
    public float maxGroundAngle = 120;
    float angle;

    Quaternion targetRotation;

    Vector3 forward;
    RaycastHit hitInfo;
    bool grounded;

    // Use this for initialization
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        cameraT = Camera.main.transform;
        Physics.gravity = new Vector3(0, -40, 0);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();


    }

    

    void ProcessTranslation()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        zThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 movement = new Vector3(xThrow, 0.0f, zThrow);

        transform.Translate(movement * Speed * Time.deltaTime, Space.World);

        if (movement != Vector3.zero)
        {
            angle = Mathf.Atan2(xThrow, zThrow);
            angle = Mathf.Rad2Deg * angle;
            angle += cameraT.eulerAngles.y;
        }

        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        float animationSpeedPercent = movement.magnitude;
        animator.SetFloat("speedPercent", animationSpeedPercent);

    }

}

