using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using System;

public class Scout : Player
{
    private bool IsWallrunning = false;
    

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        type = "Scout";
        maxjumps = 2;
        m_WalkSpeed = 14;
        m_RunSpeed = 22;
        m_JumpBob.BobAmount = 0.05f;
        LoadWeapon();
    }


    //private List<Vector3> WaitingToAddBlocks = new List<Vector3>();
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (m_CharacterController.collisionFlags != CollisionFlags.Sides || m_CharacterController.isGrounded || 
            m_IsWalking || m_CharacterController.velocity.magnitude < 10) IsWallrunning = false;

        if (IsFiring)
        {
            weapon.TriggerPrimaryFire();
        }
        float speed;
        GetMovementInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                           m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;


        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;


        if (m_CharacterController.isGrounded)
        {
            m_MoveDir.y = -m_StickToGroundForce;


            //Jumping from ground
            if (m_Jump)
            {
                Jump();
            }
        }
        else
        {
            //Multi-jump
            if (m_Jump && jumpstaken < maxjumps)
            {
                Jump();
            } //end
            else if (IsWallrunning && !m_Jumping)
            {
                m_MoveDir.y = 1;
                jumpstaken = 1;
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
                if (m_MoveDir.y < 0) m_Jumping = false;
            }
            
        }
        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);

        m_MouseLook.UpdateCursorLock();
    }

    protected override void GetMovementInput(out float speed)
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        bool waswalking = m_IsWalking;
        m_IsWalking = !Input.GetKey(KeyCode.LeftShift);

        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
        // set the desired speed to be walking or running
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }

        // handle speed change to give an fov kick
        // only if the player is going to a run, is running and the fovkick is to be used
        if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
            StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
        }
    }
    protected override void GetAbilityInput()
    {
        base.GetAbilityInput();
        if (Input.GetMouseButton(0))
        {
            IsFiring = true;
        }
        else IsFiring = false;
    }

    protected override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        base.OnControllerColliderHit(hit);
        if (enabled && m_CharacterController.collisionFlags == CollisionFlags.Sides && !m_CharacterController.isGrounded 
            && !m_IsWalking && m_CharacterController.velocity.magnitude > 10)
        {
            IsWallrunning = true;
        }
    }

    protected override void LoadWeapon()
    {
        weapon = Instantiate(Resources.Load<Weapon>("Weapons/Akimbo"), transform.position, Camera.main.transform.rotation);
        weapon.transform.parent = this.transform;
    }
}
