using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuScript : MonoBehaviour {

    public GameObject sceneCamera;
    public float menuActiveAngle;
    public float menuInactiveAngle;
    public float menuFinalAngle;
    float menuStartAngle;
    public float menuSpeed;

	// Use this for initialization
	void Start () {        

        menuStartAngle = transform.eulerAngles.x;        

	}
	
	// Update is called once per frame
	void Update () {
        
        float cameraOrientation = sceneCamera.transform.eulerAngles.x;        

        if (cameraOrientation < menuActiveAngle || cameraOrientation > 90f)
        {
            if (this.transform.eulerAngles.x < menuStartAngle)
            {
                transform.RotateAround(sceneCamera.transform.localPosition, transform.right, menuSpeed * Time.deltaTime);
            }
        }
        else
        {
            this.transform.Find("Menu buttons").gameObject.SetActive(true);

            if (this.transform.eulerAngles.x > menuFinalAngle)
            {
                transform.RotateAround(sceneCamera.transform.localPosition, transform.right, - menuSpeed * Time.deltaTime);
            }

        }

        if (cameraOrientation < menuInactiveAngle || cameraOrientation > 90f)
        {
            this.transform.Find("Menu buttons").gameObject.SetActive(false);
        }


    }   

}
