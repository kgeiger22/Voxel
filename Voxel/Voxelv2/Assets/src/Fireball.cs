using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{

    public float charge = 0;
    private float chargeTime = 0.5f;
    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        transform.Rotate(-5, 0, 0);
        GetComponent<Rigidbody>().velocity = transform.forward.normalized * velocity;
    }

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
        World.WorldInstance.DamageBlocks(transform.position + transform.forward.normalized * 0.5f, explosionsize, damage);
        Transform explosion = Instantiate(Resources.Load<Transform>("Fireball/FireballExplosion"), transform.position, Quaternion.identity);
        explosion.localScale *= explosionsize;
        Transform PE = transform.FindChild("FireTrail");
        if (PE != null)
        {
            PE.GetComponent<Explosion>().enabled = true;
            PE.GetComponent<ParticleSystem>().Stop();
            PE.transform.parent = null;
        }
        Destroy(gameObject);
    }

    public void LoadFromWeapon(FlameCannon weapon)
    {
        charge = weapon.GetCharge();
        int chargemultiplier = Mathf.Min(Mathf.FloorToInt(charge / chargeTime), 2) + 1;
        damage = (int)(weapon.GetDamage() * (1 + (chargemultiplier - 1) / 2.0f));
        explosionsize = weapon.GetExplosionSize() * chargemultiplier;
        lifetime = 6 + 2 * chargemultiplier;
        velocity = velocity = 80 + 20 * chargemultiplier;
        transform.localScale *= chargemultiplier;
        transform.GetChild(0).localScale *= chargemultiplier;
    }
}
