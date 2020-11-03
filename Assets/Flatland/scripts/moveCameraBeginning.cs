using UnityEngine;
using System.Collections;

public class moveCameraBeginning : MonoBehaviour
{


    float cameraSpeed = 0.2F;

    float x0 = -4.0F;
    float y0 = 2.5F;
    float z0 = 0.5F;

    float xFinal = 0;
    float yFinal = 15;
    float zFinal = -10;
    Vector3 deltaVector;

    void Start()
    {
        // Compute the difference between final and inital position

        deltaVector = new Vector3(xFinal, yFinal, zFinal) - new Vector3(x0, y0, (float)(z0)) ;
    }

    void Update()
    {

        // Change the location
        if (Time.timeSinceLevelLoad < 4)
        {
            transform.position = new Vector3(x0, y0, (float)(z0)) + (Time.timeSinceLevelLoad * cameraSpeed) * deltaVector;
        }

        // It would be nice to also change the roatation



    }
}