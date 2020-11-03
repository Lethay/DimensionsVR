using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Leap.Unity.Interaction;

public class toggleRotationScript : MonoBehaviour {

    private InteractionButton button;
    public Color normalColour;
    public Color fixedColour;
    public Color disabledColour;
    private Color targetColour;

    public GameObject otherToggle1;
    public GameObject otherToggle2;
    public GameObject resetOrientationHolder;

    public string fixed_rotation;
    string activeShapeName;

    private RigidbodyConstraints constraints;

    public bool isActivated;
    private int progressLevel;
    Vector3 parentOriginalPosition, parentHiddenPosition;
    Transform parentTransform;

    void Start()
    {
        if (string.Compare(fixed_rotation, "X", true) == 0)
        {
            constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        else if (string.Compare(fixed_rotation, "Y", true) == 0)
        {
            constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        else if (string.Compare(fixed_rotation, "Z", true) == 0)
        {
            constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }

        if(resetOrientationHolder == null)
        {
            resetOrientationHolder = this.transform.parent.parent.Find("Reset orientation").gameObject;
        }

        parentTransform = transform.parent.transform;
        parentOriginalPosition = parentTransform.localPosition;
        parentHiddenPosition = parentTransform.localPosition + new Vector3(0, 0, 0.09f);
    }

    void Update()
    {
        progressLevel = GameObject.Find("Main Camera").GetComponent<appProgress>().progressLevel;

        if (progressLevel == 11 || progressLevel == 19)
        {
            button.controlEnabled = true;
        }
        else
        {
            button.controlEnabled = false;
        }

        if (button.controlEnabled)
        {
            if (isActivated)
            {
                targetColour = fixedColour;
            }
            else
            {
                targetColour = normalColour;
            }

            Color textColor = transform.parent.transform.GetComponentInChildren<TMPro.TextMeshPro>().color;
            textColor.a = 255 / 255f;
            transform.parent.transform.GetComponentInChildren<TMPro.TextMeshPro>().color = textColor;

            if (parentTransform.localPosition[2] != parentOriginalPosition[2])
                parentTransform.localPosition = Vector3.MoveTowards(parentTransform.localPosition, parentOriginalPosition, 4f * Time.deltaTime);

        }
        else
        {
            targetColour = disabledColour;

            Color textColor = transform.parent.transform.GetComponentInChildren<TMPro.TextMeshPro>().color; //transform.parent.transform.Find("Text").gameObject.GetComponent<Text>().color;
            textColor.a = 0 / 255f;
            transform.parent.transform.GetComponentInChildren<TMPro.TextMeshPro>().color = textColor;

            if (parentTransform.localPosition[2] != parentHiddenPosition[2])
                parentTransform.localPosition = Vector3.MoveTowards(parentTransform.localPosition, parentHiddenPosition, 4f * Time.deltaTime);
        }

        //Material buttonMaterial = button.transform.Find("Button Cube").transform.Find("Cube").GetComponent<Renderer>().material;
        Material buttonMaterial = this.transform.Find("FrontCube").GetComponent<Renderer>().material;
        buttonMaterial.color = Color.Lerp(buttonMaterial.color, targetColour, 30F * Time.deltaTime);
    }

    private void OnEnable()
    {
        button = this.GetComponent<InteractionButton>(); //this.transform.Find("Cube UI Button").GetComponent<InteractionButton>();
        isActivated = false;
        
    }   

    public GameObject findController(){
        activeShapeName = GameObject.Find("Main Camera").GetComponent<appProgress>().activeShapeName;
        string controllerName = activeShapeName + " 4D controller";
        GameObject controller = GameObject.Find(controllerName).gameObject;

        return controller;
    }

    public void activate(){
        GameObject controller = findController();
        activate(controller);
    }
    public void activate(GameObject controller){
        isActivated = true;
        controller.transform.Find("Cylinder_" + fixed_rotation).GetComponent<Renderer>().material.color = fixedColour;
    }

    public void deactivate(){
        GameObject controller = findController();
        deactivate(controller);
    }
    public void deactivate(GameObject controller){
        isActivated = false;
        controller.transform.Find("Cylinder_" + fixed_rotation).GetComponent<Renderer>().material.color = normalColour;
    }
    public void fixRotations()
    {
        GameObject controller = findController();

        //if we touched a button that is off:
        if (!isActivated){
            activate(controller);
            otherToggle1.GetComponent<toggleRotationScript>().deactivate(controller);
            otherToggle2.GetComponent<toggleRotationScript>().deactivate(controller);
            controller.GetComponent<Rigidbody>().constraints = constraints;
            resetControllerOrientation();
        }

        //If we touched a button that is already on
        else{
            deactivate(controller);
            controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void resetControllerOrientation()
    {
        if (resetOrientationHolder == null)
        {
            Debug.LogWarning("toggleRotationScript: resetOrientationHolder is null");
            return;
        }

        GameObject activeShape = GameObject.Find("Main Camera").GetComponent<appProgress>().activeShape;
        Quaternion rot = activeShape.transform.rotation;
        resetOrientationHolder.GetComponent<resetOrientationScript>().resetControllerOrientation(unlockAxes: false);
        //setOrientationHolder.GetComponent<resetOrientationScript>().resetShapeOrientation();
        activeShape.transform.rotation = rot;
    }
}
