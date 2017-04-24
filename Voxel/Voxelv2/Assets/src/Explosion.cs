using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float lifetime = 0.5f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (lifetime < 0) Destroy(gameObject);
        else lifetime -= Time.deltaTime;
	}
}
