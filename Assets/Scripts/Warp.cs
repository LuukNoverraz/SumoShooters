using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    
    [SerializeField] private Vector3 warpLocation;

    void OnTriggerEnter(Collider collision)
    {
        // Check which warp is being touched
        if (gameObject.layer == 25)
        {
            Debug.Log("go to 1");
        }
        if (gameObject.layer == 26)
        {
            Debug.Log("go to 2");
        }
    }
}
