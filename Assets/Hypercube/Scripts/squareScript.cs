using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class squareScript : MonoBehaviour {

    public GameObject verticesShapeObj;
    public GameObject edgesShapeObj;
    public GameObject facesShapeObj;
    public shapeUtils shapeUtils;
    public Vector3 squareStartPos;
    public Vector3 squareEndPos;
    public Vector3 squareFinalScenePos;

    private appProgress ap;
    private int progressLevel;

    public hypercubeAudioScript hypercubeAudioScript;
    private int maxProgressLevel;
    private bool audioNotFinished;

    public int numRotations;

    private Vector3 m_localScale;
    private InteractionBehaviour m_interactionBehaviour;

    public readonly Vector3[] squareVerticesInit = new Vector3[4]
    {
        new Vector3(-1,-1,0),
        new Vector3(-1,1,0),
        new Vector3(1,-1,0),
        new Vector3(1,1,0)
    };

    public readonly int[,] squareEdges = new int[4,2]
    {
        {0,1},
        {0,2},
        {1,3},
        {2,3}
    };

    public readonly int[,] squareFace = new int[1, 4]
    {
        {0,1,2,3}
    };

    // Use this for initialization
    void Start () {
        //Get references to other objects
        ap = GameObject.Find("Main Camera").GetComponent<appProgress>();
        progressLevel = ap.progressLevel;
        if (hypercubeAudioScript == null)
        {
            hypercubeAudioScript = GameObject.Find("Dialogue").GetComponent<hypercubeAudioScript>();
        }

        //Stop here if we dont want to use the square
        if (ap.m_skipSquareAndCube)
            return;

        //Define scane for this object and spawn it
        shapeUtils = transform.parent.GetComponent<shapeUtils>();
        m_localScale = this.transform.localScale;

        for (int i = 0; i < 4; i++)
            for(int j=0; j<3; j++) squareVerticesInit[i][j] *= m_localScale[j];

        shapeUtils.createShape(squareVerticesInit, squareEdges, squareFace, this.gameObject, m_localScale);
        Rigidbody Rbdy = this.gameObject.AddComponent<Rigidbody>();
        Rbdy.useGravity = false;
        Rbdy.constraints = RigidbodyConstraints.FreezePosition;

        verticesShapeObj = this.transform.Find("Vertices").gameObject;
        edgesShapeObj = this.transform.Find("Edges").gameObject;
        facesShapeObj = this.transform.Find("Faces").gameObject;

        this.transform.localPosition = squareStartPos;

    }
	
	// Update is called once per frame
	void Update () {
        progressLevel = ap.progressLevel;
        maxProgressLevel = hypercubeAudioScript.maxProgressLevel;
        audioNotFinished = hypercubeAudioScript.audioNotFinished;
        if (ap.m_skipSquareAndCube)
            return;
        m_localScale = this.transform.localScale;

        if (progressLevel == 0)
        {
            this.gameObject.AddComponent<InteractionBehaviour>();
            m_interactionBehaviour = this.gameObject.GetComponent<InteractionBehaviour>();
            m_interactionBehaviour.manager = GameObject.Find("Interaction Manager").GetComponent<InteractionManager>();
            m_interactionBehaviour.enabled = false;
            ap.handAttachments.SetActive(false);
            ap.progressLevel++;
        }

        // STAGE 1: rotate square in plane
        if (progressLevel == 1 && !ap.squareStageInProgress && maxProgressLevel>=1)
        {
            StartCoroutine(planarRotation());
            ap.squareStageInProgress = !ap.squareStageInProgress;            
        }

        // STAGE 2: rotate in 3D and discuss projections
        if (progressLevel == 2 && !ap.squareStageInProgress && maxProgressLevel >= 1)
        {
            StartCoroutine(rotation3D());
            ap.squareStageInProgress = !ap.squareStageInProgress;
            m_interactionBehaviour.enabled = true;
        }        

        //STAGE 4: transition stage -move plane and make cube appear
        if (progressLevel == 4 && maxProgressLevel >= 4)
        {
            m_interactionBehaviour.enabled = false;
            Vector3 rotationPoint = new Vector3(0f, squareStartPos[1], squareEndPos[2]);
            shapeUtils.RotateTowardsTarget(transform, rotationPoint, squareStartPos, squareEndPos, Vector3.up, -10f, 5f);
        }

        if (progressLevel >= 4 && maxProgressLevel >= 4)
        {
            transform.Rotate(100f * Vector3.right * Time.deltaTime);
        }

        // STAGE 21: Finish scene - move square back into view
        if (progressLevel == 21 && maxProgressLevel >= 21)
        {                                   
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, squareFinalScenePos, 4f * Time.deltaTime);

            float x = System.Math.Abs(transform.localPosition[0] - squareFinalScenePos[0]),
                  y = System.Math.Abs(transform.localPosition[1] - squareFinalScenePos[1]),
                  z = System.Math.Abs(transform.localPosition[2] - squareFinalScenePos[2]),
            delta = 4f * Time.deltaTime;

            if (x < delta && y < delta && z < delta)
            {
                StartCoroutine(wait(1f));
                ap.progressLevel++;
            }
        }

        // STAGE 22: Exit. On Hypercube script.

    }

    //Coroutines
    IEnumerator planarRotation()
    {
        float rotationAngle = 0f;
        float targetRotation = numRotations * 360f;

        yield return new WaitForSecondsRealtime(2);

        while (rotationAngle <= targetRotation || audioNotFinished)
        {
            rotationAngle += 100f * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f,0f,rotationAngle);

            if (rotationAngle > targetRotation && audioNotFinished) targetRotation += 180f;
            
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        yield return new WaitForSecondsRealtime(1);
        
        ap.progressLevel++;
        ap.squareStageInProgress = false;
        
        yield return ap.squareStageInProgress;
    }

    IEnumerator rotation3D()
    {
        float rotationAngle = 0f;
        float targetRotation = numRotations * 360f;

        while (rotationAngle <= targetRotation || audioNotFinished)
        {
            rotationAngle += 100f * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rotationAngle, 0f, 0f);

            if (rotationAngle > targetRotation && audioNotFinished) targetRotation += 180f;

            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        yield return new WaitForSecondsRealtime(1);
        if (ap.m_skipInteraction) ap.progressLevel++;

        yield return ap.squareStageInProgress;
    }

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
