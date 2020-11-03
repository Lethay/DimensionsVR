using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movePlane : MonoBehaviour
{
    public GameObject Target;
    private float m_Speed;
    public Slider SliderDis;

    void Start()
    {

    }

    void Update()
    {
        // Moving the frame according to the position of the slider
        Target.transform.position = new Vector3(0.0f, 0.0f, SliderDis.value);

  
    }
}