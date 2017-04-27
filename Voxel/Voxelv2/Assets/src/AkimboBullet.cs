using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkimboBullet : Projectile {
    
    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        lifetime = 0.2f;
        velocity = 180;
        explosionsize = 0;
        //damage = player.damage;
        GetComponent<Rigidbody>().velocity = transform.forward.normalized * velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    protected override void Explode()
    {
        World.WorldInstance.DamageBlocks(transform.position + transform.forward.normalized * 0.5f, explosionsize, damage);
        Instantiate(Resources.Load<Transform>("Akimbo/AkimboExplosion"), transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void LoadFromWeapon(Akimbo weapon)
    {
        damage = weapon.GetDamage();
        explosionsize = weapon.GetExplosionSize();
    }
}
