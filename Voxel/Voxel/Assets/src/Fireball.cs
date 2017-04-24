using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile {
    
	// Use this for initialization
	override protected void Start ()
    {
        base.Start();
        lifetime = 8;
        velocity = 60;
        transform.Rotate(-7, 0, 0);
    }

    private bool alreadyhit = false;
    private float fallingacceleration = -0.14f;
    private bool DoneRotating = false;
    private int explosionsize = -1;
    override protected void Update ()
    {
        base.Update();
        if (!alreadyhit)
        {
            Vector3 testposition = transform.position + new Vector3(0.5f, 0.5f, 0.5f);
            bool IsHit = World._instance.BlockCollision(testposition);

            if (IsHit)
            {

                alreadyhit = true;
                Vector3 newpos = new Vector3(Mathf.FloorToInt(testposition.x), Mathf.FloorToInt(testposition.y), Mathf.FloorToInt(testposition.z));
                for (int x = -explosionsize; x <= explosionsize; ++x)
                {
                    for (int y = -explosionsize; y <= explosionsize; ++y)
                    {
                        for (int z = -explosionsize; z <= explosionsize; ++z)
                        {
                            Vector3 deltapos = new Vector3(x, y, z);
                            if (deltapos.magnitude < explosionsize + 0.5f)
                            {
                                Vector3 DeleteBlockPos = newpos + deltapos;
                                MathHelper.AddBlock(DeleteBlockPos, Block.Air);
                            }
                        }
                    }
                }
                Instantiate<Transform>(Resources.Load<Transform>("FireballExplosion"), transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        if (!DoneRotating)
        {
            fallingacceleration *= 1 + 1.2f * Time.deltaTime;
            transform.Rotate(new Vector3(-fallingacceleration, 0, 0));
            
            if (transform.rotation.eulerAngles.x > 80 && transform.rotation.eulerAngles.x < 100)
            {
                DoneRotating = true;

            }
        }
        transform.position += transform.forward.normalized * (velocity / 100);
    }

}
