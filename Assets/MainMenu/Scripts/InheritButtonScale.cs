using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritButtonScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.parent.position;
        transform.localScale = transform.parent.localScale;
    }

    // Update is called once per frame
    [ExecuteInEditMode]
    void Update()
    {
        transform.position = transform.parent.position;
        transform.localScale = transform.parent.localScale;
    }

    
    public void Awake()
    {
        Start();
    }
}
