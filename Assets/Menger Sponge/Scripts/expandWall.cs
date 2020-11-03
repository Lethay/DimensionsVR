using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expandWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > 20)
        { 
            if (transform.localScale.x < 1.8)
        {
            // grow the wall
            transform.localScale += new Vector3((float)(0.1), 0, 0);
        }


            // move the wall
            if (transform.position.z < 0.6)
            {
               
                transform.position += new Vector3(0, 0, (float)(0.1));
            }

        }
    }
}
