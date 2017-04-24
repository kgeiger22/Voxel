using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization
{
    public static string saveFolderName = "voxelGameSaves";

    public static string SaveLocation(string worldName)
    {
        string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }

    public static string FileName(WorldPos chunkLocation)
    {
        string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
        return fileName;
    }

    public static void SaveChunk(Chunk chunk)
    {
        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.pos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        Block.BlockType[,,] blocktypes = new Block.BlockType[Chunk.chunkSize, Chunk.chunkSize, Chunk.chunkSize];
        for (int x = 0; x < Chunk.chunkSize; ++x)
        {
            for (int y = 0; y < Chunk.chunkSize; ++y)
            {
                for (int z = 0; z < Chunk.chunkSize; ++z)
                {
                    blocktypes[x, y, z] = chunk.blocks[x, y, z].type;
                }
            }
        }
        formatter.Serialize(stream, blocktypes);
        //formatter.Serialize(stream, chunk.blocks);
        stream.Close();

    }
    public static bool Load(Chunk chunk)
    {
        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.pos);

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        Block.BlockType[,,] blocktypes = (Block.BlockType[,,])formatter.Deserialize(stream);
        for (int x = 0; x < Chunk.chunkSize; ++x)
        {
            for (int y = 0; y < Chunk.chunkSize; ++y)
            {
                for (int z = 0; z < Chunk.chunkSize; ++z)
                {
                    switch (blocktypes[x,y,z])
                    {
                        case Block.BlockType.Air:
                            chunk.SetBlock(x, y, z, new BlockAir());
                            break;
                        case Block.BlockType.Dirt:
                            chunk.SetBlock(x, y, z, new BlockDirt());
                            break;
                        case Block.BlockType.Stone:
                            chunk.SetBlock(x, y, z, new BlockStone());
                            break;
                        case Block.BlockType.Grass:
                            chunk.SetBlock(x, y, z, new BlockGrass());
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        //chunk.blocks = (Block[,,])formatter.Deserialize(stream);
        stream.Close();
        return true;
    }
}