using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class appProgress : MonoBehaviour {

    //!!!TO DO!!! check interactions of hands with shapes when doing rotations, shield around shapes when animating?, voice-overs   
    //!!!TO DO!!! link up exit scene button to main menu and automatically return to main menu at end of scene

    // App stages
    // STAGE 0: Square appears on start 
    // STAGE 1: Square rotates in plane - transition automatic
    // STAGE 2: Square rotates in 3D and discuss projections (and free playing with square)
    //
    // -- UI to initiate transition --
    // STAGE 3: Transition - cube appears - transition automatic
    // STAGE 4: Transition - square moves and cube rotates into place - transition automatic
    //
    // STAGE 5: highlight different faces of cube - transition automatic
    // STAGE 6: draw plane filling space of cube and discuss projections - transition automatic
    // STAGE 7: Cube 3D rotations animation - transition automatic
    // STAGE 8: free playing moving cube around, introduce menu to reset orientation -- transition automatic on button press
    //
    // -- UI to initiate stage 8: controller appearing --
    // STAGE 9: example rotation of cube in 4D 
    // STAGE 10: Make 4D controller for cube appear
    // STAGE 11: free playing with 4D rotations of cube
    //
    // -- UI to initiate transition --
    // STAGE 12: Transition - disappear smoke and destroy cube controller, hypercube appears - transition automatic
    // STAGE 13: Transition - cube moves and hypercube rotates into place
    //
    // STAGE 14: discuss projection and highlight different sub-cubes of hypercube - transition automatic
    // STAGE 15: draw cube filling space of hypercube - transition automatic
    // STAGE 16: -blank-
    // STAGE 17: Animate 4D rotation example - transition automatic
    // STAGE 18: Make 4D controller for hypercube appear - transition automatic
    // STAGE 19: Free playing with rotations of hypercube - NB. encourage exploration of different fixed axis rotations first
    //
    // -- UI to initiate final transition -- 
    // STAGE 20: Finish scene - disappear smoke and destroy hypercube controller
    // STAGE 21: Finish scene - move all shapes back into view - transfer back to main menu at end of scene

    public int progressLevel;
    public bool skipSquareAndCube;
    public bool skipInteraction;   //We will use this in other scripts to make all stages automatic (i.e. skip free-playing stages)
    [HideInInspector] public bool m_skipInteraction;   //We will use this in other scripts to make all stages automatic (i.e. skip free-playing stages)
    [HideInInspector] public bool m_skipSquareAndCube; //We will use this in other scripts, so that the user CAN'T uncheck the above box mid-simulation and break the program logic
    [HideInInspector] public bool squareStageInProgress;
    [HideInInspector] public bool cubeStageInProgress;
    [HideInInspector] public bool hypercubeStageInProgress;

    public GameObject PauseMenu;
    [HideInInspector] public int activeShapeIndex;
    [HideInInspector] public string activeShapeName;
    [HideInInspector] public GameObject activeShape;
    private string[] shapeNames;
    private GameObject[] shapes;
    [HideInInspector] public squareScript squareScript;
    [HideInInspector] public cubeScript cubeScript;
    [HideInInspector] public hypercubeScript hypercubeScript;
    [HideInInspector] public GameObject handAttachments;

    // Use this for initialization
    void Start () {
        m_skipSquareAndCube = skipSquareAndCube;
        m_skipInteraction = skipInteraction;
        progressLevel = (m_skipSquareAndCube) ? 12 : 0; //start at hypercube if skipping first two stages, else start at beginning

        PauseMenu       = GameObject.Find("PauseMenu");
        handAttachments = GameObject.Find("Attachment Hands");
        handAttachments.SetActive(false);

        shapeNames = new string[3];
        shapes = new GameObject[3];

        shapeNames[0] = "Square";
        shapeNames[1] = "Cube";
        shapeNames[2] = "Hypercube";

        for (int i = 0; i < 3; i++) shapes[i] = GameObject.Find(shapeNames[i]).gameObject;

        squareScript =    shapes[0].GetComponent<squareScript>();
        cubeScript =      shapes[1].GetComponent<cubeScript>();
        hypercubeScript = shapes[2].GetComponent<hypercubeScript>();

        activeShapeIndex = 0;
        setActiveShape(); 

	}
	
	// Update is called once per frame
	void Update () {

        setActiveShape();

        if (PauseMenu.transform.GetComponent<PauseScript>().isPaused){
            if (cubeStageInProgress) { 
                if (cubeScript.highlightedFace != null)                       Destroy(cubeScript.highlightedFace);
                if (progressLevel == 10 && cubeScript.controller != null)      Destroy(cubeScript.controller);
            }

            if (hypercubeStageInProgress) { 
                if (hypercubeScript.highlightedCube != null)                    Destroy(hypercubeScript.highlightedCube);
                if (progressLevel == 18 && hypercubeScript.controller != null)  Destroy(hypercubeScript.controller);
            }

            squareStageInProgress = false;
            cubeStageInProgress = false;
            hypercubeStageInProgress = false;

            return;
        }
    }

    public bool stageInProgress()
    {
        return squareStageInProgress || cubeStageInProgress || hypercubeStageInProgress;
    }

    private void setActiveShape()
    {
        if (progressLevel < 3)
        {
            activeShapeIndex = 0;
        }
        else if (progressLevel < 12)
        {
            activeShapeIndex = 1;
        }
        else
        {
            activeShapeIndex = 2;
        }

        activeShapeName = shapeNames[activeShapeIndex];
        activeShape     = shapes[activeShapeIndex];
        // activeScript     = scripts[activeShapeIndex];
    }
}
