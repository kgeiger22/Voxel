using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Int3
{
    public int x, y, z;
    public Int3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Int3(Vector3 pos)
    {
        x = (int)pos.x;
        y = (int)pos.y;
        z = (int)pos.z;
    }

    public override string ToString()
    {
        return string.Format("X:{0},Y{1},Z{2}", x, y, z);
    }

    internal void AddPos(Int3 int3)
    {
        x += int3.x;
        y += int3.y;
        z += int3.z;
    }

    internal void ToChunkCoordinates()
    {
        x = Mathf.FloorToInt(x / Chunk.ChunkWidth);
        z = Mathf.FloorToInt(z / Chunk.ChunkWidth);
    }
}

public class MathHelper {

    static private float f = 1f;

    public static MeshData DrawCube(Chunk chunk, Block[,,] _Blocks, Block block, int x, int y, int z, Vector2[] _uvmap)
    {
        MeshData d = new MeshData();

        if (block.Equals(Block.Air))
        {
            return new MeshData();
        }

        if (y - 1 <= 0 || _Blocks[x, y - 1, z].Istransparent())
        {
            d.Merge(new MeshData( // Bottom Face
        new List<Vector3>() {
            new Vector3(0,0,0),
            new Vector3(0,0,f),
            new Vector3(f,0,0),
            new Vector3(f,0,f)
        },
        new List<int>() {
                 0,2,1   ,3,1,2
        },
        _uvmap));
        }



        if (y + 1 >= Chunk.ChunkHeight || _Blocks[x, y + 1, z].Istransparent())
        {
            d.Merge(new MeshData( // Top Face
          new System.Collections.Generic.List<Vector3>() {
            new Vector3(0,f,0),
            new Vector3(0,f,f),
            new Vector3(f,f,0),
            new Vector3(f,f,f)
           },
           new System.Collections.Generic.List<int>() {
                 0,1,2    ,3,2,1
           },
            _uvmap));
        }



        if (x + 1 >= Chunk.ChunkWidth || _Blocks[x + 1, y, z].Istransparent())
        {
            d.Merge(new MeshData( // Back Face
          new System.Collections.Generic.List<Vector3>() {
            new Vector3(f,0,0),
            new Vector3(f,0,f),
            new Vector3(f,f,0),
            new Vector3(f,f,f)
           },
           new System.Collections.Generic.List<int>() {
                 0,2,1,3,1,2
           },
            _uvmap));

        }



        if (x - 1 <= 0 || _Blocks[x - 1, y, z].Istransparent())
        {
            d.Merge(new MeshData( // Front Face
         new System.Collections.Generic.List<Vector3>() {
            new Vector3(0,0,0),
            new Vector3(0,0,f),
            new Vector3(0,f,0),
            new Vector3(0,f,f)
          },
          new System.Collections.Generic.List<int>() {
                 0,1,2,3,2,1
          },
           _uvmap));
        }



        
        if (z + 1 >= Chunk.ChunkWidth || _Blocks[x, y, z + 1].Istransparent())
        {
            d.Merge(new MeshData( // Right Face
          new System.Collections.Generic.List<Vector3>() {
            new Vector3(0,0,f),
            new Vector3(f,0,f),
            new Vector3(0,f,f),
            new Vector3(f,f,f)
           },
           new System.Collections.Generic.List<int>() {
                 0,1,2,3,2,1
           },
            _uvmap));
        }



        
        if (z - 1 <= 0 || _Blocks[x, y, z - 1].Istransparent())
        {
            d.Merge(new MeshData( // Left Face
         new System.Collections.Generic.List<Vector3>() {
            new Vector3(0,0,0),
            new Vector3(f,0,0),
            new Vector3(0,f,0),
            new Vector3(f,f,0)
         },
         new System.Collections.Generic.List<int>() {
                 0,2,1    ,3,1,2
         },
         _uvmap));

        }
        
        

        d.AddPos(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));


        return d;
    }

   internal static void AddBlock(Vector3 roundedposition, Block block)
    {
        //if (roundedposition.y >= Chunk.ChunkHeight) return;
        Int3 chunkpos = new Int3(0,0,0);
        chunkpos.x = Mathf.FloorToInt(roundedposition.x / Chunk.ChunkWidth);
        chunkpos.y = Mathf.FloorToInt(roundedposition.y / Chunk.ChunkHeight);
        chunkpos.z = Mathf.FloorToInt(roundedposition.z / Chunk.ChunkWidth);

        Chunk currentchunk;
        try
        {
            currentchunk = World._instance.GetChunk(chunkpos.x, chunkpos.y, chunkpos.z);
            //Debug.Log(string.Format("ChunkX: {0}, ChunkY: {1}, ChunkZ: {2},", currentchunk.PosX, currentchunk.PosY, currentchunk.PosZ));
            if (currentchunk.GetType().Equals(typeof(ErroredChunk))) return;
            int x = (int)(roundedposition.x - chunkpos.x * Chunk.ChunkWidth);
            int z = (int)(roundedposition.z - chunkpos.z * Chunk.ChunkWidth);
            int y = (int)(roundedposition.y - chunkpos.y * Chunk.ChunkHeight);
            if (currentchunk.GetBlockAt(x,y,z) != "Black") currentchunk.SetBlock(x, y, z, block);

        }
        catch (System.Exception e)
        {
            Logger.Log(e.StackTrace);
        }
    }
}
