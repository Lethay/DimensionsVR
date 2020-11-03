using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class movePenguinInHouse : MonoBehaviour
{


    public GameObject movingText;
    private Text text;

    public int lastStep = 0; // 0 if left step, 1 if right step
    private int updatesToSkip = 50;
    private int updatesToSkipCounter = 0;

    public float rotationAnglePenguin = 1.0F;
    public float MoveSpeedPenguin = 0.1F;



    // Start is called before the first frame update
    void Start()
    {
        


    }


    // Update is called once per frame
    void Update()
    {
        //Debug.LogFormat("Penguin Speed {0}", MoveSpeedPenguin);

        if (Time.timeSinceLevelLoad > 12 && Time.timeSinceLevelLoad < 22) //Starting moving after moving triangle finished
        {

            // Change text
            text = movingText.GetComponent<Text>();
            text.text = "The penguin is moving into its house!";

            updatesToSkipCounter += 1; // Coutner such that we don't execute at every update


            // Left Step
            if (lastStep == 0 && updatesToSkipCounter> updatesToSkip)
            {
                leftStep();
                lastStep = 1;
                updatesToSkipCounter = 0;
            }

            // Right Step
            if (lastStep == 1 && updatesToSkipCounter > updatesToSkip)
            {
                rightStep();
                lastStep = 0;
                updatesToSkipCounter = 0;
            }
        }


        // define functions
        void leftStep()
        {
            transform.Translate(Vector3.forward * MoveSpeedPenguin * Time.deltaTime);

            transform.Rotate(0, rotationAnglePenguin, 0);
        }
        void rightStep()
        {
            transform.Translate(Vector3.forward * MoveSpeedPenguin * Time.deltaTime);
            transform.Rotate(0, -rotationAnglePenguin, 0);
        }



    }
}