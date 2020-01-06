using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnieScript : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;

    void Update()
    {
        transform.Rotate(rotation.x * Time.deltaTime, rotation.y * Time.deltaTime, rotation.z * Time.deltaTime);
    }
}
