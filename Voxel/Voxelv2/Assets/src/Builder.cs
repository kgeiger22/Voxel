﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Player {
    private Transform Add;
    private Transform Delete;
    //private Block BlockType = Block.White;

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

        //Releasing left click
        //Places block at position
        if (Input.GetMouseButtonUp(0))
        {
            Add.transform.GetComponent<MeshRenderer>().enabled = false;

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 30, ~(1 << 8)))
            {
                EditTerrain.SetBlock(hit, new BlockDirt(), true);
            }
        }

        //Holding left click
        //Shows Add block on screen 
        else if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 30, ~(1 << 8)))
            {
                Add.transform.GetComponent<MeshRenderer>().enabled = true;
                Vector3 rawposition = hit.point + hit.normal / 2;
                Vector3 roundedpostiion = new Vector3(Mathf.RoundToInt(rawposition.x), Mathf.RoundToInt(rawposition.y), Mathf.RoundToInt(rawposition.z));
                Add.transform.position = roundedpostiion;
            }
            else Add.transform.GetComponent<MeshRenderer>().enabled = false;
        }
        //not holding left click

        //Releasing right click
        //Deletes block at position
        if (Input.GetMouseButtonUp(1))
        {
            Delete.transform.GetComponent<MeshRenderer>().enabled = false;

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 30, ~(1 << 8)))
            {
                EditTerrain.SetBlock(hit, new BlockAir());
            }
        }
        //Holding right click
        //Shows delete on screen
        else if (Input.GetMouseButton(1))
        {

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 30, ~(1 << 8)))
            {
                Delete.transform.GetComponent<MeshRenderer>().enabled = true;
                Vector3 rawposition = hit.point - hit.normal / 2;
                Vector3 roundedpostiion = new Vector3(Mathf.RoundToInt(rawposition.x), Mathf.RoundToInt(rawposition.y), Mathf.RoundToInt(rawposition.z));
                Delete.transform.position = roundedpostiion;
            }
            else Delete.transform.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    protected override void LoadWeapon()
    {
        
    }
}
