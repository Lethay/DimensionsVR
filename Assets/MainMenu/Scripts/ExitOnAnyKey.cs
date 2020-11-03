using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitOnAnyKey: MonoBehaviour
{
    private KeyCode[] numRowCodes = {
        KeyCode.Alpha0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9
    };
    private KeyCode[] numPadCodes = {
        KeyCode.Keypad0,
        KeyCode.Keypad1,
        KeyCode.Keypad2,
        KeyCode.Keypad3,
        KeyCode.Keypad4,
        KeyCode.Keypad5,
        KeyCode.Keypad6,
        KeyCode.Keypad7,
        KeyCode.Keypad8,
        KeyCode.Keypad9
    };
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Load a scene if given a number
        for (int i = 0; i< SceneManager.sceneCountInBuildSettings; i++){
            if (Input.GetKey(numRowCodes[i]) || Input.GetKey(numPadCodes[i])){
                SceneManager.LoadScene(i);
                return;
            }
        }

        //Load the main menu if given any other key
        if (Input.anyKeyDown){
            SceneManager.LoadScene(0);
        }
    }
}
