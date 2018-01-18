using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerOLDER : MonoBehaviour {

    public float velocity = 8;
    public float turnSpeed = 4;
    public float height = 0.5f;
    public float heightPadding = 0.05f;
    public LayerMask ground;
    public float maxGroundAngle = 120;
    public bool debug;
    

    Vector2 input;
    float angle;
    float groundAngle;
    public Animator animator;
    public Rigidbody playerRigidbody;

    Quaternion targetRotation;
    Transform cam;

    Vector3 forward;
    RaycastHit hitInfo;
    bool grounded;

    // Use this for initialization
    void Start () {
        cam = Camera.main.transform;
        animator = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.useGravity = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        GetInput();
        CalculateDirection();
        CalculateForward();
        CalculateGroundAngle();
        CheckGround();
        DrawDebugLines();

        if (Mathf.Abs(input.x) < 0.1f && Mathf.Abs(input.y) < 0.1f) return;


        rotate();
        move();
    }



    private void CalculateForward()
    {
        if (!grounded)
        {
            forward = transform.forward;
            return;
        }

        forward = Vector3.Cross(transform.right, hitInfo.normal);
    }

    private void CalculateGroundAngle()
    {
        if (!grounded)
        {
            groundAngle = 90;
        }

        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);

    }

    private void CheckGround()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo, height + heightPadding, ground))
        {
            grounded = true;
        }

        else
        {
            grounded = false;
        }
    }

    private void DrawDebugLines()
    {
        if (!debug) return;
        Debug.DrawLine(transform.position, transform.position + forward * height * 2, Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
    }

    private void GetInput()
    {
        input.x = CrossPlatformInputManager.GetAxis("Horizontal");
        input.y = CrossPlatformInputManager.GetAxis("Vertical");
        MovementAnimationControl();
    }

    private void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
    }

    private void MovementAnimationControl()
    {
        Vector3 movement = new Vector3(input.x, 0.0f, input.y);
        float animationSpeedPercent = movement.magnitude;
        animator.SetFloat("speedPercent", animationSpeedPercent);
    }

    private void move()
    {
        if (groundAngle >= maxGroundAngle) return;
        Vector3 movement = new Vector3(input.x, 0.0f, input.y);

        transform.Translate(Quaternion.Euler(0, cam.eulerAngles.y, 0) * movement * velocity * Time.deltaTime, Space.World);
    }

    private void rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }


}
