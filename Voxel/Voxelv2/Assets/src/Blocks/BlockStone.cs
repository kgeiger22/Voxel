using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class BlockStone : Block
{
    public BlockStone()
        : base()
    {
        type = BlockType.Stone;
    }
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 0;
        return tile;
    }
}