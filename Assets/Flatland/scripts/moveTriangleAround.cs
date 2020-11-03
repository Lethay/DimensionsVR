using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveTriangleAround : MonoBehaviour
{

    public GameObject movingText;
    private Text text;

    public float MoveSpeed = 0.1f;
    private GameObject movingTextObject;

    // Start is called before the first frame update
    void Start()
    {
        movingTextObject = GameObject.Find("movingText");
    }

    // Update is called once per frame
    void Update()
    {


        if (Time.timeSinceLevelLoad > 35) //Starting moving after moving square finished
        {


            if (Time.timeSinceLevelLoad < 37) // move 
            {
                // Change text
                text = movingText.GetComponent<Text>();
                text.text = "Objects can rotate in Flatland!";
                // the actual movement
                //transform.Translate(new Vector3(-1, 0, -1) * MoveSpeed * Time.deltaTime);
                transform.Rotate(0, -0.1F, 0, Space.Self);
            }
            else if(Time.timeSinceLevelLoad < 39) // move backwards
            {
                //transform.Translate(new Vector3(1, 0, 1) * MoveSpeed * Time.deltaTime);
                transform.Rotate(0, +0.6F, 0, Space.Self);
            }

        }
    }
}
