using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    protected float lifetime;
    protected float velocity;
    public bool IsDead = false;

    protected virtual void Start()
    {
        
    }


    protected virtual void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0) IsDead = true;

        
	}






}
