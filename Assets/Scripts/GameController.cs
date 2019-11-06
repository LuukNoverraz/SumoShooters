using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform mainLightTransform;
    float randomYRotation;

    void Start()
    {
        randomYRotation = Random.Range(0.0f, 360.0f);
        mainLightTransform.rotation = Quaternion.Euler(new Vector3(50.0f, Mathf.Floor(randomYRotation), 0));
    }

    void Update()
    {
        
    }
}
