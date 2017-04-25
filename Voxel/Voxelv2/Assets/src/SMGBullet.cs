using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGBullet : Projectile {
    
    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        lifetime = 0.2f;
        velocity = 180;
        damage = player.damage;
        GetComponent<Rigidbody>().velocity = transform.forward.normalized * velocity;
    }

    private int explosionsize = 0;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    protected override void Explode()
    {
        player.world.DamageBlocks(transform.position + transform.forward.normalized * 0.5f, explosionsize, player.damage);
        Instantiate(Resources.Load<Transform>("SMGExplosion"), transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
