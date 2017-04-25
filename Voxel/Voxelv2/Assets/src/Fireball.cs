using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile {
    
	// Use this for initialization
	override protected void Start ()
    {
        base.Start();
        lifetime = 8;
        velocity = 100;
        transform.Rotate(-5, 0, 0);
        damage = player.damage;
        GetComponent<Rigidbody>().velocity = transform.forward.normalized * velocity;
    }

    private int explosionsize = 1;

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(Physics.gravity * 3);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    protected override void Explode()
    {
        player.world.DamageBlocks(transform.position + transform.forward.normalized * 0.5f, explosionsize, damage);
        Instantiate(Resources.Load<Transform>("FireballExplosion"), transform.position, Quaternion.identity);
        Transform PE = transform.FindChild("FireTrail");
        if (PE != null)
        {
            PE.GetComponent<Explosion>().enabled = true;
            PE.GetComponent<ParticleSystem>().Stop();
            PE.transform.parent = null;
        }
        Destroy(gameObject);
    }
}
