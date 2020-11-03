using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.Interaction;

public class toggleFacesScript : MonoBehaviour {

    private InteractionButton button;
    private int progressLevel;

    void Start()
    {
        button = this.GetComponent<InteractionButton>();  //this.transform.Find("Cube UI Button").GetComponent<InteractionButton>();
    }

    void Update()
    {
        progressLevel = GameObject.Find("Main Camera").GetComponent<appProgress>().progressLevel;

        if (progressLevel == 8 || progressLevel == 11 || progressLevel == 16 || progressLevel == 19)
        {
            button.controlEnabled = true;
        }
        else
        {
            button.controlEnabled = false;
        }
    }

    public void toggleFaces()
    {
        GameObject activeShape = GameObject.Find("Main Camera").GetComponent<appProgress>().activeShape;
        activeShape.transform.Find("Faces").gameObject.SetActive(!activeShape.transform.Find("Faces").gameObject.activeSelf);
    }
}
