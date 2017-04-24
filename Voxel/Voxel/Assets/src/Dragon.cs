using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class Dragon : Player
{
    private float firerate = 2;
    private float cooldown = 0;
    private float shiftrate = 2f;
    private float shiftcooldown = 0;
    private float m_FlightAcc = 0;
    private bool m_Dash = false;
    private bool m_Flying = false;
    private float m_FlyingSpeed = 0;
    private bool ResetAcc = false;
    private bool righthand = true;
    Vector3 dashspeed = new Vector3(0, 0, 0);
    private List<Projectile> Bullets = new List<Projectile>();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        type = "Dragon";
        maxjumps = 1;
        m_JumpSpeed = 20;
        m_CanFly = true;
        m_GravityMultiplier = 2.75f;
    }


    //private List<Vector3> WaitingToAddBlocks = new List<Vector3>();
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (changing)
        {
            changing = false;
            return;
        }
        foreach (Projectile p in new List<Projectile>(Bullets))
        {
            if (p.IsDead == true)
            {
                Bullets.Remove(p);
                Destroy(p.gameObject);
            }
        }

        if (Input.GetMouseButton(0) && cooldown < 0)
        {
            Vector3 startpos = transform.position + new Vector3(0, 0.2f, 0);
            Projectile f = Instantiate(Resources.Load<Projectile>("Fireball"), startpos, Camera.main.transform.rotation);
            if (righthand)
            {
                f.transform.position += f.transform.right.normalized * 0.5f;
                f.transform.RotateAround(f.transform.position, f.transform.up, -0.4f);

            }
            else
            {
                f.transform.position += f.transform.right.normalized * -0.5f;
                f.transform.RotateAround(f.transform.position, f.transform.up, 0.4f);
            }
            righthand = !righthand;
            Bullets.Add(f);
            cooldown = 1.0f / firerate;
        }
        else cooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && shiftcooldown < 0)
        {
            m_Dash = true;
            shiftcooldown = shiftrate;
        }
        else shiftcooldown -= Time.deltaTime;

    }

    protected override void FixedUpdate()
    {
        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;



        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                           m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        if (m_Dash)
        {
            if (m_CharacterController.isGrounded) dashspeed.y = m_JumpSpeed;
            else
            {
                dashspeed.x = desiredMove.x * m_JumpSpeed * 0.13f;
                dashspeed.z = desiredMove.z * m_JumpSpeed * 0.13f;
            }
            
            m_Flying = false;
        }
        else if (dashspeed.magnitude < 0.1f) dashspeed = new Vector3(0, 0, 0);
        else dashspeed *= 0.98f;

        m_MoveDir.x = desiredMove.x * speed + dashspeed.x * m_WalkSpeed;
        if (Mathf.Abs(m_MoveDir.x) < Mathf.Abs(dashspeed.x * m_WalkSpeed)) dashspeed.x = m_MoveDir.x / m_WalkSpeed;
        m_MoveDir.z = desiredMove.z * speed + dashspeed.z * m_WalkSpeed;
        if (Mathf.Abs(m_MoveDir.z) < Mathf.Abs(dashspeed.z * m_WalkSpeed)) dashspeed.z = m_MoveDir.z / m_WalkSpeed;



        if (m_CharacterController.isGrounded)
        {
            dashspeed *= 0.7f;
            ResetAcc = false;
            m_FlightAcc = 0;
            m_MoveDir.y = -m_StickToGroundForce;
            m_Flying = false;


            //Jumping from ground
            if (m_Jump)
            {
                jumpstaken += 1;
                m_MoveDir.y = m_JumpSpeed;
                PlayJumpSound();
                m_Jump = false;
                m_Jumping = true;
            }
            if (m_Dash)
            {
                m_MoveDir.y = 1.5f * m_JumpSpeed;
                PlayJumpSound();
                m_Jumping = true;
            }
        }
        else
        {

            if (m_Jump && jumpstaken < maxjumps)
            {
                jumpstaken += 1;
                m_MoveDir.y = m_JumpSpeed;
                PlayJumpSound();
                m_Jump = false;
                m_Jumping = true;
            } //end
            else if (m_Dash && m_Input.magnitude < 0.1)
            {
                m_MoveDir.y = m_JumpSpeed * 1.5f;
                PlayJumpSound();
                m_Jump = false;
                m_Jumping = true;
            } //end
            else
            {
                //flying
                if (CrossPlatformInputManager.GetButton("Jump") && !m_Jumping)
                {
                    jumpstaken = 1;
                    if (m_MoveDir.y > 0 && !ResetAcc)
                    {
                        m_FlightAcc = 0;
                        ResetAcc = true;
                    }
                    else if (m_MoveDir.y > 0) m_FlightAcc += 1.3f * Time.fixedDeltaTime;
                    else m_FlightAcc += 2.0f * Time.fixedDeltaTime;
                    m_MoveDir.y += m_FlightAcc;
                    m_MoveDir.y = Mathf.Min(m_MoveDir.y, 2);
                    m_FlyingSpeed -= m_FlightAcc;
                    m_FlyingSpeed = Mathf.Max(m_FlyingSpeed, 6);
                    m_Flying = true;

                }
                //Apply gravity
                else
                {
                    ResetAcc = false;
                    m_FlightAcc = 0;
                    m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
                    if (m_MoveDir.y < 0) m_Jumping = false;
                }
            }
            //if (m_Dash && dashspeed.magnitude < 0.1f) m_MoveDir.y += 1 * m_JumpSpeed;
        }

        m_Dash = false;


        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);

        m_MouseLook.UpdateCursorLock();
    }

    protected override void GetInput(out float speed)
    {
        base.GetInput(out speed);
        if (m_Flying) speed = m_FlyingSpeed;
    }
}

