﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UvMap {
    private static List<UvMap> _Maps = new List<UvMap>();
    public string name;
    public Vector2[] _UVMAP;
    public UvMap(string name, Vector2[] _UVMAP)
    {
        this.name = name;
        this._UVMAP = _UVMAP;
    }

    public void Register()
    {
        _Maps.Add(this);
    }

    public static UvMap GetUvMap(string name)
    {
        foreach (UvMap m in _Maps)
        {
            if (m.name.Equals(name)) return m;
        }

        return new UvMap("empty", new Vector2[0]);
    }
}
