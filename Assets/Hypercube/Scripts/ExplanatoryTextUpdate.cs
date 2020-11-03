using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplanatoryTextUpdate : MonoBehaviour
{
    private int progressLevel;
    private TMPro.TextMeshPro textObject;

    // Start is called before the first frame update
    void Start()
    {
        textObject = this.GetComponentInChildren<TMPro.TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        progressLevel = GameObject.Find("Main Camera").GetComponent<appProgress>().progressLevel;

        if (progressLevel == 2)
        {
            textObject.SetText("Use your hands to touch and rotate the square. Show two thumbs up to move on to the cube.");
        }
        else if (progressLevel == 8)
        {
            textObject.SetText("Use your hands to touch and rotate the cube. Show two thumbs up to move on to 4D rotation.");
        }
        else if (progressLevel == 11)
        {
            textObject.SetText("Use your hands to touch and rotate the cube or controller. Show two thumbs up to move on to the hypercube.");
        }
        else if (progressLevel == 19)
        {
            textObject.SetText("Use your hands to touch and rotate the hypercube. Show two thumbs up to finish.");
        }
        else
        {
            textObject.SetText("");
        }
    }
}
