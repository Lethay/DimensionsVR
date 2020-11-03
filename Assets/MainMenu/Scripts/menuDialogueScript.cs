using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuDialogueScript : MonoBehaviour
{
    private AudioManager AudioManager; //Play and stop audio files
    private bool havePlayed = false;   //Prevent us playing the audio file twice (unless we leave the scene)
    private bool waitOver = false;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager = this.gameObject.GetComponent<AudioManager>();
        havePlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!havePlayed && !AudioManager.isPlaying())
        {
            StartCoroutine(wait(2f));
            if (waitOver) { 
                AudioManager.play(0);
                havePlayed = true;
            }
        }
    }

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        waitOver = true;
        yield return waitOver;
    }
}
