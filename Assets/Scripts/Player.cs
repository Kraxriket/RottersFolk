using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//TODO
//Keep player character from sliding or floating down angular terrain due to gravity. Some research required.
//camera collision and 
//Set joystick float epsilon for precise controls

public class Player : MonoBehaviour {

    [SerializeField] float Speed = 10f;

    float xThrow, zThrow;
    public Animator animator;
    Transform cameraT;

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

        transform.Translate(Quaternion.Euler(0, cameraT.eulerAngles.y, 0) * movement * Speed * Time.deltaTime, Space.World);

        if (movement != Vector3.zero)
        {
            Vector3 cameraRelation = (Quaternion.Euler(0, cameraT.eulerAngles.y, 0) * movement);
            transform.rotation = Quaternion.LookRotation(cameraRelation);
        }

        float animationSpeedPercent = movement.magnitude;
        animator.SetFloat("speedPercent", animationSpeedPercent);
   
    }

}

