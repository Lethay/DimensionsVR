using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grow4DSphere : MonoBehaviour
{

    private float sizeFluctuation = 0.01f;
    private float growSpeed = 0.010f;


    private int updatesToSkip = 100;
    private int updatesToSkipCounter = 0;
    private int randomOffsetTime = 0;

    private float randomFluctuation = 0.001F;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // growing of the sphere
        if (Time.timeSinceLevelLoad > 12 && Time.timeSinceLevelLoad < 16)
        {
            transform.localScale += growSpeed * new Vector3(1, 1, 1); ;
        }






        // random fluctuation of sphere size
        if (Time.timeSinceLevelLoad > 16)
        {   



            updatesToSkipCounter += 1 + randomOffsetTime; // Coutner such that we don't execute at every update; random create non uniform periods
            if (updatesToSkipCounter > updatesToSkip)
            {

                randomFluctuation = Random.Range(-sizeFluctuation, sizeFluctuation);
                updatesToSkipCounter = 0;
                randomOffsetTime = Random.Range(0, 2); // some random time
            }
            transform.localScale += randomFluctuation*new Vector3(1, 1, 1);

        }

    }
}
