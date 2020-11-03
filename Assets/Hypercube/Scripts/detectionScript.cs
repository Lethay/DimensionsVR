using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class detectionScript : MonoBehaviour
{
    private int progressLevel;
    public GameObject activeShape;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        progressLevel = GameObject.Find("Main Camera").GetComponent<appProgress>().progressLevel;

        activeShape = GameObject.Find("Main Camera").GetComponent<appProgress>().activeShape;

        FingerDirectionDetector[] detectors = transform.Find("indexPoint").GetComponents<FingerDirectionDetector>();
        foreach (FingerDirectionDetector detector in detectors)
        {
            detector.TargetObject = activeShape.transform;
        }

    }

    public void thumbsUp()
    {
        if (progressLevel == 8 || progressLevel == 16)                        controllerAppear();
        if (progressLevel == 2 || progressLevel == 11 || progressLevel == 19) nextStage();
    }

    public void palmsOut()
    {
        return;
    }

    public void controllerAppear()
    {
        if (progressLevel == 8 || progressLevel == 16)
        {
            GameObject.Find("Main Camera").GetComponent<appProgress>().progressLevel += 1;
        }
    }

    public void nextStage()
    {
        if (progressLevel == 2 || progressLevel == 11 || progressLevel == 19)
        {
            GameObject.Find("Main Camera").GetComponent<appProgress>().progressLevel += 1;
        }
    }
}
