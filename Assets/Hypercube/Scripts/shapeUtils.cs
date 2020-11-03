using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shapeUtils : MonoBehaviour
{

    public static float capRadius = 0.15f;
    public static float sphereRadius = 2 * capRadius;
    public bool drawFaces = true;
    public GameObject edge;
    public GameObject vertex;
    public Material faceMaterial;
    public Material faceMaterial_highlighted;

    public static int[] meshTriOrder = new int[] { 0, 1, 3, 0, 3, 1, 0, 2, 3, 0, 3, 2 };

    public void createShape(Vector3[] hypercubeVertices, int[,] hypercubeEdges, int[,] hypercubeFaces, GameObject parentObj, Vector3 localScale)
    {
        GameObject Vertices = new GameObject();
        Vertices.name = "Vertices";
        Vertices.transform.parent = parentObj.transform;

        foreach (Vector3 vCoords in hypercubeVertices)
        {
            _createVertex(vCoords, Vertices, "vertex", localScale);
        }

        GameObject Edges = new GameObject();
        Edges.name = "Edges";
        Edges.transform.parent = parentObj.transform;

        for (int i = 0; i <= hypercubeEdges.GetUpperBound(0); i++)
        {
            Vector3 startInd = hypercubeVertices[hypercubeEdges[i, 0]];
            Vector3 endInd = hypercubeVertices[hypercubeEdges[i, 1]];
            _createEdge(startInd, endInd, Edges, "edge", localScale);
        }

        GameObject Faces = new GameObject();
        Faces.name = "Faces";
        Faces.transform.parent = parentObj.transform;

        if (drawFaces)
        {
            for (int i = 0; i <= hypercubeFaces.GetUpperBound(0); i++)
            {
                Vector3[] faceVertices = new Vector3[4]
                {
                    hypercubeVertices[hypercubeFaces[i, 0]],
                    hypercubeVertices[hypercubeFaces[i, 1]],
                    hypercubeVertices[hypercubeFaces[i, 2]],
                    hypercubeVertices[hypercubeFaces[i, 3]]
                };

                _createFaceMesh(faceVertices, meshTriOrder, Faces, "face_" + i.ToString());
            }
        }

    }

    public void updateShape(Vector3[] verticesUpdated, int[,] hypercubeEdges, int[,] hypercubeFaces, GameObject verticesGO, GameObject edgesGO, GameObject facesGO, Vector3 localScale)
    {

        for (int i = 0; i < verticesGO.transform.childCount; i++)
        {
            _updateVertex(verticesGO.transform.GetChild(i).gameObject, verticesUpdated[i]);
        }

        for (int i = 0; i < edgesGO.transform.childCount; i++)
        {
            Vector3 startInd = verticesUpdated[hypercubeEdges[i, 0]];
            Vector3 endInd = verticesUpdated[hypercubeEdges[i, 1]];
            _updateEdge(edgesGO.transform.GetChild(i).gameObject, startInd, endInd, localScale);
        }

        if (drawFaces)
        {
            for (int i = 0; i < facesGO.transform.childCount; i++)
            {
                Vector3[] faceVerticesUpdated = new Vector3[4]
                {
                    verticesUpdated[hypercubeFaces[i, 0]],
                    verticesUpdated[hypercubeFaces[i, 1]],
                    verticesUpdated[hypercubeFaces[i, 2]],
                    verticesUpdated[hypercubeFaces[i, 3]]
                };

                _updateFaceMeshByVertices(facesGO.transform.GetChild(i).gameObject, faceVerticesUpdated);
            }
        }
    }

    private void _createEdge(Vector3 edgeStart, Vector3 edgeEnd, GameObject parentObj, string objName, Vector3 localScale)
    {
        GameObject edge1 = Instantiate(edge);
        edge1.transform.parent = parentObj.transform;
        edge1.name = objName;
        Vector3 ls = new Vector3(capRadius * localScale[0], Vector3.Magnitude(edgeEnd - edgeStart) / 2, capRadius * localScale[0]);
        // ls.Scale(localScale);
        edge1.transform.localScale = ls;
        edge1.transform.localRotation = Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(edgeEnd - edgeStart));
        edge1.transform.localPosition = (edgeEnd + edgeStart) / 2f;
    }

    private void _createVertex(Vector3 vertexCoords, GameObject parentObj, string objName, Vector3 localScale)
    {
        GameObject sphere1 = Instantiate(vertex);
        sphere1.transform.parent = parentObj.transform;
        sphere1.name = objName;
        sphere1.transform.localPosition = vertexCoords;
        Vector3 ls = new Vector3(sphereRadius, sphereRadius, sphereRadius);
        ls.Scale(localScale);
        sphere1.transform.localScale = ls;
    }

    private void _createFaceMesh(Vector3[] vertices, int[] triangles, GameObject parentObj, string objName)
    {
        GameObject face1 = new GameObject();
        face1.transform.parent = parentObj.transform;
        face1.name = objName;
        face1.AddComponent<MeshFilter>();
        face1.AddComponent<MeshRenderer>();

        Mesh mesh1 = new Mesh();
        mesh1.MarkDynamic();
        face1.GetComponent<MeshFilter>().mesh = mesh1;
        mesh1.vertices = vertices;
        mesh1.triangles = triangles;
        face1.GetComponent<MeshRenderer>().material = faceMaterial;
        //mesh1.RecalculateBounds(); //this is done automatically when assigning triangles
    }

    public void createFaceMesh_highlighted(Vector3[] vertices, int[] triangles, GameObject parentObj, string objName)
    {
        GameObject face1 = new GameObject();
        face1.transform.parent = parentObj.transform;
        face1.name = objName;
        face1.AddComponent<MeshFilter>();
        face1.AddComponent<MeshRenderer>();

        Mesh mesh1 = new Mesh();
        mesh1.MarkDynamic();
        face1.GetComponent<MeshFilter>().mesh = mesh1;
        mesh1.vertices = vertices;
        mesh1.triangles = triangles;
        face1.GetComponent<MeshRenderer>().material = faceMaterial_highlighted;
    }

    public GameObject createFaceMesh_highlighted(GameObject originalFace, GameObject parentObj, string objName)
    {
        GameObject face1 = Object.Instantiate(originalFace);
        face1.transform.parent = parentObj.transform;
        face1.name = objName;
        face1.GetComponent<MeshRenderer>().material = faceMaterial_highlighted;
        face1.GetComponent<MeshFilter>().mesh.RecalculateBounds();

        return face1;
    }

    private void _updateVertex(GameObject vGO, Vector3 vCoords)
    {
        vGO.transform.localPosition = vCoords;
    }

    private void _updateEdge(GameObject eGO, Vector3 startInd, Vector3 endInd, Vector3 localScale)
    {
        Vector3 currSize = eGO.transform.localScale;
        currSize[1] = Vector3.Magnitude(endInd - startInd) / 2;
        eGO.transform.localScale = currSize;
        eGO.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(endInd - startInd));
        eGO.transform.localPosition = (endInd + startInd) / 2f;
    }

    public void _updateFaceMeshByVertices(GameObject fGO, Vector3[] vertices)
    {
        Mesh mesh = fGO.GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    // Helper function for moving objects in and out of view with a pretty arc
    public int RotateTowardsTarget(Transform transform, Vector3 globalRotationPoint, Vector3 start, Vector3 target, Vector3 axis, float rotVel, float movVel)
    {
        int xSign = target[0] > start[0] ? 1 : -1,
            ySign = target[1] > start[1] ? 1 : -1,
            zSign = target[2] > start[2] ? 1 : -1;
        Vector3 diff = target - transform.localPosition;

        if ((rotVel != 0)
            && (diff[0] / xSign >= 0)
            && (diff[1] / ySign >= 0)
            && (diff[2] / zSign >= 0)) //e.g. xSign>0 means target bigger than start. Then, if xDiff < 0, we've surpassed target
        {
            transform.RotateAround(globalRotationPoint, axis, rotVel * Time.deltaTime);
            return 0;
        }

        else
            return MoveTowardsTarget(transform, start, target, rotVel, movVel/2);
    }

    public int MoveTowardsTarget(Transform transform, Vector3 start, Vector3 target, float rotVel, float movVel)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, movVel * Time.deltaTime);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.identity, System.Math.Abs(rotVel) * Time.deltaTime);
        Vector3 diff = target - transform.localPosition;

        if (Mathf.Abs(diff[0] / movVel / Time.deltaTime) < 1 && Mathf.Abs(diff[1] / movVel / Time.deltaTime) < 1 && Mathf.Abs(diff[2] / movVel / Time.deltaTime) < 1)
        {
            transform.localPosition = target;
            transform.localRotation = Quaternion.identity;
            return 1;
        }

        return 0;
    }
}
