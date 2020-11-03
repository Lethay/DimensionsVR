using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class moveCircleAround : MonoBehaviour
{

    public GameObject movingText;
    private Text text;

    public float MoveSpeedSquare = 1.1f;
    private GameObject movingTextObject;


    private float randomWalkerRange = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        movingTextObject = GameObject.Find("movingText");
    }

    // Update is called once per frame
    void Update()
    {


        if (Time.timeSinceLevelLoad > 40 && Time.timeSinceLevelLoad < 50) //Starting moving after moving triangle finished
        {
        
            // Change text
            text = movingText.GetComponent<Text>();
            text.text = "Combinations of these four directions are possible, too!";

            //transform.Translate(new Vector3(-0.5F, 0, 0.5F) * MoveSpeedSquare * Time.deltaTime);
            transform.Translate(new Vector3(Random.Range(-randomWalkerRange, randomWalkerRange), 0, Random.Range(-randomWalkerRange, randomWalkerRange)));

        }
    }
}
