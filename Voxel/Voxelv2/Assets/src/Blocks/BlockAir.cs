using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockAir : Block
{
    public BlockAir()
        : base()
    {
        type = BlockType.Air;
        health = 0;
    }
    public override MeshData Blockdata
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }
    public override bool IsSolid(Direction direction)
    {
        return false;
    }
}