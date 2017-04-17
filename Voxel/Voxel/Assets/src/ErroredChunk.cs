using System;
using UnityEngine;
using System.Collections;

public class ErroredChunk : Chunk
{
    public ErroredChunk(int px, int pz, World world) : base(px, pz, world)
    {
    }

    public override void Start()
    {
        throw new Exception("Tried to use start erroredchunk class");
    }
    public override void Update()
    {
        throw new Exception("Tried to use update erroredchunk class");
    }
    public override void OnUnityUpdate()
    {
        throw new Exception("Tried to use onunityupdate erroredchunk class");
    }
}
