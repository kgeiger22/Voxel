using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{

    [SerializeField] protected bool m_IsWalking = false;
    [SerializeField] protected float m_WalkSpeed = 12;
    [SerializeField] protected float m_RunSpeed = 16;
    [SerializeField] [Range(0f, 1f)] protected float m_RunstepLenghten = 0.7f;
    [SerializeField] protected float m_JumpSpeed = 10;
    [SerializeField] protected float m_StickToGroundForce = 10;
    [SerializeField] protected float m_GravityMultiplier = 2.5f;
    [SerializeField] protected MouseLook m_MouseLook;
    [SerializeField] protected bool m_UseFovKick = true;
    [SerializeField] protected FOVKick m_FovKick = new FOVKick();
    [SerializeField] protected bool m_UseHeadBob = true;
    [SerializeField] protected CurveControlledBob m_HeadBob = new CurveControlledBob();
    [SerializeField] protected LerpControlledBob m_JumpBob = new LerpControlledBob();
    [SerializeField] protected float m_StepInterval = 5;
    [SerializeField] protected AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] protected AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] protected AudioClip m_LandSound;           // the sound played when character touches back on ground.
    [SerializeField] protected bool m_CanFly = false;

    protected Camera m_Camera;
    protected bool m_Jump;
    protected Vector2 m_Input;
    protected Vector3 m_MoveDir = Vector3.zero;
    protected CharacterController m_CharacterController;
    protected CollisionFlags m_CollisionFlags;
    protected bool m_PreviouslyGrounded;
    protected Vector3 m_OriginalCameraPosition;
    protected float m_StepCycle;
    protected float m_NextStep;
    protected bool m_Jumping;
    protected AudioSource m_AudioSource;
    protected int jumpstaken = 0;
    protected int maxjumps = 1;
    protected string type = "Generic";
    protected bool changing = false;





    // Use this for initialization
    protected virtual void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_FovKick.Setup(m_Camera);
        m_HeadBob.Setup(m_Camera, m_StepInterval);
        m_HeadBob.HorizontalBobRange = 0.03f;
        m_HeadBob.VerticalBobRange = 0.03f;
        m_HeadBob.VerticaltoHorizontalRatio = 2;
        m_JumpBob.BobAmount = 0.1f;
        m_JumpBob.BobDuration = 0.2f;
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_Jumping = false;
        m_AudioSource = GetComponent<AudioSource>();
        m_MouseLook.Init(transform, m_Camera.transform);
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        //CHANGE CLASS
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!type.Equals("Builder"))
            {
                Builder b = gameObject.GetComponent<Builder>();
                b.enabled = true;
                m_MouseLook.LookRotation(transform, m_Camera.transform);
                this.enabled = false;
                changing = true;
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            if (!type.Equals("Dragon"))

            {
                //gameObject.AddComponent<Dragon>().Start();
                Dragon d = gameObject.GetComponent<Dragon>();
                d.enabled = true;
                m_MouseLook.LookRotation(transform, m_Camera.transform);
                this.enabled = false;
                changing = true;

                return;

            }
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            if (!type.Equals("Scout"))
            {
                //gameObject.AddComponent<Scout>();
                Scout s = gameObject.GetComponent<Scout>();
                s.enabled = true;
                m_MouseLook.LookRotation(transform, m_Camera.transform);
                this.enabled = false;
                changing = true;

                return;

            }
        }

        RotateView();
        // the jump state needs to read here to make sure it is not missed

        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            StartCoroutine(m_JumpBob.DoBobCycle());
            PlayLandingSound();
            m_MoveDir.y = 0f;
            m_Jumping = false;
            m_Jump = false;
            jumpstaken = 0;
        }
        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;
    }


    protected void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
        m_NextStep = m_StepCycle + .5f;
    }



    protected virtual void FixedUpdate()
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


        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;


        if (m_CharacterController.isGrounded)
        {
            m_MoveDir.y = -m_StickToGroundForce;


            //Jumping from ground
            if (m_Jump)
            {
                jumpstaken += 1;
                m_MoveDir.y = m_JumpSpeed;
                PlayJumpSound();
                m_Jump = false;
                m_Jumping = true;
            }
        }
        else
        {
            //Multi-jump
            if (m_Jump && jumpstaken < maxjumps)
            {
                jumpstaken += 1;
                m_MoveDir.y = m_JumpSpeed;
                PlayJumpSound();
                m_Jump = false;
                m_Jumping = true;
            } //end
            else
            {
                //flying
                if (m_CanFly && CrossPlatformInputManager.GetButton("Jump") && m_MoveDir.y < 0.1)
                {
                    m_MoveDir.y = 0;
                }
                //Apply gravity
                else
                {
                    m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
                }
            }
        }
        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);

        m_MouseLook.UpdateCursorLock();
    }


    protected void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }


    protected void ProgressStepCycle(float speed)
    {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
        {
            m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                         Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }


    protected void PlayFootStepAudio()
    {
        if (!m_CharacterController.isGrounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        //int n = Random.Range(1, m_FootstepSounds.Length);
        //m_AudioSource.clip = m_FootstepSounds[n];
        //m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        //m_FootstepSounds[n] = m_FootstepSounds[0];
        //m_FootstepSounds[0] = m_AudioSource.clip;
    }


    protected void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;
        if (!m_UseHeadBob)
        {
            return;
        }
        if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
        {
            m_Camera.transform.localPosition =
                m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                  (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
        }
        else
        {
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
        }
        m_Camera.transform.localPosition = newCameraPosition;
    }


    protected virtual void GetInput(out float speed)
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        speed = m_WalkSpeed;
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }
    }


    protected void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }


    protected void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
}

