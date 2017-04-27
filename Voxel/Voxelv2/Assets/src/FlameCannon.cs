using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameCannon : Weapon
{

    private bool righthand = true;

    // Use this for initialization
    void Start()
    {
        type = WeaponType.FlameCannon;
        damage = 200;
        firerate = 2.0f;
        explosionsize = 1;
    }

    protected override void ShootPrimary()
    {
        Vector3 startpos = transform.position + new Vector3(0, 0.3f, 0);
        Fireball fireball = Instantiate(Resources.Load<Fireball>("Fireball/Fireball"), startpos, Camera.main.transform.rotation);
        fireball.LoadFromWeapon(this);
        if (righthand)
        {
            fireball.transform.position += fireball.transform.right.normalized * 1.0f;
            fireball.transform.RotateAround(fireball.transform.position, fireball.transform.up, -0.6f);

        }
        else
        {
            fireball.transform.position += fireball.transform.right.normalized * -1.0f;
            fireball.transform.RotateAround(fireball.transform.position, fireball.transform.up, 0.6f);
        }
        righthand = !righthand;
    }

    protected override void ShootSecondary()
    {
        Vector3 startpos = transform.position + new Vector3(0, 0.3f, 0);
        Fireball fireball = Instantiate(Resources.Load<Fireball>("Fireball/Fireball"), startpos, Camera.main.transform.rotation);
        fireball.LoadFromWeapon(this);
        charge = 0;
    }



}
