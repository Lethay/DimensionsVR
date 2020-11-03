using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fillObjectWithMesh : MonoBehaviour
{

    private static Mesh viewedModel; // for the mesh of this object
    private Vector3 randPoint;
    private int[] oldTriangles;


    private static int numNewTriangles = 100000;
    private int[] newTriangles = new int[3*numNewTriangles];

    public class Triangle
    {
        public Vector3 vertexA, vertexB, vertexC;


        public Vector3 GetRandomPoint()
        {
            //This happens to be the easiest to calculate
            Vector3 min = GetMinPoint();
            Vector3 max = GetMaxPoint();

            //Give a random point.
            //NOTE! This will not return a point on the triangle, but instead just a random point within the area the triangle makes up.
            //You will have to find more complicated algorithms to detect a random point on 3D triangles.
            return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        }

        Vector3 GetMinPoint()
        {
            Vector3 point = new Vector3();
            point.x = Mathf.Min(vertexA.x, vertexB.x, vertexC.x);
            point.y = Mathf.Min(vertexA.y, vertexB.y, vertexC.y);
            point.z = Mathf.Min(vertexA.z, vertexB.z, vertexC.z);
            return point;
        }
        Vector3 GetMaxPoint()
        {
            Vector3 point = new Vector3();
            point.x = Mathf.Max(vertexA.x, vertexB.x, vertexC.x);
            point.y = Mathf.Max(vertexA.y, vertexB.y, vertexC.y);
            point.z = Mathf.Max(vertexA.z, vertexB.z, vertexC.z);
            return point;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //MeshFilter viewedModelFilter = (MeshFilter)gameObject.GetComponent("MeshFilter");
        //viewedModel = viewedModelFilter.mesh;
        //randPoint = GetRandomPoint(viewedModel);
        ////UnityEngine.Debug.LogFormat("random point: {0} ", randPoint);



        // Get instantiated mesh
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        // Find the old triangles in this mesh
        oldTriangles = mesh.triangles;

        // Construct random triangles
        //Random rnd = new Random(); // Instance of a random number generator

        int numVertices = mesh.vertices.Length; // number of vertices in this mesh
        int p = 0;
        while (p < numNewTriangles)
        {   // a random triangle
            //newTriangles[p * 3] = new int[] { Random.Range(0, numVertices), Random.Range(0, numVertices), Random.Range(0, numVertices) };
            newTriangles[p * 3] =  Random.Range(0, numVertices);
            newTriangles[p * 3 + 1] = Random.Range(0, numVertices);
            newTriangles[p * 3 + 2] = Random.Range(0, numVertices);

            //newTriangles[p * 3 + 1] = new int[] { Random.Range(0, numVertices), Random.Range(0, numVertices), Random.Range(0, numVertices) };
            //newTriangles[p * 3 + 2] = new int[] { Random.Range(0, numVertices), Random.Range(0, numVertices), Random.Range(0, numVertices) };
            p++;
        }
        //mesh.vertices = vertices;

        // Combine the together
        var combinedTrianglesList = new List<int>();
        combinedTrianglesList.AddRange(oldTriangles);
        combinedTrianglesList.AddRange(newTriangles);
        int[] combinedTrianglesArray = combinedTrianglesList.ToArray();


        mesh.triangles = combinedTrianglesArray;

        //UnityEngine.Debug.LogFormat("triangles: {0} ", mesh.triangles);

        mesh.RecalculateNormals();




    }

    // Update is called once per frame
    void Update()
    {

        //// Randomly change vertices
        //Vector3[] vertices = mesh.vertices;
        //int p = 0;
        //while (p < vertices.Length)
        //{
        //    vertices[p] += new Vector3(0, Random.Range(-0.3F, 0.3F), 0);
        //    p++;
        //}
        //mesh.vertices = vertices;

        //UnityEngine.Debug.LogFormat("triangles: {0} ", mesh.triangles);

       




    }
}
