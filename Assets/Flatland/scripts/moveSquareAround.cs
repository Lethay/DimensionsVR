using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveSquareAround : MonoBehaviour
{

    public GameObject movingText;
    private Text text;
    private float MoveSpeed = 1.1f;


    //private GameObject movingTextObject;

    // Start is called before the first frame update
    void Start()
    {
        //movingTextObject = GameObject.Find("movingText");
    }

    // Update is called once per frame
    void Update()
    {


        if (Time.timeSinceLevelLoad > 30) //Starting moving after camera finished
        {

            if (Time.timeSinceLevelLoad < 31) // moving forward
            {
                transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
                // Change text
                text = movingText.GetComponent<Text>();
                text.text = "UP";

            }
            else if (Time.timeSinceLevelLoad < 32) //moving backwards
            {
                transform.Translate(-1 * Vector3.forward * MoveSpeed * Time.deltaTime);
                // Change text
                text = movingText.GetComponent<Text>();
                text.text = "DOWN";
            }
            else if (Time.timeSinceLevelLoad < 33) //moving left
            {
                transform.Translate(Vector3.left * MoveSpeed * Time.deltaTime);
                // Change text
                text = movingText.GetComponent<Text>();
                text.text = "LEFT";
            }
            else if (Time.timeSinceLevelLoad < 34) //moving right
            {
                transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
                // Change text
                text = movingText.GetComponent<Text>();
                text.text = "RIGHT";
            }
        }
    }
}
