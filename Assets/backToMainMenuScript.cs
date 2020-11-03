using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class backToMainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    // we execute this on button click (which is actually a collusion)
    void OnCollisionEnter()
    {
        changeScene();
    }
    // this is the function to change the scene
    void changeScene()
    {
        SceneManager.LoadScene(0);

    }
}
