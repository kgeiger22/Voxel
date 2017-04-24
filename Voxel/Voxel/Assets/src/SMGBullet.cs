using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGBullet : Projectile {
    
    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        lifetime = 0.2f;
        velocity = 120;
    }

    private bool alreadyhit = false;
    private int explosionsize = -1;
    override protected void Update()
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
                Instantiate<Transform>(Resources.Load<Transform>("SMGExplosion"), transform.position, Quaternion.identity);
                IsDead = true;
            }
        }
        transform.position += transform.forward.normalized * (velocity / 100);
    }

}
