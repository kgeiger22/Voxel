using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevChunk : Chunk {

	public DevChunk(int px, int py, int pz, World world) : base(px, py, pz, world)
    {

    }

    public override void OnUnityUpdate()
    {
        
        if (HasGenerated && !HasRendered && HasDrawn)
        {
            
            base.OnUnityUpdate();
            HasGenerated = false;
            HasDrawn = false;
            HasRendered = false;
            Start();
        }


    }
}
