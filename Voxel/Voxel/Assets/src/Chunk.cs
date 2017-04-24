using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : ITickable
{
    private static bool firstchunk = false;
    private bool isFirstChunk = false;
    private World world;

    public static readonly int ChunkWidth = 20;
    public static readonly int ChunkHeight = 20;

    private Block[,,] _Blocks;
    private int[,,] _BlockHP;

    public int PosX { get; private set; }
    public int PosY { get; private set; }
    public int PosZ { get; private set; }
    public Chunk(int px, int py, int pz, World world)
    {
        PosX = px;
        PosY = py;
        PosZ = pz;
        this.world = world;
    }

    public Chunk(int px, int py, int pz, int[,,] _data, World world)
    {
        PosX = px;
        PosY = py;
        PosZ = pz;
        LoadChunkFromData(_data);
        HasGenerated = true;
        this.world = world;
    }


    public float GetHeight(float px, float pz, float py)
    {
        px += (PosX * ChunkWidth);
        pz += (PosZ * ChunkWidth);

        float p1 = Mathf.PerlinNoise(px / GameManager.Sdx, pz / GameManager.Sdz) * GameManager.Smul;
        p1 *= (GameManager.Smy * py);
        return p1;
    }

    protected bool HasGenerated = false;
    public virtual void Start()
    {
        if (!firstchunk)
        {
            firstchunk = true;
            isFirstChunk = true;
        }
        if (HasGenerated) return;
        
        _Blocks = new Block[ChunkWidth, ChunkHeight, ChunkWidth];
        
        for (int x = 0; x < ChunkWidth; ++x)
        {
            for (int y = 0; y < ChunkHeight; ++y)
            {
                for (int z = 0; z < ChunkWidth; ++z)
                {
                    //Above the ground
                    //if (PosY > 0) _Blocks[x, y, z] = Block.Air;
                    //On ground level
                    if (true)
                    {
                        float perlin = GetHeight(x, z, y + ChunkHeight * PosY);
                        if (perlin > GameManager.Scutoff)
                        {
                            _Blocks[x, y, z] = Block.Air;
                        }
                        else
                        {
                            if (perlin < GameManager.Scutoff / 1.5) _Blocks[x, y, z] = Block.Gray;
                            else _Blocks[x, y, z] = Block.White;
                        }
                        if (y <= 1 && PosY == 0) _Blocks[x, y, z] = Block.Black;
                        if (PosX == 0 && PosZ == 0 && PosY == 0) _Blocks[x, y, z] = Block.Black;
                    }
                } 
            }
        }
        HasGenerated = true;
    }

    public void Tick()
    {

    }

    protected bool HasDrawn = false;
    private MeshData data;
    protected bool Drawlock = false;
    private bool NeedToUpdate = false;
    public virtual void Update()
    {
        if (NeedToUpdate)
        {
            if(!Drawlock && !RenderingLock)
            {
                HasDrawn = false;
                HasRendered = false;
                NeedToUpdate = false;
            }
        }

        if (!HasDrawn && HasGenerated && !Drawlock)
        {

            Drawlock = true;
            data = new MeshData();
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int y = 0; y < ChunkHeight; y++)
                {
                    for (int z = 0; z < ChunkWidth; z++)
                    {
                        data.Merge(_Blocks[x, y, z].Draw(this, _Blocks, x, y, z));
                    }

                }
            }
            HasDrawn = true;
            Drawlock = false;
        }

    }

    protected bool HasRendered = false;
    private GameObject go;
    private bool RenderingLock = false;
    public virtual void OnUnityUpdate()
    {
        if (HasGenerated && !HasRendered && HasDrawn && !RenderingLock)
        {
            RenderingLock = true;
            HasRendered = true;
            Mesh mesh = data.ToMesh();
            if (go == null)
            {
                go = new GameObject();
            }
            Transform t = go.transform;

            if (t.gameObject.GetComponent<MeshFilter>() == null)
            {
                t.gameObject.AddComponent<MeshFilter>();
                t.gameObject.AddComponent<MeshRenderer>();
                t.gameObject.AddComponent<MeshCollider>();
                t.gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Chunkmat");
                t.transform.position = new Vector3(PosX * ChunkWidth, PosY * ChunkHeight, PosZ * ChunkWidth);
                Texture2D tmp = new Texture2D(0, 0);
                tmp.LoadImage(System.IO.File.ReadAllBytes("textures/atlas.png"));
                tmp.filterMode = FilterMode.Point;
                tmp.wrapMode = TextureWrapMode.Clamp;
                tmp.anisoLevel = 3;
                t.gameObject.GetComponent<MeshRenderer>().material.mainTexture = tmp;




            }

            t.transform.GetComponent<MeshFilter>().sharedMesh = mesh;
            t.transform.GetComponent<MeshCollider>().sharedMesh = mesh;


            RenderingLock = false;
            if(isFirstChunk)
            {
                GameManager._Instance.Startplayer(new Vector3(PosX * ChunkWidth + ChunkWidth / 2, 25, PosZ * ChunkWidth + ChunkWidth / 2));
            }
            
        }
    }

    internal void SetBlock(int x, int y, int z, Block block)
    {
        _Blocks[x, y, z] = block;
        //Debug.Log(string.Format("X: {0}, Y: {1}, Z: {2}", x, y, z));
        NeedToUpdate = true;
    }

    public void Degenerate()
    {
        Serializer.Serialize_ToFile_FullPath(FileManager.GetChunkString(PosX, PosY, PosZ), GetChunkSaveData());
        GameManager._Instance.RegisterDelegate( new Action(() => {
            GameObject.Destroy(go);



        }));
        world.RemoveChunk(this);
    }

    public int[,,] GetChunkSaveData()
    {
        return _Blocks.ToIntArray();
    }
    public void LoadChunkFromData(int[,,] _data)
    {
        _Blocks = _data.ToBlockArray();
    }

    public string GetBlockAt(int x, int y, int z)
    {
        return _Blocks[x, y, z].GetBlockName();
    }



}
