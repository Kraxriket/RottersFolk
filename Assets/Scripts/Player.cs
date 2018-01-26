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

    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 8f;
    public float slopeControl;
    public float gravity = -12f;
    public float jumpSpeed = 8.0F;
    float velocityY;
    bool onceJump = false;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public Animator animator;
    Transform cameraT;
    CharacterController characterController;

    // Use this for initialization
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
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
        Vector2 input = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));
        Vector2 inputDirection = input;

        if (inputDirection != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }
        characterController.slopeLimit = slopeControl;

        float speed = runSpeed * inputDirection.magnitude;

        velocityY += Time.deltaTime * gravity;

        float animationSpeedPercent = inputDirection.magnitude;
        animator.SetFloat("speedPercent", animationSpeedPercent);

        Vector3 velocity = transform.forward * speed + Vector3.up * velocityY;

        characterController.Move(velocity * Time.deltaTime);

        if (characterController.isGrounded)
        {
            velocityY = 0;
        }

    }

}