using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class turnOffShadows : MonoBehaviour
{

    float storedShadowDistance = 0;


    void OnPreRender()
    {
        storedShadowDistance = QualitySettings.shadowDistance;
        QualitySettings.shadowDistance = 0;
    }


    void OnPostRender()
    {
        QualitySettings.shadowDistance = storedShadowDistance;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
