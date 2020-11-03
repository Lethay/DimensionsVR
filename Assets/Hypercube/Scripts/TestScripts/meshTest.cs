using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshTest : MonoBehaviour {

    public Material faceMaterial;

    // Use this for initialization
    void Start() {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(-1,1,0),
            new Vector3(2,1,0),
            new Vector3(1,0,0)
        };
        int[] triangles = new int[]{0,1,2,0,2,3,0,3,2,0,2,1};
        _createMesh(vertices, triangles);
    }

    // Update is called once per frame
    void Update() {

    }

    void _createMesh(Vector3[] vertices,int[] triangles)
    {
        GameObject face1 = new GameObject();
        face1.transform.parent = this.transform;
        face1.AddComponent<MeshFilter>();
        face1.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        face1.name = "testMesh";
        face1.GetComponent<MeshFilter>().mesh = mesh;
        face1.GetComponent<MeshRenderer>().material = faceMaterial;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
