using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : ILoopable {

    public static Logger MainLog = new Logger();
    private List<string> mainlogtxt = new List<string>();

    public static void Instantiate()
    {
        MainLoopable.GetInstance().RegisterLoops(MainLog);

    }

    public static void Log(string l)
    {
        MainLog.log(l);
    }
    public static void Log(System.Exception e)
    {
        MainLog.log(e);
    }
    public void log(string l)
    {
        mainlogtxt.Add(l);
    }
    public void log(System.Exception e)
    {
        mainlogtxt.Add(e.StackTrace.ToString());
    }
    
    public void Start () {
		
	}
	
	
	public void Update () {
        System.IO.File.WriteAllLines("Log.txt", new List<string>(mainlogtxt).ToArray());
	}

    public void OnApplicationQuit()
    {

    }
}
