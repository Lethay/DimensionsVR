using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateShape : MonoBehaviour {

    public GameObject verticesShapeObj;
    public GameObject edgesShapeObj;
    public GameObject facesShapeObj;
    public shapeUtils shapeUtils;
    public Vector4[] hypercubeVerticesCurrent;
    public Vector3 cubeStartPos;

    public readonly Vector4[] hypercubeVerticesInit = new Vector4[8]
    {
        new Vector4(-1,-1,-1,0),
        new Vector4(-1,-1,1,0),
        new Vector4(-1,1,-1,0),
        new Vector4(-1,1,1,0),
        new Vector4(1,-1,-1,0),
        new Vector4(1,-1,1,0),
        new Vector4(1,1,-1,0),
        new Vector4(1,1,1,0)
    };

    public readonly int[,] hypercubeEdges = new int[12,2]
    {
        {0,1},
        {0,2},
        {0,4},
        {1,3},
        {1,5},
        {2,3},
        {2,6},
        {3,7},
        {4,5},
        {4,6},
        {5,7},
        {6,7}
    };

    public readonly int[,] hypercubeFaces = new int[6, 4]
    {
        {0,1,2,3},
        {0,1,4,5},
        {0,2,4,6},
        {4,5,6,7},
        {2,3,6,7},
        {1,3,5,7}
    };

    // Use this for initialization
    void Start () {
        //Debug.Log("Start");

        shapeUtils = transform.parent.GetComponent<shapeUtils>();

        hypercubeVerticesCurrent = hypercubeVerticesInit;
        Vector3[] hypercubeVerticesProjected = geom4d.ProjectPerspective(hypercubeVerticesInit, 1f);

        shapeUtils.createShape(hypercubeVerticesProjected, hypercubeEdges, hypercubeFaces, this.gameObject, this.transform.localScale);
        Rigidbody Rbdy = this.gameObject.AddComponent<Rigidbody>();
        Rbdy.useGravity = false;
        Rbdy.constraints = RigidbodyConstraints.FreezePosition;

        verticesShapeObj = this.transform.Find("Vertices").gameObject;
        edgesShapeObj = this.transform.Find("Edges").gameObject;
        facesShapeObj = this.transform.Find("Faces").gameObject;

        this.transform.position = cubeStartPos;

    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("Update");

        for (int i = 0; i <= hypercubeVerticesCurrent.GetUpperBound(0); i++)
        {
            hypercubeVerticesCurrent[i] = geom4d.RotateVertex4d(hypercubeVerticesCurrent[i], 0f, 0.01f * Mathf.PI, 0f);
        }

        Vector3[] hypercubeVerticesProjected = geom4d.ProjectPerspective(hypercubeVerticesCurrent, 1f);

        shapeUtils.updateShape(hypercubeVerticesProjected, hypercubeEdges, hypercubeFaces, verticesShapeObj, edgesShapeObj, facesShapeObj, this.transform.localScale);
    }
}
