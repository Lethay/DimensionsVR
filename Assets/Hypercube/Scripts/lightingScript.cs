using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightingScript : MonoBehaviour {

    private int progressLevel;
    private Light spotlight;

	// Use this for initialization
	void Start () {

        progressLevel = GameObject.Find("Main Camera").GetComponent<appProgress>().progressLevel;

        spotlight = this.transform.Find("Spotlight").GetComponent<Light>();

        if (progressLevel == 0)
        {
            spotlight.enabled = false;
        }        

    }
	
	// Update is called once per frame
	void Update () {

        progressLevel = GameObject.Find("Main Camera").GetComponent<appProgress>().progressLevel;

        if (progressLevel == 3 || progressLevel == 8 || progressLevel == 10)
        {
            spotlight.enabled = true;
        }
        else
        {
            spotlight.enabled = false;
        }

    }
}
