﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {

    public float lifetime { get; protected set; }
    public float velocity { get; protected set; }
    public int explosionsize = -1;
    public int damage = 0;

    protected virtual void Start()
    {
    }
    
    protected virtual void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0) Expire();
        
	}

    protected virtual void Explode()
    {
        Destroy(gameObject);
    }

    protected virtual void Expire()
    {
        Destroy(gameObject);
    }
    

}
