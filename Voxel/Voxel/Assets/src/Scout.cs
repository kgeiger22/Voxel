using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class Scout : Player
{
    private float firerate = 10;
    private float cooldown = 0;
    private bool righthand = true;
    private List<Projectile> Bullets = new List<Projectile>();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        type = "Scout";
        maxjumps = 2;
        m_WalkSpeed = 14;
        m_RunSpeed = 22;
        m_JumpBob.BobAmount = 0.05f;
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
            Vector3 startpos = transform.position + new Vector3(0, 0.3f, 0);
            Projectile p = Instantiate(Resources.Load<Projectile>("SMGBullet"), startpos, Camera.main.transform.rotation);

            if (righthand)
            {
                p.transform.position += p.transform.right.normalized * 0.4f;
                p.transform.RotateAround(p.transform.position, p.transform.up, -0.7f);

            }
            else
            {
                p.transform.position += p.transform.right.normalized * -0.4f;
                p.transform.RotateAround(p.transform.position, p.transform.up, 0.7f);
            }
            p.transform.Rotate(-1, 0, 0);
            righthand = !righthand;
            Bullets.Add(p);
            cooldown = 1.0f / firerate;
        }
        else cooldown -= Time.deltaTime;
        
    }

    protected override void GetInput(out float speed)
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
}
