using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class hypercubeScript : MonoBehaviour {

    public GameObject verticesShapeObj;
    public GameObject edgesShapeObj;
    public GameObject facesShapeObj;
    public shapeUtils shapeUtils;
    private SceneChangeOnPress SceneChangeOnPress;

    private appProgress ap;
    private int progressLevel;

    public hypercubeAudioScript hypercubeAudioScript;
    private int maxProgressLevel;
    private bool audioNotFinished;

    public Vector4[] hypercubeVerticesCurrent;
    public Vector3 hypercubeStartPos;
    private Vector3 hypercubeStagePos;
    private Vector3 rotationPoint;
    public Vector3 hypercubeFinalScenePos;
    public Vector3 controllerSpawnPos;
    Vector3 hypercubeCurrentRotation;

    Animator controller4D_Animator;
    public RuntimeAnimatorController controller4D_AnimatorController;
    private InteractionBehaviour m_interactionBehaviour;

    public GameObject controllerPrefab;
    [HideInInspector] public GameObject controller;
    private InteractionBehaviour m_controllerInteractionBehaviour;
    Vector3 controllerRotation;

    [HideInInspector] public GameObject[] highlightedFaces;
    [HideInInspector] public GameObject highlightedCube;

    public int numRotations;

    public GameObject smokePuff;

    public float m_scaleIn4th=0.5f;
    private Vector3 m_localScale;
    private Vector4 m_localScale4D;
    public bool stageCompleted;

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

    public readonly int[,] hypercubeEdges = new int[32, 2]
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

    public readonly int[,] hypercubeCubes = new int[8,6]
    {
        {6,7,8,9,10,11},
        {0,1,2,3,4,5},
        {3,9,13,15,19,21},
        {1,7,12,16,19,22},
        {0,6,12,14,18,20},
        {4,10,13,17,18,23},
        {2,8,14,16,21,23},
        {5,11,15,17,20,22}
    };

    // Use this for initialization
    void Start () {
        ap = GameObject.Find("Main Camera").GetComponent<appProgress>();

        shapeUtils = transform.parent.GetComponent<shapeUtils>();
        SceneChangeOnPress = GameObject.Find("Main Camera").GetComponent<SceneChangeOnPress>();
        m_localScale = this.transform.localScale;
        m_localScale4D = new Vector4(m_localScale[0], m_localScale[1], m_localScale[2], m_scaleIn4th);

        hypercubeStagePos = ap.squareScript.squareStartPos;
        rotationPoint = new Vector3(0, hypercubeStartPos[1], hypercubeStartPos[2]);
        rotationPoint.Scale(transform.lossyScale);

        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 4; j++) hypercubeVerticesInit[i][j] *= m_localScale4D[j];
        }
        hypercubeVerticesCurrent = hypercubeVerticesInit.Clone() as Vector4[];

        ap.hypercubeStageInProgress = false;

        if (hypercubeAudioScript == null)
        {
            hypercubeAudioScript = GameObject.Find("Dialogue").GetComponent<hypercubeAudioScript>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        progressLevel = ap.progressLevel;
        maxProgressLevel = hypercubeAudioScript.maxProgressLevel;
        audioNotFinished = hypercubeAudioScript.audioNotFinished;
        m_localScale = this.transform.localScale;
        m_localScale4D = new Vector4(m_localScale[0], m_localScale[1], m_localScale[2], m_scaleIn4th);

        // STAGE 12: Transition - hypercube appears
        if (progressLevel == 12 && !ap.hypercubeStageInProgress && maxProgressLevel >= 12)
        {
            ap.hypercubeStageInProgress = !ap.hypercubeStageInProgress;
            stageCompleted = false;
            Vector3[] hypercubeVerticesProjected = geom4d.ProjectPerspective(hypercubeVerticesInit, m_localScale4D[3]);

            shapeUtils.createShape(hypercubeVerticesProjected, hypercubeEdges, hypercubeFaces, this.gameObject, m_localScale);
            Rigidbody Rbdy = this.gameObject.AddComponent<Rigidbody>();
            Rbdy.useGravity = false;
            Rbdy.constraints = RigidbodyConstraints.FreezePosition;

            this.gameObject.AddComponent<InteractionBehaviour>();
            m_interactionBehaviour = this.gameObject.GetComponent<InteractionBehaviour>();
            m_interactionBehaviour.manager = GameObject.Find("Interaction Manager").GetComponent<InteractionManager>();
            m_interactionBehaviour.enabled = false;
            ap.handAttachments.SetActive(true);

            verticesShapeObj = this.transform.Find("Vertices").gameObject;
            edgesShapeObj = this.transform.Find("Edges").gameObject;
            facesShapeObj = this.transform.Find("Faces").gameObject;

            this.transform.localPosition = hypercubeStartPos;
            stageCompleted = true;
        }

        // STAGE 13: Transition - rotate hypercube into centre view
        if (progressLevel == 13 && maxProgressLevel >= 13) { 
            if (
            (shapeUtils.RotateTowardsTarget(transform, rotationPoint, hypercubeStartPos, hypercubeStagePos, Vector3.left, -10f, 5f) == 1)
            && (!audioNotFinished)
            ) {
                ap.progressLevel += 1;
                ap.hypercubeStageInProgress = false;
            }
        }

        // STAGE 14: highlight different sub-cubes of hypercube and discuss projection
        if (progressLevel == 14 && !ap.hypercubeStageInProgress && maxProgressLevel >= 14)
        {
            StartCoroutine(cubeHighlighting());
            ap.hypercubeStageInProgress = !ap.hypercubeStageInProgress;
        }

        // STAGE 15: draw cube filling space of hypercube
        if (progressLevel == 15 && !ap.hypercubeStageInProgress && maxProgressLevel >= 15)
        {
            StartCoroutine(cubeSpaceFilling());
            ap.hypercubeStageInProgress = !ap.hypercubeStageInProgress;
        }

        // STAGE 16: Blank for comedic effect (Was: user free rotation to view hypercube)
        if (progressLevel == 16 && !ap.hypercubeStageInProgress && maxProgressLevel >= 16)
        {
            ap.progressLevel += 1;
            ap.hypercubeStageInProgress = false;
        }

        // STAGE 17: Animate 4D rotation example
        if (progressLevel == 17 && !ap.hypercubeStageInProgress && maxProgressLevel >= 17)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.angularVelocity = new Vector3(0, 0, 0);
            StartCoroutine(example4Drotation());
            ap.hypercubeStageInProgress = !ap.hypercubeStageInProgress;
        }

        // STAGE 18: Make 4D controller for hypercube appear        
        if (progressLevel == 18 && !ap.hypercubeStageInProgress && maxProgressLevel >= 18)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.angularVelocity = new Vector3(0, 0, 0);

            if (!ap.m_skipInteraction){
                StartCoroutine(controllerAppear());
                ap.hypercubeStageInProgress = !ap.hypercubeStageInProgress;
            }
            else{
                ap.progressLevel += 1;
                ap.cubeStageInProgress = true;
            }
        }

        // STAGE 19: Free playing with rotations of hypercube - NB. encourage exploration of different fixed axis rotations first
        if (progressLevel == 19 && !ap.hypercubeStageInProgress && maxProgressLevel >= 19)
        {
            m_interactionBehaviour.enabled = true;
            ap.handAttachments.SetActive(true);
            if (!ap.m_skipInteraction){
                controller4D_Animator.enabled = false;
                m_controllerInteractionBehaviour.enabled = true;
            }

            hypercubeCurrentRotation = transform.eulerAngles;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);

            if (!ap.m_skipInteraction) updateShapeFromController();

            transform.eulerAngles = hypercubeCurrentRotation;
            if (ap.m_skipInteraction) ap.progressLevel++;
        }

        // STAGE 20: Finish scene - turn on smoke to disappear and destroy cube controller
        if (progressLevel == 20 && !ap.hypercubeStageInProgress && maxProgressLevel >= 20)
        {
            m_interactionBehaviour.enabled = false;
            ap.handAttachments.SetActive(false);
            if (!ap.m_skipInteraction) StartCoroutine(destroyController());
            ap.hypercubeStageInProgress = !ap.hypercubeStageInProgress;

            ap.progressLevel += 1;
        }

        // STAGE 21: Finish scene - move hypercube to one side and rotate through 4D
        if (progressLevel == 21 && maxProgressLevel >= 21)
        {
            for (int i = 0; i <= hypercubeVerticesCurrent.GetUpperBound(0); i++)
            {
                hypercubeVerticesCurrent[i] = geom4d.RotateVertex4d(hypercubeVerticesCurrent[i], 0f, 0.01f * Mathf.PI, 0f);
            }

            Vector3[] hypercubeVerticesProjected = geom4d.ProjectPerspective(hypercubeVerticesCurrent, m_localScale4D[3]);

            shapeUtils.updateShape(hypercubeVerticesProjected, hypercubeEdges, hypercubeFaces, verticesShapeObj, edgesShapeObj, facesShapeObj, m_localScale);

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, hypercubeFinalScenePos, 2f * Time.deltaTime);

            //Normally, the square - being the longest to reach the final scene - controls the progress through the app here.
            //If we have no square, check when the HYPERCUBE has finished moving in order to stop the scene.
            if (ap.m_skipSquareAndCube)
            {
                float x = System.Math.Abs(transform.localPosition[0] - hypercubeFinalScenePos[0]),
                      y = System.Math.Abs(transform.localPosition[1] - hypercubeFinalScenePos[1]),
                      z = System.Math.Abs(transform.localPosition[2] - hypercubeFinalScenePos[2]),
                delta = 4f * Time.deltaTime;

                if (x < delta && y < delta && z < delta)
                {
                    StartCoroutine(wait(1f));
                    ap.progressLevel++;
                }
            }
        }

        // STAGE 22: Exit
        if (progressLevel >= 22 && maxProgressLevel >= 22)
        {
            SceneChangeOnPress.sceneChange(0);
        }

    }

    //Coroutines
    IEnumerator cubeHighlighting()
    {
        // yield return new WaitForSeconds(5f);

        Vector3[] startVertices = new Vector3[4]
        {
            new Vector3(-1,-1,-1),
            new Vector3(-1,1,-1),
            new Vector3(1,-1,-1),
            new Vector3(1,1,-1)
        };

        highlightedCube = new GameObject();
        highlightedCube.name = "highlighted_cube";
        highlightedCube.transform.parent = this.transform;
        highlightedCube.transform.localPosition = new Vector3(0, 0, 0);
        highlightedCube.transform.localEulerAngles = new Vector3(0, 0, 0);

        highlightedFaces = new GameObject[6];

        for (int i = 0; i < 6; i++)
        {
            string faceName = "highlighted_face_" + i.ToString();
            shapeUtils.createFaceMesh_highlighted(startVertices, shapeUtils.meshTriOrder, highlightedCube, faceName);
            highlightedFaces[i] = this.transform.Find(highlightedCube.name).transform.Find(faceName).gameObject;
            //highlightedFaces[i].transform.localPosition = highlightedFaces[i].transform.localPosition + GameObject.Find("Square").GetComponent<squareScript>().squareStartPos;
            highlightedFaces[i].transform.localPosition = new Vector3(0, 0, 0);
            highlightedFaces[i].transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        this.transform.Find("Faces").gameObject.SetActive(false);

        for (int i = 0; i <= hypercubeCubes.GetUpperBound(0); i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Vector3[] faceVertices = this.transform.Find("Faces").transform.Find("face_" + hypercubeCubes[i,j].ToString()).GetComponent<MeshFilter>().mesh.vertices;
                shapeUtils._updateFaceMeshByVertices(highlightedFaces[j], faceVertices);
            }
            yield return new WaitForSeconds(1.5f);
        }

        this.transform.Find("Faces").gameObject.SetActive(true);        
        Destroy(highlightedCube);

        while (audioNotFinished) yield return new WaitForSeconds(1f);

        ap.progressLevel += 1;
        ap.hypercubeStageInProgress = false;
        yield return ap.hypercubeStageInProgress;
    }

    IEnumerator cubeSpaceFilling()
    {

        yield return new WaitForSeconds(9f);

        Vector3[] startVertices = new Vector3[4]
        {
            new Vector3(-1,-1,-1),
            new Vector3(-1,1,-1),
            new Vector3(1,-1,-1),
            new Vector3(1,1,-1)
        };

        highlightedCube = new GameObject();
        highlightedCube.name = "highlighted_cube";
        highlightedCube.transform.parent = this.transform;
        highlightedCube.transform.localPosition = new Vector3(0, 0, 0);
        highlightedCube.transform.localEulerAngles = new Vector3(0, 0, 0);

        highlightedFaces = new GameObject[6];

        for (int i = 0; i < 6; i++)
        {
            string faceName = "highlighted_face_" + i.ToString();
            shapeUtils.createFaceMesh_highlighted(startVertices, shapeUtils.meshTriOrder, highlightedCube, faceName);
            highlightedFaces[i] = this.transform.Find(highlightedCube.name).transform.Find(faceName).gameObject;
            //highlightedFaces[i].transform.localPosition = highlightedFaces[i].transform.localPosition + GameObject.Find("Square").GetComponent<squareScript>().squareStartPos;
            highlightedFaces[i].transform.localPosition = new Vector3(0, 0, 0);
            highlightedFaces[i].transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        this.transform.Find("Faces").gameObject.SetActive(false);

        // float cosMax = 1f * m_localScale[2] / transform.lossyScale[2]; //already have local scale, but don't have world scale included in this. TODO: Use lossyScale throughout, or fix vertices
        int numPis = 6;
        for (float t = 0f; (t <= numPis * Mathf.PI || audioNotFinished); t += 3f * Time.deltaTime)
        {
            for (int i = 0; i < 6; i++)
            {                
                Vector4[] faceVertices4D = new Vector4[4];
                for(int v = 0; v < 4; v++)
                {
                    faceVertices4D[v] = hypercubeVerticesInit[hypercubeFaces[hypercubeCubes[6,i],v]] 
                        + new Vector4(0f, 0f, m_localScale[2] * (1-Mathf.Cos(t)), 0f);
                }

                Vector3[] faceVerticesProjected = geom4d.ProjectPerspective(faceVertices4D, m_localScale4D[3]);
                shapeUtils._updateFaceMeshByVertices(highlightedFaces[i], faceVerticesProjected);
            }
            if (t > numPis * Mathf.PI && audioNotFinished) numPis += 2;
            yield return null;
        }

        this.transform.Find("Faces").gameObject.SetActive(true);
        Destroy(highlightedCube);

        yield return new WaitForSeconds(2f);

        ap.progressLevel += 1;
        ap.hypercubeStageInProgress = false;

        yield return ap.hypercubeStageInProgress;
    }    

    IEnumerator controllerAppear()
    {
        controller = Instantiate(controllerPrefab, GameObject.Find("Shapes").transform);
        controller.transform.localPosition = controllerSpawnPos;
        controller.transform.localRotation = Quaternion.identity;
        controller.transform.localScale = Vector3.one * 2f;
        controller.transform.Find("Cylinder_X").GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        controller.transform.Find("Cylinder_Y").GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        controller.transform.Find("Cylinder_Z").GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        controller.name = "Hypercube 4D controller";

        controller4D_Animator = controller.gameObject.AddComponent<Animator>();
        controller4D_Animator.GetComponent<Animator>().runtimeAnimatorController = controller4D_AnimatorController;

        m_controllerInteractionBehaviour = controller.AddComponent<InteractionBehaviour>();
        m_controllerInteractionBehaviour.manager = GameObject.Find("Interaction Manager").GetComponent<InteractionManager>();
        m_controllerInteractionBehaviour.enabled = false;
        float animationTime = 5f;
        yield return new WaitForSecondsRealtime(animationTime);

        ap.progressLevel += 1;
        ap.hypercubeStageInProgress = false;

        yield return ap.hypercubeStageInProgress;
    }

    void updateShapeFromController()
    {
        controllerRotation = controller.transform.eulerAngles;

        updateShapeWithoutController(controllerRotation);
    }

    void updateShapeWithoutController(Vector3 rotation)
    {
        for (int i = 0; i <= hypercubeVerticesCurrent.GetUpperBound(0); i++)
        {
            hypercubeVerticesCurrent[i] = geom4d.RotateVertex4d(hypercubeVerticesInit[i], rotation[0] * Mathf.PI / 180f, rotation[1] * Mathf.PI / 180f, rotation[2] * Mathf.PI / 180f);
        }

        Vector3[] cubeVerticesProjected = geom4d.ProjectPerspective(hypercubeVerticesCurrent, m_localScale4D[3]);

        shapeUtils.updateShape(cubeVerticesProjected, hypercubeEdges, hypercubeFaces, verticesShapeObj, edgesShapeObj, facesShapeObj, m_localScale);
    }

    IEnumerator example4Drotation()
    {
        Vector3 rotationAngle = Vector3.zero;
        float targetRotation = numRotations * 360f;

        while (rotationAngle[1] <= targetRotation || audioNotFinished)
        {
            updateShapeWithoutController(rotationAngle);

            rotationAngle[1] += 75f * Time.deltaTime;
            //controller.transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
            if (rotationAngle[1] > targetRotation && audioNotFinished) targetRotation += 360f;

            yield return null;
        }

        updateShapeWithoutController(rotationAngle);

        //controller.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        ap.progressLevel += 1;
        ap.hypercubeStageInProgress = false;
        yield return ap.hypercubeStageInProgress;
    }

    IEnumerator destroyController()
    {
        transform.rotation = Quaternion.Euler(0f,0f,0f);
        controller.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        updateShapeFromController();

        Vector3 pos = controller.transform.localPosition;
        var smokeClone = Instantiate(smokePuff, pos, Quaternion.identity);
        float smokeDuration = 1.5f;
        Destroy(controller);
        Destroy(smokeClone, smokeDuration);

        yield return null;
    }

    IEnumerator waitBefore4DController(float time)
    {
        yield return new WaitForSeconds(time);
        ap.progressLevel += 1;
        ap.hypercubeStageInProgress = false;
        yield return ap.hypercubeStageInProgress;
    }

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
