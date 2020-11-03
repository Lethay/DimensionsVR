using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.parent.position - transform.parent.forward * transform.parent.lossyScale.z / transform.parent.forward.magnitude; 
            // transform.parent.lossyScale.z;
        
        // Vector3 a = transform.localEulerAngles;
        // a.y = 0;
        // transform.localEulerAngles = a;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ExecuteInEditMode]
    public void Awake()
    {
        Start();
    }
}
