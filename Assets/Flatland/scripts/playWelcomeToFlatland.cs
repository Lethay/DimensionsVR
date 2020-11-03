using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playWelcomeToFlatland : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioClip audioClip2;
    public AudioClip audioClip3;
    public AudioClip audioClip4;
    public AudioClip audioClip5;

    public AudioSource audioSource;
    private Subtitler subtitler;      //Script that makes subttiles

    public float audioLevel = 0.5f;

    private bool played1 = false;
    private bool played2 = false;
    private bool played3 = false;
    private bool played4 = false;
    private bool played5 = false;

    void Start()
    {
        // audioClip = Resources.Load("click") as AudioClip;
        //AudioClip audioClip = (AudioClip)Resources.Load("Sounds/explanation/1-welcomeToFlatland.mp3");

        //AudioClip audioClip2 = (AudioClip)Resources.Load("Sounds/explanation/2-populated.mp3");

        //audioSource = gameObject.AddComponent<AudioSource>();

        subtitler = this.GetComponent<Subtitler>();
        audioSource.PlayOneShot(audioClip, audioLevel);
        
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad > 0.1 && played1 == false)
        {
            subtitler.play(0, audioClip.length); //"Welcome to Flatland"
            played1 = true;
        }

        if (Time.timeSinceLevelLoad > 8 && played2== false && audioSource.isPlaying == false)
        {
            played2 = true;
            audioSource.PlayOneShot(audioClip2, audioLevel);
            subtitler.play(1, audioClip2.length); //"This land is populated by..."
        }

        if (Time.timeSinceLevelLoad > 19 && played3 == false && audioSource.isPlaying == false)
        {
            played3 = true;
            audioSource.PlayOneShot(audioClip3, audioLevel);
            subtitler.play(2, audioClip3.length); //"Flat penguin lives in his house"
        }


        if (Time.timeSinceLevelLoad > 30 && played4 == false && audioSource.isPlaying == false)
        {
            played4 = true;
            audioSource.PlayOneShot(audioClip4, audioLevel);
            subtitler.play(3, audioClip4.length); //"None of the flat objects can enter the house"
        }

        if (Time.timeSinceLevelLoad > 43 && played5 == false && audioSource.isPlaying == false)
        {
            played5 = true;
            audioSource.PlayOneShot(audioClip5, audioLevel);
            subtitler.play(4, audioClip5.length); //"Now, let's assume that a strange three-dimensional object"
        }


        /// Change Scene
        /// 


        if (Time.timeSinceLevelLoad > 90 && audioSource.isPlaying == false)
        {
            SceneManager.LoadScene(3);

        }

        




    }



}
