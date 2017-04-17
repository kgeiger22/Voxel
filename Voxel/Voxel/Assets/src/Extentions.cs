using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extentions  {

	public static int[,,] ToIntArray(this Block[,,] _Chunkdata)
    {

        int lx = _Chunkdata.GetLength(0);
        int ly = _Chunkdata.GetLength(1);
        int lz = _Chunkdata.GetLength(2);
        int[,,] data = new int[lx, ly, lz];

        for (int x = 0; x < lx; ++x)
        {
            for (int y = 0; y < ly; ++y)
            {
                for (int z = 0; z < lz; ++z)
                {
                    data[x, y, z] = _Chunkdata[x, y, z].GetID();
                }
            }
        }
        return data;
    }

    public static Block[,,] ToBlockArray(this int[,,] _data)
    {
        int lx = _data.GetLength(0);
        int ly = _data.GetLength(1);
        int lz = _data.GetLength(2);
        Block[,,] Chunkdata = new Block[lx, ly, lz];

        for (int x = 0; x < lx; ++x)
        {
            for (int y = 0; y < ly; ++y)
            {
                for (int z = 0; z < lz; ++z)
                {
                    Chunkdata[x, y, z] = BlockRegistry.GetBlockFromID(_data[x, y, z]);
                }
            }
        }
        return Chunkdata;
    }
}
