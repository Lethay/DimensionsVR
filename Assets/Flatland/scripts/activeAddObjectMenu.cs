using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeAddObjectMenu : MonoBehaviour{
    public GameObject ObjectsSubmenu;



    public void ChangeMenuVisibility()
    {
        //
        this.gameObject.SetActive(!this.gameObject.activeSelf);
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
