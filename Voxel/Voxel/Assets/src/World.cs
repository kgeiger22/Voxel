using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class World : ILoopable
{
    public static World _instance { get; private set; }
    private bool IsRunning;
    private Thread worldThread;
   // private static Int3 playerPos;
   // private static readonly int RenderDistanceInChunks = 3;
    public static void Instantiate()
    {
        _instance = new World();
        MainLoopable.GetInstance().RegisterLoops(_instance);
        //System.Random r = new System.Random();
        //playerPos = new Int3(0, 21, 0);


    }

    public void OnApplicationQuit()
    {
        foreach (Chunk c in new List<Chunk>(_LoadedChunks))
        {
            Serializer.Serialize_ToFile_FullPath(FileManager.GetChunkString(c.PosX, c.PosZ), c.GetChunkSaveData());

        }
        IsRunning = false;
        Logger.Log("Stopping world thread");
    }

    private bool RanOnce = false;
    private List<Chunk> _LoadedChunks = new List<Chunk>();



    public void Start()
    {
        IsRunning = true;
        worldThread = new Thread(() =>
        {
            Logger.Log("Initializing world thread");

            while (IsRunning)
            {
                try
                {


                    if (!RanOnce)
                    {
                        RanOnce = true;


                        for (int x = 0; x < 5; x++)
                        {
                            for (int z = 0; z < 5; z++)
                            {
                                Int3 newChunkPos = new Int3(x,0,z);

                                if (System.IO.File.Exists(FileManager.GetChunkString(newChunkPos.x, newChunkPos.z)))
                                {

                                    _LoadedChunks.Add(new Chunk(newChunkPos.x, newChunkPos.z, Serializer.Deserialize_From_File<int[,,]>(FileManager.GetChunkString(newChunkPos.x, newChunkPos.z)), this));
                                }
                                else _LoadedChunks.Add(new Chunk(newChunkPos.x, newChunkPos.z, this));
                            }
                        }

                        /*
                        for (int x = -RenderDistanceInChunks; x < RenderDistanceInChunks; x++)
                        {
                            for (int z = -RenderDistanceInChunks; z < RenderDistanceInChunks; z++)
                            {
                                Int3 newChunkPos = new Int3(playerPos.x, playerPos.y, playerPos.z);
                                newChunkPos.AddPos(new Int3(x * Chunk.ChunkWidth, 0, z * Chunk.ChunkWidth));
                                newChunkPos.ToChunkCoordinates();


                                if (System.IO.File.Exists(FileManager.GetChunkString(newChunkPos.x, newChunkPos.z)))
                                {

                                    _LoadedChunks.Add(new Chunk(newChunkPos.x, newChunkPos.z, Serializer.Deserialize_From_File<int[,,]>(FileManager.GetChunkString(newChunkPos.x, newChunkPos.z)), this));
                                }
                                else _LoadedChunks.Add(new Chunk(newChunkPos.x, newChunkPos.z, this));

                            }
                        }
                        */
                        foreach (Chunk c in _LoadedChunks)
                        {
                            c.Start();
                        }
                        

                    }
                    /*

                        if (GameManager.PlayerLoaded())
                    {
                        playerPos = new Int3(GameManager._Instance.PlayerPosition);
                    }
                        /*
                    foreach (Chunk c in new List<Chunk>(_LoadedChunks))
                    {
                        if (Vector2.Distance(new Vector2(c.PosX * Chunk.ChunkWidth, c.PosZ * Chunk.ChunkWidth), new Vector2(playerPos.x, playerPos.z)) > (RenderDistanceInChunks * 2) * Chunk.ChunkWidth)
                        {
                            c.Degenerate();

                        }
                    }
                    for (int x = -RenderDistanceInChunks; x < RenderDistanceInChunks; x++)
                    {
                        for (int z = -RenderDistanceInChunks; z < RenderDistanceInChunks; z++)
                        {
                            Int3 newChunkPos = new Int3(playerPos.x, playerPos.y, playerPos.z);
                            newChunkPos.AddPos(new Int3(x * Chunk.ChunkWidth, 0, z * Chunk.ChunkWidth));
                            newChunkPos.ToChunkCoordinates();
                            if (!ChunkExists(newChunkPos.x, newChunkPos.z))
                            {
                                if (System.IO.File.Exists(FileManager.GetChunkString(newChunkPos.x, newChunkPos.z)))
                                {
                                    Chunk c = new Chunk(newChunkPos.x, newChunkPos.z, Serializer.Deserialize_From_File<int[,,]>(FileManager.GetChunkString(newChunkPos.x, newChunkPos.z)), this);
                                    c.Start();
                                    _LoadedChunks.Add(c);
                                }
                                else
                                {
                                    Chunk c = new Chunk(newChunkPos.x, newChunkPos.z, this);
                                    c.Start();
                                    _LoadedChunks.Add(c);

                                    
                                }
                            }
                        }
                    }
                    */

                    foreach (Chunk c in new List<Chunk>(_LoadedChunks))
                    {
                        c.Update();
                    }


                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.Log(e.StackTrace);
                    Logger.Log(e);
                }
            }
            Logger.Log("World thread succesfully stopped");
            Logger.MainLog.Update();// Rerun last log;

        });
        worldThread.Start();
    }

    public bool BlockCollision(Vector3 p)
    {
        if (p.y > Chunk.ChunkHeight || p.y < 0) return false;
        int pxtochunk = Mathf.FloorToInt(p.x) / Chunk.ChunkWidth;
        int pztochunk = Mathf.FloorToInt(p.z) / Chunk.ChunkWidth;

        
        if (!ChunkExists(pxtochunk, pztochunk)) return false;
        else
        {
            Chunk c = GetChunk(pxtochunk, pztochunk);

            int pxtoblock = (Mathf.FloorToInt(p.x) - (pxtochunk) * Chunk.ChunkWidth);
            int pztoblock = (Mathf.FloorToInt(p.z) - (pztochunk) * Chunk.ChunkWidth);
            //Debug.Log(string.Format("PX: {0}, {1}   PZ: {2}, {3}", pxtochunk, pxtoblock, pztochunk, pztoblock));
            if (pxtoblock < 0 || pxtoblock > 19 || pztoblock < 0 || pztoblock > 19) return false;
            else if (c.GetBlockAt(pxtoblock, (Mathf.FloorToInt(p.y)), pztoblock) != "Air")
            {
                //Debug.Log(string.Format("PX: {0}, {1}  PY: {2}, {3}   PZ: {4}, {5}", pxtochunk, pxtoblock, 0, Mathf.FloorToInt(p.y), pztochunk, pztoblock));
                return true;
            }
            else return false;
        }
    }

    public bool ChunkExists(int posx, int posz)
    {
        foreach (Chunk c in new List<Chunk>(_LoadedChunks))
        {
            if (c.PosX.Equals(posx) && c.PosZ.Equals(posz))
            {
                return true;
            }
        }
        return false;
    }

    public Chunk GetChunk(int posx, int posz)
    {
        foreach (Chunk c in new List<Chunk>(_LoadedChunks))
        {
            if (c.PosX.Equals(posx) && c.PosZ.Equals(posz))
            {
                return c;
            }
        }
        return new ErroredChunk(0, 0, this);
    }

    public void Update()
    {
        {
            foreach (Chunk c in new List<Chunk>(_LoadedChunks))
            {
                if (c != null)
                {
                    c.OnUnityUpdate();
                }

            }
        }

    }

    internal void RemoveChunk(Chunk chunk)
    {
        _LoadedChunks.Remove(chunk);
    }
}
