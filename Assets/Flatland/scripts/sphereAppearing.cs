using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sphereAppearing : MonoBehaviour
{
    public float a = 0.1f; //a is in AU, Semimajor Axis
    private float angle = 0; // angle theta
    public float speed;// = (2 * Mathf.PI) / 200;
    public float reductionRate;// = 0.5f;
    float x;
    float y;
    float e = 0.8f; // Eccentricity
    float reduction = 1.0f;
    bool sinking = false;
    bool scaling = false;


    public GameObject movingText;
    private Text text;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (Time.timeSinceLevelLoad > 40)
        {

            text = movingText.GetComponent<Text>();
            text.text = "A three-dimensional sphere appears in flatland!";

            // make the sphere float in a nice curve
            float b = Mathf.Sqrt(Mathf.Pow(a, 2) * (1 - Mathf.Pow(e, 2))); // Finding the Semiminor Axis (sqrt);
            angle += speed * Mathf.PI * Time.deltaTime;
            x = 20000 * Mathf.Cos(angle) * a; // a is the Radius in the x direction
            y = 20000 * Mathf.Sin(angle) * b; // b is the Radius in the y direction

            reduction *= 1-reductionRate*Time.deltaTime;

            //Make the sphere fall downwards into the house
            if (sinking == false)
            {
                transform.position = new Vector3(1.5f, 5, 0f) + new Vector3((float)(reduction * x), 0, (float)(reduction * y));

                if (Mathf.Sqrt(Mathf.Pow((float)(gameObject.transform.position.x - 1.5), 2) + Mathf.Pow(gameObject.transform.position.z, 2)) < 0.02)
                {
                    sinking = true; // we let gravity do the sinking
                    gameObject.GetComponent<TrailRenderer>().enabled = false;
                    //gameObject.GetComponent<ParticleSystem>().enableEmission = false; // switch particle trail off
                }
            }

            //Flatten the sphere into a circle
            if (sinking == true && transform.position.y < 0.8 && transform.localScale.y > 0.001)
            {
                transform.localScale = new Vector3(1, (float)(0.99 * transform.localScale.y), 1);
                //transform.localScale = new Vector3(1, (float)(0.0001*transform.localScale.x), 1);

            }

            // if the sphere is flat enough (a circle); make it change in size
            if (scaling == false && transform.localScale.y < 0.05 && transform.localScale.x > 0.4)
            {
                StartCoroutine(Scale(transform.localScale.x, transform.localScale.y, transform.localScale.z));
                scaling = true;
            }

        }




    }

    IEnumerator Scale(float x, float y, float z)
    {
        //This routine will change the scale of x and z between 1% and 100% of its value over time, 
        // according to a sin^2 curve.
        float timer = 0;
        float sin   = 0;
        float xa = 0.01f*x, xb = 0.99f*x, ya = 0.01f*y, yb = 0.99f*y, za = 0.01f*z, zb = 0.99f*z;
   
        while (true)
        {
            timer += Time.deltaTime;
            sin = Mathf.Pow(Mathf.Sin(Mathf.PI*(0.5f + speed * timer)), 2);
            transform.localScale = new Vector3(
                xa + xb*sin, 
                y,
                za + zb*sin
            );
            yield return null;
        }
    }
}