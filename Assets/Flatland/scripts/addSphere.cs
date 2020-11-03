using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class addSphere : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddObject()
    {
        // Create object (actually it is a copy)
        // Instantiate(this.gameObject, new Vector3(0,4,0)  , Quaternion.identity);
        // Create it with random noise on location
        Vector3 position = new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
        Instantiate(this.gameObject, position + new Vector3(0, 4, 0), Quaternion.identity);


    }

}
