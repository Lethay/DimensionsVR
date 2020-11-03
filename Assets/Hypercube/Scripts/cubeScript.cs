using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class cubeScript : MonoBehaviour {

    public GameObject verticesShapeObj;
    public GameObject edgesShapeObj;
    public GameObject facesShapeObj;
    public shapeUtils shapeUtils;

    private appProgress ap;
    private int progressLevel;

    public hypercubeAudioScript hypercubeAudioScript;
    private int maxProgressLevel;
    private bool audioNotFinished;

    Vector4[] cubeVerticesCurrent;
    public Vector3 cubeStartPos;
    public Vector3 cubeEndPos;
    private Vector3 cubeStagePos;
    private Vector3 rotationPoint;
    public Vector3 cubeFinalScenePos;
    public Vector3 controllerSpawnPos;
    Animator cubeAnimator;
    public RuntimeAnimatorController cubeAnimatorController;
    Vector3 cubeCurrentRotation;

    Animator controller4D_Animator;
    public RuntimeAnimatorController controller4D_AnimatorController;
    private InteractionBehaviour m_interactionBehaviour;

    [HideInInspector] public GameObject highlightedFace;

    public GameObject controllerPrefab;
    [HideInInspector] public GameObject controller;
    private InteractionBehaviour m_controllerInteractionBehaviour;
    Vector3 controllerRotation;

    public int numRotations;

    public GameObject smokePuff;

    private Vector3 m_localScale;
    bool stageCompleted;

    public readonly Vector4[] cubeVerticesInit = new Vector4[8]
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

    public readonly int[,] cubeEdges = new int[12, 2]
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

    public readonly int[,] cubeFaces = new int[6, 4]
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
        //Get references to other objects
        ap = GameObject.Find("Main Camera").GetComponent<appProgress>();
        if (hypercubeAudioScript == null)
        {
            hypercubeAudioScript = GameObject.Find("Dialogue").GetComponent<hypercubeAudioScript>();
        }

        //Stop here if we don't want to use the cube
        if (ap.m_skipSquareAndCube)
            return;

        //Define this object
        shapeUtils = transform.parent.GetComponent<shapeUtils>();
        m_localScale = this.transform.localScale;

        cubeStagePos = ap.squareScript.squareStartPos;
        rotationPoint = new Vector3(0, cubeStartPos[1], cubeStartPos[2]);
        rotationPoint.Scale(transform.lossyScale);

        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 3; j++) cubeVerticesInit[i][j] *= m_localScale[j];

        cubeVerticesCurrent = cubeVerticesInit.Clone() as Vector4[];

        cubeAnimator = this.gameObject.AddComponent<Animator>();

        ap.cubeStageInProgress = false;

    }
	
	// Update is called once per frame
	void Update () {
        progressLevel = ap.progressLevel;
        maxProgressLevel = hypercubeAudioScript.maxProgressLevel;
        audioNotFinished = hypercubeAudioScript.audioNotFinished;

        if (ap.m_skipSquareAndCube)
        {
            if (progressLevel == 12 && GameObject.Find("Hypercube").GetComponent<hypercubeScript>().stageCompleted)
            {
                ap.progressLevel += 1;
            }
            return;
        }
        m_localScale = this.transform.localScale;

        // STAGE 3: Transition - cube appears
        if (progressLevel==3)
        {
            Vector3[] cubeVerticesProjected = geom4d.ProjectPerspective(cubeVerticesInit, m_localScale[2]);

            shapeUtils.createShape(cubeVerticesProjected, cubeEdges, cubeFaces, this.gameObject, m_localScale);
            Rigidbody Rbdy = this.gameObject.AddComponent<Rigidbody>();            
            Rbdy.useGravity = false;
            Rbdy.constraints = RigidbodyConstraints.FreezePosition;

            this.gameObject.AddComponent<InteractionBehaviour>();
            m_interactionBehaviour = this.gameObject.GetComponent<InteractionBehaviour>();
            m_interactionBehaviour.manager = GameObject.Find("Interaction Manager").GetComponent<InteractionManager>();
            m_interactionBehaviour.enabled = false;
            ap.handAttachments.SetActive(false);

            verticesShapeObj = this.transform.Find("Vertices").gameObject;
            edgesShapeObj = this.transform.Find("Edges").gameObject;
            facesShapeObj = this.transform.Find("Faces").gameObject;

            this.transform.localPosition = cubeStartPos;

            ap.progressLevel += 1;                       
        }
        
        // STAGE 4: Transition - rotate cube into centre view
        if (progressLevel == 4 && maxProgressLevel >= 4)
        {
            if (shapeUtils.RotateTowardsTarget(transform, rotationPoint, cubeStartPos, cubeStagePos, Vector3.left, -10f, 5f) == 1)
                ap.progressLevel += 1;
        }

        // STAGE 5: highlight different faces of cube
        if (progressLevel == 5 && !ap.cubeStageInProgress && maxProgressLevel >= 5)
        {
            StartCoroutine(faceHighlighting());
            ap.cubeStageInProgress = !ap.cubeStageInProgress;
        }

        // STAGE 6: plane filling space of cube and discuss projections.
        if (progressLevel == 6 && !ap.cubeStageInProgress && maxProgressLevel >= 6)
        {
            StartCoroutine(faceSpaceFilling());
            ap.cubeStageInProgress = !ap.cubeStageInProgress;
        }

        // STAGE 7: 3D rotations animation
        if (progressLevel == 7 && !ap.cubeStageInProgress && maxProgressLevel >= 7)
        {
            StartCoroutine(rotations3DAnimation());
            ap.cubeStageInProgress = !ap.cubeStageInProgress;
        }

        // STAGE 8: free playing moving cube around, introduce menu to reset orientation 
        if (progressLevel == 8 && maxProgressLevel >= 8)
        {
            m_interactionBehaviour.enabled = true;
            ap.handAttachments.SetActive(true);
            cubeAnimator.enabled = false;
            if (ap.m_skipInteraction) ap.progressLevel++;
        }

        // STAGE 9: example rotation of cube in 4D
        if (progressLevel == 9 && !ap.cubeStageInProgress && maxProgressLevel >= 9)
        {
            m_interactionBehaviour.enabled = false;
            ap.handAttachments.SetActive(false);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.angularVelocity = new Vector3(0, 0, 0);
            StartCoroutine(example4Drotation());
            ap.cubeStageInProgress = !ap.cubeStageInProgress;
        }

        // STAGE 10: Make 4D controller for cube appear
        if (progressLevel == 10 && !ap.cubeStageInProgress && maxProgressLevel >= 10)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.angularVelocity = new Vector3(0, 0, 0);
            cubeAnimator.enabled = false;

            if (!ap.m_skipInteraction){
                StartCoroutine(controllerAppear());
                ap.cubeStageInProgress = !ap.cubeStageInProgress;
            }
            else{
                ap.progressLevel += 1;
                ap.cubeStageInProgress = true;
            }
        }

        // STAGE 11: free playing with 4D rotations of cube
        if (progressLevel == 11 && maxProgressLevel >= 11)
        {
            m_interactionBehaviour.enabled = true;
            ap.handAttachments.SetActive(true);
            if (!ap.m_skipInteraction){
                controller4D_Animator.enabled = false;
                m_controllerInteractionBehaviour.enabled = true;
            }

            cubeCurrentRotation = transform.eulerAngles;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);

            if (!ap.m_skipInteraction) updateShapeFromController();

            transform.eulerAngles = cubeCurrentRotation;
            if (ap.m_skipInteraction) ap.progressLevel++;
        }

        // STAGE 12: Transition - turn on smoke to disappear and destroy cube controller
        if (progressLevel == 12 && !ap.cubeStageInProgress && maxProgressLevel >= 12)
        {
            m_interactionBehaviour.enabled = false;
            ap.handAttachments.SetActive(false);
            if (!ap.m_skipInteraction) StartCoroutine(destroyController());
            ap.cubeStageInProgress = !ap.cubeStageInProgress;
        }

        if (progressLevel == 12 && GameObject.Find("Hypercube").GetComponent<hypercubeScript>().stageCompleted && maxProgressLevel >= 12)
        {
            ap.progressLevel += 1;
        }

        // STAGE 13: Transition - move cube and make hypercube come into view
        if (progressLevel == 13 && maxProgressLevel >= 13)
        {
            Vector3 rotationPoint = new Vector3(0f, cubeStartPos[1], cubeEndPos[2]);
            shapeUtils.RotateTowardsTarget(transform, rotationPoint, cubeStagePos, cubeEndPos, Vector3.up, -10f, 5f);
        }

        if (progressLevel >= 13 && maxProgressLevel >= 13)
        {
            transform.Rotate(100f * Vector3.right * Time.deltaTime);
        }

        // STAGE 21: Finish scene - move cube back into view
        if (progressLevel == 21 && maxProgressLevel >= 21)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, cubeFinalScenePos, 4f * Time.deltaTime);
        }
    }

    //Coroutines
    IEnumerator faceHighlighting()
    {
        yield return new WaitForSeconds(2f);

        Vector3[] startVertices = new Vector3[4]
        {
            new Vector3(-1,-1,-1),
            new Vector3(-1,1,-1),
            new Vector3(1,-1,-1),
            new Vector3(1,1,-1)
        };

        int[] faceOrder = {2,5,3,1,0,4};

        string faceName = "highlighted_face";
        shapeUtils.createFaceMesh_highlighted(startVertices, shapeUtils.meshTriOrder, this.gameObject, faceName);
        highlightedFace = this.transform.Find("highlighted_face").gameObject;
        highlightedFace.transform.localPosition = new Vector3( 0, 0, 0 );
        highlightedFace.transform.localEulerAngles = new Vector3(0, 0, 0);

        this.transform.Find("Faces").gameObject.SetActive(false);

        for (int i=0;i<6;i++)
        {
            Vector3[] faceVertices = this.transform.Find("Faces").transform.Find("face_" + faceOrder[i].ToString()).GetComponent<MeshFilter>().mesh.vertices;
            shapeUtils._updateFaceMeshByVertices(highlightedFace, faceVertices);

            yield return new WaitForSeconds(1.5f);
        }

        this.transform.Find("Faces").gameObject.SetActive(true);
        //this.transform.Find("Faces").transform.Find("face_" + faceOrder[5].ToString()).gameObject.SetActive(true);
        Destroy(highlightedFace);

        while (audioNotFinished) yield return new WaitForSeconds(1f);

        ap.progressLevel += 1;
        ap.cubeStageInProgress = false;
        yield return ap.cubeStageInProgress;
    }

    IEnumerator faceSpaceFilling()
    {
        // yield return new WaitForSeconds(2f);

        Vector3[] startVertices = new Vector3[4]
        {
            new Vector3(-m_localScale[0], -m_localScale[1], -m_localScale[2]),
            new Vector3(-m_localScale[0],  m_localScale[1], -m_localScale[2]),
            new Vector3( m_localScale[0], -m_localScale[1], -m_localScale[2]),
            new Vector3( m_localScale[0],  m_localScale[1], -m_localScale[2])
        };        

        string faceName = "highlighted_face";
        shapeUtils.createFaceMesh_highlighted(startVertices, shapeUtils.meshTriOrder, this.gameObject, faceName);
        highlightedFace = this.transform.Find("highlighted_face").gameObject;
        highlightedFace.transform.localPosition = new Vector3(0, 0, 0);
        highlightedFace.transform.localEulerAngles = new Vector3(0, 0, 0);

        this.transform.Find("Faces").gameObject.SetActive(false);

        float cosMax = 1f * m_localScale[2] / transform.lossyScale[2]; //already have local scale, but don't have world scale included in this. TODO: Use lossyScale throughout, or fix vertices
        int numPis = 6;
        for (float t = 0f; (t <= numPis * Mathf.PI || audioNotFinished); t += 3f * Time.deltaTime)
        {
            highlightedFace.transform.localPosition = new Vector3(0f, 0f, cosMax * (1 - Mathf.Cos(t))); //From 2 to 0 //GameObject.Find("Square").GetComponent<squareScript>().squareStartPos + new Vector3(0f,0f,1-Mathf.Cos(t));
            if (t > numPis * Mathf.PI && audioNotFinished) numPis += 2;
            yield return null;
        }

        this.transform.Find("Faces").gameObject.SetActive(true);
        Destroy(highlightedFace);

        yield return new WaitForSeconds(2f);

        ap.progressLevel += 1;
        ap.cubeStageInProgress = false;

        yield return ap.cubeStageInProgress;
    }

    IEnumerator rotations3DAnimation()
    {
        yield return new WaitForSeconds(2f);

        float animationTime = 6f;

        cubeAnimator.GetComponent<Animator>().runtimeAnimatorController = cubeAnimatorController;        

        yield return new WaitForSeconds(animationTime);

        ap.progressLevel += 1;
        ap.cubeStageInProgress = false;

        yield return ap.cubeStageInProgress;
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
        controller.name = "Cube 4D controller";

        controller4D_Animator = controller.gameObject.AddComponent<Animator>();
        controller4D_Animator.GetComponent<Animator>().runtimeAnimatorController = controller4D_AnimatorController;

        m_controllerInteractionBehaviour = controller.AddComponent<InteractionBehaviour>();
        m_controllerInteractionBehaviour.manager = GameObject.Find("Interaction Manager").GetComponent<InteractionManager>();
        m_controllerInteractionBehaviour.enabled = false;

        float animationTime = 5f;
        yield return new WaitForSecondsRealtime(animationTime);

        ap.progressLevel += 1;
        ap.cubeStageInProgress = false;

        yield return ap.cubeStageInProgress;
    }

    void updateShapeFromController()
    {
        controllerRotation = controller.transform.eulerAngles;

        updateShapeWithoutController(controllerRotation);
    }

    void updateShapeWithoutController(Vector3 rotation)
    {
        for (int i = 0; i <= cubeVerticesCurrent.GetUpperBound(0); i++)
        {
            cubeVerticesCurrent[i] = geom4d.RotateVertex4d(cubeVerticesInit[i], rotation[0] * Mathf.PI / 180f, rotation[1] * Mathf.PI / 180f, rotation[2] * Mathf.PI / 180f);
        }

        Vector3[] cubeVerticesProjected = geom4d.ProjectPerspective(cubeVerticesCurrent, m_localScale[2]);

        shapeUtils.updateShape(cubeVerticesProjected, cubeEdges, cubeFaces, verticesShapeObj, edgesShapeObj, facesShapeObj, m_localScale);
    }

    IEnumerator example4Drotation()
    {
        Vector3 rotationAngle = Vector3.zero;
        float targetRotation = numRotations * 360f;

        while (rotationAngle[1] <= targetRotation || audioNotFinished)
        {
            updateShapeWithoutController(rotationAngle);

            rotationAngle[1] += 100f * Time.deltaTime;
            // controller.transform.rotation = Quaternion.Euler(rotationAngle);

            if (rotationAngle[1] > targetRotation && audioNotFinished) targetRotation += 360f;

            yield return null;
        }

        updateShapeWithoutController(rotationAngle);

        //controller.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        ap.progressLevel += 1;
        ap.cubeStageInProgress = false;

        yield return ap.cubeStageInProgress;
    }

    IEnumerator destroyController()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        controller.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        updateShapeFromController();

        Vector3 pos = controller.transform.localPosition;
        var smokeClone = Instantiate(smokePuff, pos, Quaternion.identity);
        float smokeDuration = 1.5f;
        Destroy(controller);
        Destroy(smokeClone, smokeDuration);

        yield return null;
    }
}
