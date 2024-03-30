using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingMechanics : MonoBehaviour
{
    // !!!!VARIABLES!!!!
    [Header("Player Reference")]
    public Transform pT_orientation;
    public Rigidbody rb;
    public PlayerMovement pm;
    public LayerMask pLM_wallLayer;

    [Header("Climbing")]
    public float pf_climbSpd;
    public bool b_isClimbing;
    float f_horizontalInput;
    float f_verticalInput;
    //Stamina implementation?

    [Header("Triggers")]
    public float pf_detectionLength;
    public float pf_sphereRadius;
    private RaycastHit RH_wallHit;
    private bool b_frontWall;

    // Update is called once per frame
    void Update()
    {
        MyInput();
        WallCheck();
        StateMachine();

        if (b_isClimbing) Climbing();
    }

    void MyInput()
    {
        // Move Keys 
        f_horizontalInput = Input.GetAxisRaw("Horizontal");
        f_verticalInput = Input.GetAxisRaw("Vertical");
    }

    void StateMachine()
    {
        // Climbing
        if(Input.GetKey("space") && b_frontWall && f_verticalInput != 0)
        {
            if (!b_isClimbing) StartClimbing();

        }

        // Not Climbing
        else
        {
            if (b_isClimbing) StopClimbing();
        }
    }

    void WallCheck()
    {
        b_frontWall = Physics.SphereCast(transform.position, pf_sphereRadius, pT_orientation.forward, out RH_wallHit, pf_detectionLength, pLM_wallLayer);
        
    }

    void StartClimbing()
    {
        b_isClimbing = true;
    }

    void Climbing()
    {
        rb.velocity = new Vector3(rb.velocity.x, pf_climbSpd, rb.velocity.z);
    }

    void StopClimbing()
    {
        b_isClimbing = false;
    }


}
