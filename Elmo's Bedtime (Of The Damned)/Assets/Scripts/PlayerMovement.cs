using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // !!!!VARIABLES!!!!

    Rigidbody rb;

    [Header("Basic Movement")]
    // Movement + Player Configuration
    public float pf_moveSpd;
    public float pf_drag;
    public Transform pt_orientation;
    float f_horizontalInput;
    float f_verticalInput;
    Vector3 v3_moveDirection;

    [Header("Jumping")]
    public float pf_jumpForce;
    public float pf_jumpCooldown;
    public float pf_airMultiplier;
    bool b_jumpReady;

    [Header("Ground Check")]
    public Transform pT_groundCheck;
    public float pf_groundDistance = 0.4f;
    public LayerMask pLM_groundLayer;
    public bool pb_isGrounded;

    //KeyBinds
    KeyCode inp_jumpKey = KeyCode.Space;

    // !!!!START COMPONENT!!!!

    private void Start()
    {
        // Set player ready to jump.
        b_jumpReady = true;

        // Configure player rigid body
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // !!!!UPDATE COMPONENT!!!!
    void Update()
    {
        // Obtain constants 
        MyInput(); // Keyboard Inputs
        SpeedControl();

        // Check if player is on the ground
        pb_isGrounded = Physics.CheckSphere(pT_groundCheck.position, pf_groundDistance, pLM_groundLayer);
        
        // Apply player drag physics
        if (pb_isGrounded)
        {
            rb.drag = pf_drag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        // Move Keys 
        f_horizontalInput = Input.GetAxisRaw("Horizontal");
        f_verticalInput = Input.GetAxisRaw("Vertical");
        
        // Jump
        if(Input.GetKey(inp_jumpKey) && b_jumpReady && pb_isGrounded)
        {
            b_jumpReady = false;
            Jump();
            Invoke(nameof(ResetJump), pf_jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        v3_moveDirection = pt_orientation.forward * f_verticalInput + pt_orientation.right * f_horizontalInput;

        // on Ground
        if(pb_isGrounded)
        {
            rb.AddForce(v3_moveDirection.normalized * pf_moveSpd * 10f, ForceMode.Force);
        }
        // on Air
        else if(!pb_isGrounded)
        {
            rb.AddForce(v3_moveDirection.normalized * pf_moveSpd * 10f * pf_airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if needed
        if(flatVel.magnitude > pf_moveSpd)
        {
            Vector3 limitedVel = flatVel.normalized * pf_moveSpd;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * pf_jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        b_jumpReady = true;
    }
}
