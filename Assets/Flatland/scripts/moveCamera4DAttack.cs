using UnityEngine;
using System.Collections;

public class moveCamera4DAttack : MonoBehaviour
{


    float cameraSpeed = 0.1F;


    float x0 = 50.0F;
    float y0 = 5.5F;
    float z0 = 26.5F;


    float xFinal = 12.2F;
    float yFinal = 0.5F;
    float zFinal = 5.5F;


    Vector3 deltaVector;

    void Start()
    {
        // Compute the difference between final and inital position
        deltaVector = new Vector3(xFinal, yFinal, zFinal) - new Vector3(x0, y0, (float)(z0));
        // set position to initial position
        transform.position = new Vector3(x0, y0, (float)(z0));
    }

    void Update()
    {
        //Debug.Log("TEST DEBUG HERE");
        // Change the location
        if (gameObject.transform.position.x > xFinal)
        {
            transform.position = new Vector3(x0, y0, (float)(z0)) + (Time.timeSinceLevelLoad * cameraSpeed) * deltaVector;
        }


        // It would be nice to also change the rotation
        if (gameObject.transform.position.x <= xFinal  && gameObject.transform.eulerAngles.x >= 315)
        {
            transform.Rotate(-0.5F, 0, 0);
        }


    }
}