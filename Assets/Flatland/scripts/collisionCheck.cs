using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionCheck : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        Destroy(col.gameObject);
    }
}
