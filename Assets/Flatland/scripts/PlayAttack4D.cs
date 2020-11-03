using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAttack4D : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource audioSource;
    private Subtitler subtitler;      //Script that makes subttiles

    public float audioLevel = 0.5f;
    private bool played = false;

    // Start is called before the first frame update
    void Start()
    {
        subtitler = this.GetComponent<Subtitler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > 0.1 && played == false && audioSource.isPlaying == false)
        {
            played = true;
            audioSource.PlayOneShot(audioClip, audioLevel);
            subtitler.play(0, audioClip.length);
        }
    }
}
