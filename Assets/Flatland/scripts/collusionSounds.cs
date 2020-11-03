using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collusionSounds : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource audioSource;

    void Start()
    {
        // audioClip = Resources.Load("click") as AudioClip;
        AudioClip audioClip = (AudioClip)Resources.Load("Sounds/crashSound");
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        float audioLevel = collision.relativeVelocity.magnitude / 1000.0f;
        audioSource.PlayOneShot(audioClip, audioLevel);
    }
}
