using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData {
    private List<Vector3> _Verts = new List<Vector3> ();
    private List<int> _Tris = new List<int>();
    private List<Vector2> _UVS = new List<Vector2>();

    public MeshData(List<Vector3> v, List<int> t, Vector2[] u)
    {
        _Verts = v;
        _Tris = t;
        _UVS = new List<Vector2>(u);
    }

    public MeshData()
    {

    }

    public void AddPos(Vector3 loc)
    {
        for(int i = 0; i < _Verts.Count; ++i)
        {
            _Verts[i] = _Verts[i] + loc;
        }
    }

    public void Merge(MeshData m)
    {
        if (m._Verts.Count <= 0)
        {
            return;
        }
        if (_Verts.Count <= 0) //
        {
            _Verts = m._Verts;
            _Tris = m._Tris;
            _UVS = m._UVS;
            return;
        }
        int count = _Verts.Count;

        //_Verts.AddRange(m._Verts);
        for (int i = 0; i < m._Verts.Count; ++i)
        {
            _Verts.Add(m._Verts[i]);
        }
        for (int i = 0; i < m._Tris.Count; ++i)
        {
            _Tris.Add(m._Tris[i] + count);
        }
        //_UVS.AddRange(m._UVS);
        for (int i = 0; i < m._UVS.Count; ++i)
        {
            _UVS.Add(m._UVS[i]);
        }

    }

    public Mesh ToMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = _Verts.ToArray();
        mesh.triangles = _Tris.ToArray();
        mesh.uv = _UVS.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }

}
