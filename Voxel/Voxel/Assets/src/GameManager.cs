using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    private List<Delegate> _Delegates = new List<Delegate>();
    public static GameManager _Instance;
    public Camera _camera;
    public Text UITEXT;
    public Transform Player;
    public Vector3 PlayerPosition;

    public float dx = 1f;
    public float dz = 1f;
    public float my = 1f;
    public float cutoff = 1f;
    public float mul = 1f;

    public static float Sdx = 50f;
    public static float Sdz = 50f;
    public static float Smy = .23f;
    public static float Scutoff = 1.8f;
    public static float Smul = 1f;


    private MainLoopable main = MainLoopable.GetInstance();
    private bool playerload = false;
    private bool playerspawned = false;
    public void RegisterDelegate(Delegate d)
    {
        _Delegates.Add(d);
    }
    public void Startplayer(Vector3 Pos)
    {
        if (playerspawned == true) return;
        Destroy(_camera.gameObject);
        Destroy(UITEXT);

        GameObject t = Instantiate(Resources.Load("Player"), Pos, Quaternion.identity) as GameObject;
        t.transform.position = Pos;
        Player = t.transform;
        playerspawned = true;
    }
	void Start () {

        FileManager.RegisterFiles();

        _Instance = this;

        TextureAtlas._Instance.CreateAtlas();
        MainLoopable.Instantiate();
        main = MainLoopable.GetInstance();
        main.Start();
	}
	
	void Update () {
        if (Player != null)
        {
            PlayerPosition = Player.transform.position;
            playerload = true;
        }
        //else Startplayer(new Vector3(0, 0, 0));
        Sdx = dx;
        Sdz = dz;
        Smy = my;
        Scutoff = cutoff;
        Smul = mul;

        main.Update();

        foreach(Delegate d in new List<Delegate>(_Delegates))
        {
            d.DynamicInvoke();
            _Delegates.Remove(d);
        }
        

    }

    void OnApplicationQuit()
    {
        main.OnApplicationQuit();
    }

    internal static bool PlayerLoaded()
    {
        return _Instance.playerload;
    }
}
