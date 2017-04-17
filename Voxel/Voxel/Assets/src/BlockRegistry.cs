using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRegistry {
    private static readonly bool DebugMode = false;
    private static List<Block> _REGISTEREDBLOCKS = new List<Block>();
    public static void RegisterBlock(Block B)
    {
        _REGISTEREDBLOCKS.Add(B);
    }

    public static void RegisterBlocks()
    {
        if (DebugMode)
        {
            int i = 0;
            List<string> _names = new List<string>();
            foreach (Block b in _REGISTEREDBLOCKS)
            {
                _names.Add(string.Format("CurrentID: {0}, Blockname: {1}, BlockID: {2}", i, b.GetBlockName(), b.GetID()));
                ++i;
            }
            System.IO.File.WriteAllLines("BlockRegistry.txt", _names.ToArray());
        }
    }

    internal static Block GetBlockFromID(int _id)
    {
        try
        {
            return _REGISTEREDBLOCKS[_id];
        }catch (System.Exception e)
        {
            Logger.Log(e.StackTrace);
        }
        return null;
    }
}
