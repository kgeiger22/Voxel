using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akimbo : Weapon {

    private bool righthand = true;

    // Use this for initialization
    void Start () {
        type = WeaponType.Akimbo;
        damage = 50;
        firerate = 20;
        explosionsize = 0;
    }


    protected override void ShootPrimary()
    {
        Vector3 startpos = transform.position + new Vector3(0, 0.3f, 0);
        AkimboBullet bullet = Instantiate(Resources.Load<AkimboBullet>("Akimbo/AkimboBullet"), startpos, Camera.main.transform.rotation);
        bullet.LoadFromWeapon(this);
        if (righthand)
        {
            bullet.transform.position += bullet.transform.right.normalized * 0.4f;
            bullet.transform.RotateAround(bullet.transform.position, bullet.transform.up, -0.7f);
        }
        else
        {
            bullet.transform.position += bullet.transform.right.normalized * -0.4f;
            bullet.transform.RotateAround(bullet.transform.position, bullet.transform.up, 0.7f);
        }
        bullet.transform.Rotate(-1, 0, 0);
        righthand = !righthand;
    }

    protected override void ShootSecondary()
    {
        
    }
}
