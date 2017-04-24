using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Player {
    private Transform Add;
    private Transform Delete;
    private Block BlockType = Block.White;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        type = "Builder";
        maxjumps = 100;
        m_CanFly = true;
        Add = Instantiate(Resources.Load<Transform>("Add"), transform.position, Quaternion.identity) as Transform;
        Delete = Instantiate(Resources.Load<Transform>("Delete"), transform.position, Quaternion.identity) as Transform;
    }


    //private List<Vector3> WaitingToAddBlocks = new List<Vector3>();
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (changing)
        {
            changing = false;
            return;
        }
        //Releasing left click
        //Places block at position
        if (Input.GetMouseButtonUp(0))
        {
            Add.transform.GetComponent<MeshRenderer>().enabled = false;

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 rawposition = hit.point + hit.normal / 2;
                Vector3 roundedpostiion = new Vector3(Mathf.RoundToInt(rawposition.x), Mathf.RoundToInt(rawposition.y), Mathf.RoundToInt(rawposition.z));
                MathHelper.AddBlock(roundedpostiion, BlockType);
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BlockType = Block.White;
        }

        //Change block
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BlockType = Block.Gray;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BlockType = Block.Black;
        }

    }
}
