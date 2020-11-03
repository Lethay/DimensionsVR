using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [HideInInspector] public bool isPaused = false;
    private GameObject pauseMenuObject;
    private GameObject pauseCanvasObject;

    public GameObject shapes;
    public GameObject[] lights;
    private float[] originalIntensities;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuObject = this.gameObject;
        pauseCanvasObject = this.transform.Find("PauseCanvas").gameObject;
        pauseCanvasObject.SetActive(false);

        if (lights.Length > 0)
        {
            originalIntensities = new float[lights.Length];
            for (int i = 0; i < lights.Length; i++) originalIntensities[i] = lights[i].transform.GetComponent<Light>().intensity;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pauseScene()
    {
        isPaused = true;

        //Stop animations and physics
        //Time.timeScale = 0;

        //Hide shapes
        if (shapes == null) {
            Debug.LogWarning("PauseScript: shapes is null, so cannot hide them from view.");
        }
        else shapes.SetActive(false);

        //Dim lights
        for (int i = 0; i < lights.Length; i++){
            lights[i].transform.GetComponent<Light>().intensity = 0;
        }

        //Show pause menu
        pauseCanvasObject.SetActive(true);
    }

    public void unpauseScene()
    {
        isPaused = false;

        //Stop animations and physics
        //Time.timeScale = 1;

        //Show shapes
        if (shapes != null) shapes.SetActive(true);

        //Reset lights
        for (int i = 0; i < lights.Length; i++){
            lights[i].transform.GetComponent<Light>().intensity = originalIntensities[i];
        }

        //Hide pause menu
        pauseCanvasObject.SetActive(false);
    }
}
