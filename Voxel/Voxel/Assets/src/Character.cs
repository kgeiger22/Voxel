using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public float firerate = 0.2f;
    private float cooldown = 0;
    private Transform Add;
    private Transform Delete;
    private Block BlockType = Block.Dirt;
    private List<Projectile> Bullets = new List<Projectile>();

	// Use this for initialization
	void Start () {
        Add = Instantiate(Resources.Load<Transform>("Add"), transform.position, Quaternion.identity) as Transform;
        Delete = Instantiate(Resources.Load<Transform>("Delete"), transform.position, Quaternion.identity) as Transform;
    }



    //private List<Vector3> WaitingToAddBlocks = new List<Vector3>();
    // Update is called once per frame
    void Update () {
        //Releasing left click
        //Places block at position
        foreach(Projectile p in new List<Projectile>(Bullets))
        {
            if(p.IsDead == true)
            {
                Bullets.Remove(p);
                Destroy(p.gameObject);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Add.transform.GetComponent<MeshRenderer>().enabled = false;

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 rawposition = hit.point + hit.normal / 2;
                Vector3 roundedpostiion = new Vector3(Mathf.RoundToInt(rawposition.x), Mathf.RoundToInt(rawposition.y), Mathf.RoundToInt(rawposition.z));
                MathHelper.AddBlock(roundedpostiion,BlockType);
            }
        }
        //Holding left click
        //Shows Add block on screen 
		else if (Input.GetMouseButton(0))
        {
            Add.transform.GetComponent<MeshRenderer>().enabled = true;

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 rawposition = hit.point + hit.normal / 2;
                Vector3 roundedpostiion = new Vector3(Mathf.RoundToInt(rawposition.x), Mathf.RoundToInt(rawposition.y), Mathf.RoundToInt(rawposition.z));
                Add.transform.position = roundedpostiion;
            }
        }
        //not holding left click

        //Releasing right click
        //Deletes block at position
        if (Input.GetMouseButtonUp(1))
        {
            Delete.transform.GetComponent<MeshRenderer>().enabled = false;


            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 rawposition = hit.point - hit.normal / 2;
                Vector3 roundedpostiion = new Vector3(Mathf.RoundToInt(rawposition.x), Mathf.RoundToInt(rawposition.y), Mathf.RoundToInt(rawposition.z));
                MathHelper.AddBlock(roundedpostiion, Block.Air);
            }
        }
        //Holding right click
        //Shows delete on screen
        else if (Input.GetMouseButton(1))
        {
            Delete.transform.GetComponent<MeshRenderer>().enabled = true;


            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 rawposition = hit.point - hit.normal / 2;
                Vector3 roundedpostiion = new Vector3(Mathf.RoundToInt(rawposition.x), Mathf.RoundToInt(rawposition.y), Mathf.RoundToInt(rawposition.z));
                Delete.transform.position = roundedpostiion;
            }
        }
        //not holding right click
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            BlockType = Block.Dirt;
        }
        
        //Change block
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BlockType = Block.Stone;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BlockType = Block.Cobblestone;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BlockType = Block.Sand;
        }


        if (Input.GetKey(KeyCode.E) && cooldown < 0)
        {
            Vector3 startpos = transform.position + new Vector3(0, 0.3f, 0);
            Bullets.Add(Instantiate(Resources.Load<Projectile>("Fireball"), startpos, Camera.main.transform.rotation));
            cooldown = firerate;
        }
        else cooldown -= Time.deltaTime;
    }
}
