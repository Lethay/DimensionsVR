using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TO DO: add controller for 4d rotations and link movements,
//make sure that interaction of hands with 3d cube rotations move whole shape + fix coordinates to locations (rigidbody etc),
//make stage that rotates to (0,0,0) to change 'scene', decide on best projection, add user interaction menu/buttons/sliders,
//combine to allow rotations purely in 3d by interacting with hypercube

public class CreateShape4d : MonoBehaviour {

    public GameObject verticesShapeObj;
    public GameObject edgesShapeObj;
    public GameObject facesShapeObj;
    public shapeUtils shapeUtils;
    public Vector4[] hypercubeVerticesCurrent;

    public GameObject controllerObj;
    public GameObject controller;
    public Vector3 controllerRotation;
    public Vector3 hypercubeRotation3D;

    public readonly Vector4[] hypercubeVerticesInit = new Vector4[16]
    {
        new Vector4(-1,-1,-1,-1),
        new Vector4(-1,-1,1,-1),
        new Vector4(-1,1,-1,-1),
        new Vector4(-1,1,1,-1),
        new Vector4(1,-1,-1,-1),
        new Vector4(1,-1,1,-1),
        new Vector4(1,1,-1,-1),
        new Vector4(1,1,1,-1),
        new Vector4(-1,-1,-1,1),
        new Vector4(-1,-1,1,1),
        new Vector4(-1,1,-1,1),
        new Vector4(-1,1,1,1),
        new Vector4(1,-1,-1,1),
        new Vector4(1,-1,1,1),
        new Vector4(1,1,-1,1),
        new Vector4(1,1,1,1)
    };

    public readonly int[,] hypercubeEdges = new int[32,2]
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
        {6,7},
        {8,9},
        {8,10},
        {8,12},
        {9,11},
        {9,13},
        {10,11},
        {10,14},
        {11,15},
        {12,13},
        {12,14},
        {13,15},
        {14,15},
        {0,8},
        {1,9},
        {2,10},
        {3,11},
        {4,12},
        {5,13},
        {6,14},
        {7,15}
    };

    public readonly int[,] hypercubeFaces = new int[24, 4]
    {
        {0,1,2,3},
        {0,1,4,5},
        {0,2,4,6},
        {4,5,6,7},
        {2,3,6,7},
        {1,3,5,7},
        {8,9,10,11},
        {8,9,12,13},
        {8,10,12,14},
        {12,13,14,15},
        {10,11,14,15},
        {9,11,13,15},
        {0,1,8,9},
        {6,7,14,15},
        {0,2,8,10},
        {5,7,13,15},
        {0,4,8,12},
        {3,7,11,15},
        {2,3,10,11},
        {4,5,12,13},
        {1,3,9,11},
        {4,6,12,14},
        {1,5,9,13},
        {2,6,10,14}
    };

    // Use this for initialization
    void Start () {
        //Debug.Log("Start");

        shapeUtils = transform.parent.GetComponent<shapeUtils>();

        hypercubeVerticesCurrent = hypercubeVerticesInit.Clone() as Vector4[];

        Vector3[] hypercubeVerticesProjected = geom4d.ProjectPerspective(hypercubeVerticesInit, 1f);

        shapeUtils.createShape(hypercubeVerticesProjected,hypercubeEdges,hypercubeFaces,this.gameObject, this.transform.localScale);
        Rigidbody Rbdy = this.gameObject.AddComponent<Rigidbody>();
        Rbdy.useGravity = false;
        Rbdy.constraints = RigidbodyConstraints.FreezePosition;

        verticesShapeObj = this.transform.Find("Vertices").gameObject;
        edgesShapeObj = this.transform.Find("Edges").gameObject;
        facesShapeObj = this.transform.Find("Faces").gameObject;

        controller = Instantiate(controllerObj,new Vector3(-5f,0f,-5f),Quaternion.identity);
        controller.name = "Hypercube controller";
        controllerRotation = controller.transform.eulerAngles;
        
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("Update");

        //for (int i = 0; i <= hypercubeVerticesInit.GetUpperBound(0); i++)
        //{
        //    hypercubeVerticesCurrent[i] = geom4d.RotateVertex4d(hypercubeVerticesCurrent[i], 0, 0, 0.01f * Mathf.PI);
        //}

        //controllerRotation += new Vector3(0f, 0f, 100f * Time.deltaTime);
        //controller.transform.eulerAngles = controllerRotation;

        //controller.transform.GetChild(3).GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

        //controller.transform.GetChild(3).GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

        controllerRotation = controller.transform.eulerAngles;

        hypercubeRotation3D = this.transform.eulerAngles;

        for (int i = 0; i <= hypercubeVerticesCurrent.GetUpperBound(0); i++)
        {
            hypercubeVerticesCurrent[i] = geom4d.RotateVertex4d(hypercubeVerticesInit[i], controllerRotation[0] * Mathf.PI / 180f, controllerRotation[1] * Mathf.PI / 180f, controllerRotation[2] * Mathf.PI / 180f);
        }

        Vector3[] hypercubeVerticesProjected = geom4d.ProjectPerspective(hypercubeVerticesCurrent, 1f);

        shapeUtils.updateShape(hypercubeVerticesProjected, hypercubeEdges, hypercubeFaces, verticesShapeObj, edgesShapeObj, facesShapeObj, this.transform.localScale);
    }
}
