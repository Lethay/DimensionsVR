using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subtitle
{
    //A simple subtitle just needs a timestamp and its text
    public float timestamp;
    public string text;

    //A longer sentence might need to be split into multiple lines. Then, the first line will be the parent and the following lines will be its children
    public Subtitle parent;
    public List<Subtitle> children;

    //Constructors
    public Subtitle(){
        children = new List<Subtitle>();
    }
    public Subtitle(float ts, string s){
        timestamp = ts;
        text = s;
        children = new List<Subtitle>();
    }

    //Functions
    public bool HasChildren() {
        return (children != null && children.Count > 0);
    }
}

