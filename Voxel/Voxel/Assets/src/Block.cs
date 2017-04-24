using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : ITickable
{

    private bool IsTransparent;
    public static Block Dirt = new Block("Dirt", false, "textures/blocks/dirt.png");
    public static Block Stone = new Block("Stone", false, "textures/blocks/stone.png");
    public static Block Bedrock = new Block("Bedrock", false, "textures/blocks/bedrock.png");
    public static Block Air = new Block("Air", true);
    public static Block Cobblestone = new Block("Cobblestone", false, "textures/blocks/cobblestone.png");
    public static Block Sand = new Block("Sand", false, "textures/blocks/sand.png");
    public static Block White = new Block("White", false, "textures/blocks/white.png");
    public static Block Gray = new Block("Gray", false, "textures/blocks/gray.png");
    public static Block Black = new Block("Black", false, "textures/blocks/black.png");





    //private string name;
    private Vector2[] _UvMap;
    private static int CurrentID = 0;
    private int ID;
    private string BlockName;

    public Block(string BlockName, bool IsTransparent)
    {
        this.IsTransparent = IsTransparent;
        this.BlockName = BlockName;
        REGISTER();
    }

    public Block(string BlockName, bool IsTransparent, string name)
    {
        this.IsTransparent = IsTransparent;
        this.BlockName = BlockName;
        //this.name = name;


        _UvMap = UvMap.GetUvMap(name)._UVMAP;
        REGISTER();
    }

    private void REGISTER()
    {
        ID = CurrentID;
        ++CurrentID;
        BlockRegistry.RegisterBlock(this);
    }

    public string GetBlockName()
    {
        return BlockName;
    }

    public int GetID()
    {
        return ID;
    }

    public bool Istransparent()
    {
        return IsTransparent;
    }




    public void Start()
    {
    }

    public void Tick()
    {
    }

    public void Update()
    {
    }

    public virtual MeshData Draw(Chunk chunk, Block[,,] _Blocks, int x, int y, int z)
    {
        if (this.Equals(Air)) return new MeshData();
        return MathHelper.DrawCube(chunk, _Blocks, this, x, y, z, this._UvMap);
    }

    public void OnUnityUpdate()
    {
    }
}
