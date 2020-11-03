using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTheCylinder : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Vector3.right * 50 * Time.deltaTime, Space.Self);

        UnityEngine.Debug.LogFormat("time: {0} ", Time.timeSinceLevelLoad);

        //UnityEngine.Debug.LogFormat("time: {0} ", timesForEvents[1]);

        // rotating of the cylinder
        if (Time.timeSinceLevelLoad > 4 && transform.rotation.x<0)
        {
            //transform.localScale += growSpeed * new Vector3(1, 1, 1); ;
            transform.Rotate(Vector3.right * 1, Space.Self);
            transform.localScale = new Vector3(1, 1, (float)(1.1 * transform.localScale.z)); // also have to grow it
        }



        // grow it to create a 2D object
        if (transform.rotation.x >= 0 && transform.localScale.x<20 && Time.timeSinceLevelLoad > 20) { 
            transform.localScale = new Vector3((float)(1.01 * transform.localScale.x), 1, (float)(0.97 * transform.localScale.z));
        }

    }
}
// gameObject.transform.position.x 