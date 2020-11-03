using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hypercubeAudioScript : MonoBehaviour
{
    private AudioManager AudioManager; //Play and stop audio files
    public GameObject PauseMenu;       //Object for pause menu. When logic is paused, we must stop playing audio
    private PauseScript PauseScript;   //The actual script that we need to check.
    private bool wasInterrupted;       //If the pause menu stopped an audio file, we need to restart it.
    private appProgress ap;            //Object for the progress through the app (an integer up to 22). This will determine which file to play/
    private int progressLevel;         //Derived from ap.
    private int fileToPlay;            //Which audio file to play by index, updated in a loop.
    private int lastFilePlayed;        //The last audio file we played, to prevent endless replaying of it.
    [HideInInspector]
    public int maxProgressLevel;       //Do not allow the program to progress past this level until audio has finished playing.
    [HideInInspector]
    public bool audioNotFinished;      //To be used in tandem with the above

    private readonly string[] fileNames =
    {
        "HC1 In front of you",
        "HC2 Since this square",
        "HC3 Now, let's take a look at a 3D cube",
        "HC4 We can think of the space inside",
        "HC5 In three dimensions",
        "HC6 Get a feel for them now",
        "HC7 What if, like the 2D square",
        "HC8 You can try this yourself",
        "HC9 The shape you can see in front of you is a four",
        "HC10 You can think of a hypercube as being two cubes",
        "HC11 Remember, because this is a projection",
        "HC12 How does the projection",
        "HC13 As before, you can try rotating"
    };

    // Start is called before the first frame update
    void Start()
    {
        AudioManager = this.gameObject.GetComponent<AudioManager>();
        PauseMenu = GameObject.Find("PauseMenu");
        PauseScript = PauseMenu.transform.GetComponent<PauseScript>();
        wasInterrupted = false;
        ap = GameObject.Find("Main Camera").GetComponent<appProgress>();
        progressLevel = ap.progressLevel;
        lastFilePlayed = -1;
        maxProgressLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        fileToPlay = -1;
        progressLevel = ap.progressLevel;

        // STAGE 0: Square appears on start 
        if (progressLevel == 0){
            fileToPlay = 0; //"HC1 In front of you"
            maxProgressLevel = 1;
        }

        // STAGE 1: Square rotates in plane - transition automatic
        else if (progressLevel == 1){
            fileToPlay = 0;
            maxProgressLevel = 1;
        }

        // STAGE 2: Square rotates in 3D and discuss projections
        else if (progressLevel == 2){
            fileToPlay = 1; //"HC2 Since this square",
            maxProgressLevel = 2;
        }

        // STAGE 3: Transition - cube appears - transition automatic
        else if (progressLevel == 3)
        {
            fileToPlay = -1; 
            maxProgressLevel = 5;
        }

        // STAGE 4: Transition - square moves and cube rotates into place - transition automatic
        else if (progressLevel == 4)
        {
            fileToPlay = -1;
            maxProgressLevel = 5;
        }

        // STAGE 5: highlight different faces of cube - transition automatic
        else if (progressLevel == 5)
        {
            fileToPlay = 2;  //"HC3 Now, let's take a look at a 3D cube",
            maxProgressLevel = 5;
        }

        // STAGE 6: draw plane filling space of cube and discuss projections - transition automatic
        else if (progressLevel == 6)
        {
            fileToPlay = 3; //"HC4 We can think of the space inside",
            maxProgressLevel = 6;
        }

        // STAGE 7: Cube 3D rotations animation - transition automatic
        else if (progressLevel == 7)
        {
            fileToPlay = 4; //"HC5 In three dimensions",
            maxProgressLevel = 7;
        }

        // STAGE 8: free playing moving cube around, introduce menu to reset orientation -- transition automatic on button press
        else if (progressLevel == 8)
        {
            if (!ap.m_skipInteraction) fileToPlay = 5; //"HC6 Get a feel for them now",
            maxProgressLevel = 8;
        }

        // STAGE 9: example rotation of cube in 4D 
        else if (progressLevel == 9)
        {
            fileToPlay = 6; // "HC7 What if, like the 2D square",
            maxProgressLevel = 9;
        }

        // STAGE 10: Make 4D controller for cube appear
        else if (progressLevel == 10)
        {
            if (!ap.m_skipInteraction) fileToPlay = 7; //"HC8 You can try this yourself",
            maxProgressLevel = 12;
        }

        // STAGE 11: free playing with 4D rotations of cube
        else if (progressLevel == 11)
        {
            if (!ap.m_skipInteraction) fileToPlay = 7;
            maxProgressLevel = 12;
        }

        // STAGE 12: Transition - disappear smoke and destroy cube controller, hypercube appears - transition automatic
        else if (progressLevel == 12)
        {
            fileToPlay = -1; //Don't want to play anything here
            maxProgressLevel = 12;
        }

        // STAGE 13: Transition - cube moves and hypercube rotates into place
        else if (progressLevel == 13)
        {
            fileToPlay = 8; // "HC9 The shape you can see in front of you is a four",
            maxProgressLevel = 13;
        }

        // STAGE 14: discuss projection and highlight different sub-cubes of hypercube - transition automatic
        else if (progressLevel == 14)
        {
            fileToPlay = 9; // "HC10 You can think of a hypercube as being two cubes
            maxProgressLevel = 14;
        }

        // STAGE 15: draw cube filling space of hypercube - transition automatic
        else if (progressLevel == 15)
        {
            fileToPlay = 10; // "HC11 Remember, because this is a projection",
            maxProgressLevel = 16;
        }

        // STAGE 16: -blank-
        else if (progressLevel == 16)
        {
            fileToPlay = -1; //Don't want to play anything here
            maxProgressLevel = 16;
        }

        // STAGE 17: Animate 4D rotation example - transition automatic
        else if (progressLevel == 17)
        {
            fileToPlay = 11; //"HC12 How does the projection"
            maxProgressLevel = 17;
        }

        // STAGE 18: Make 4D controller for hypercube appear - transition automatic
        else if (progressLevel == 18)
        {
            if (!ap.m_skipInteraction) fileToPlay = 12; // "HC13 As before, you can try rotating"
            maxProgressLevel = 22;
        }

        // STAGE 19: Free playing with rotations of hypercube - NB. encourage exploration of different fixed axis rotations first
        else if (progressLevel == 19)
        {
            fileToPlay = 11;
            maxProgressLevel = 22;
        }

        // STAGE 20: Finish scene - disappear smoke and destroy hypercube controller
        else if (progressLevel == 20)
        {
            fileToPlay = -1;
            maxProgressLevel = 22;
        }

        // STAGE 21: Finish scene - move all shapes back into view - transfer back to main menu at end of scene
        else if (progressLevel == 21)
        {
            fileToPlay = -1;
            maxProgressLevel = 100;
        }

        //Stop audio if application is paused
        if (PauseScript.isPaused) {
            if (AudioManager.isPlaying()){
                AudioManager.stop();
                wasInterrupted = true;
            }
        }

        //Play an audio file if required, and if we are not paused
        else if ((fileToPlay > lastFilePlayed) || (fileToPlay == lastFilePlayed && wasInterrupted)){
            AudioManager.play(fileToPlay);
            lastFilePlayed = fileToPlay;
            wasInterrupted = false;
        }

        if (!AudioManager.isPlaying() && !wasInterrupted) {
            audioNotFinished = false;
        }
        else{
            audioNotFinished = true;
        }
    }
}
