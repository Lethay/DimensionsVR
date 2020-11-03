using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.Interaction;

public class resetOrientationScript : MonoBehaviour {

    private InteractionButton button;
    private int progressLevel;
    public GameObject toggle1;
    public GameObject toggle2;
    public GameObject toggle3;
    string activeShapeName;

    void Start()
    {
        button = this.GetComponent<InteractionButton>(); //this.transform.Find("Cube UI Button").GetComponent<InteractionButton>();
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

    public void resetShapeOrientation()
    {
        GameObject activeShape = GameObject.Find("Main Camera").GetComponent<appProgress>().activeShape;
        activeShape.transform.rotation = Quaternion.Euler(0f,0f,0f);
        Rigidbody rb = activeShape.GetComponent<Rigidbody>();
        rb.angularVelocity = new Vector3 (0,0,0);
    }

    public void resetControllerOrientation(bool unlockAxes=true)
    {
        if (progressLevel == 11 || progressLevel == 19)
        {
            activeShapeName = GameObject.Find("Main Camera").GetComponent<appProgress>().activeShapeName;
            string controllerName = activeShapeName + " 4D controller";
            GameObject controller = GameObject.Find(controllerName).gameObject;

            controller.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Rigidbody rb = controller.GetComponent<Rigidbody>();
            rb.angularVelocity = new Vector3(0, 0, 0);

            if (unlockAxes)
            {
                toggle1.GetComponent<toggleRotationScript>().deactivate(controller);
                toggle2.GetComponent<toggleRotationScript>().deactivate(controller);
                toggle3.GetComponent<toggleRotationScript>().deactivate(controller);

            }
        }
    }
}
