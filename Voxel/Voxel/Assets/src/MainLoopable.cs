using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLoopable : ILoopable {

    private static MainLoopable _instance;
    private List<ILoopable> _RegisteredLoops = new List<ILoopable>();

    public static void Instantiate()
    {
        _instance = new MainLoopable();

        //register
        Logger.Instantiate();
        World.Instantiate();
        Block.Air.GetBlockName();
        BlockRegistry.RegisterBlocks();

    }

    public static MainLoopable GetInstance()
    {
        return _instance;
    }

    public void RegisterLoops(ILoopable l)
    {
        _RegisteredLoops.Add(l);
    }
    public void DeregisterLoops(ILoopable l)
    {
        _RegisteredLoops.Remove(l);
    }

    public void Start () {
		foreach(ILoopable l in _RegisteredLoops)
        {
            l.Start();
        }
	}
	
	
	public void Update () {
        //Logger.Log("Updating...");
        foreach (ILoopable l in _RegisteredLoops)
        {
            l.Update();
        }
    }

    public void OnApplicationQuit()
    {
        foreach (ILoopable l in _RegisteredLoops)
        {
            l.OnApplicationQuit();
        }
    }
}
