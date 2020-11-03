using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coroutineScript : MonoBehaviour {

    public shapeUtils shapeUtils;
    private bool faceSpaceFillingStarted = false;
    public Material faceMaterial_highlighted;

    // Use this for initialization
    void Start () {

        shapeUtils = transform.parent.GetComponent<shapeUtils>();

    }
	
	// Update is called once per frame
	void Update () {
        if (!faceSpaceFillingStarted) {
            StartCoroutine("faceSpaceFilling");
            faceSpaceFillingStarted = true;
        }
	}

    IEnumerator faceSpaceFilling()
    {
        Vector3[] startVertices = new Vector3[4]
        {
            new Vector3(-1,-1,-1),
        new Vector3(-1,1,-1),
        new Vector3(1,-1,-1),
        new Vector3(1,1,-1)
        };

        string faceName = "highlighted_face";        

        shapeUtils.createFaceMesh_highlighted(startVertices, shapeUtils.meshTriOrder, this.gameObject, faceName);

        for (float t = 0f; t <= 2 * Mathf.PI; t += Time.deltaTime)
        {
            Vector3[] faceVertices = new Vector3[4];

            for (int vert = 0; vert < 4; vert++)
            {
                faceVertices[vert] = new Vector3(startVertices[vert][0], startVertices[vert][1], t) + GameObject.Find("Square").GetComponent<squareScript>().squareStartPos;
            }
            Debug.Log(new Vector3(0f, 0f, t));
            this.transform.Find(faceName).transform.position = GameObject.Find("Square").GetComponent<squareScript>().squareStartPos;
            //shapeUtils._updateFaceMesh(this.transform.Find(faceName).gameObject, faceVertices);
        }

        yield return null;
    }

    private void createFaceMesh_highlighted(Vector3[] vertices, int[] triangles, GameObject parentObj, string objName)
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
}
