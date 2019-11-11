using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Transform mainLightTransform;
    float randomYRotation;
    public RectTransform borderTransform;
    public RectTransform[] scoreTexts;

    void Start()
    {
        randomYRotation = Random.Range(0.0f, 360.0f);
        mainLightTransform.rotation = Quaternion.Euler(new Vector3(50.0f, Mathf.Floor(randomYRotation), 0));
        borderTransform.sizeDelta = new Vector2(Screen.width, 10.0f);
        scoreTexts[0].sizeDelta = new Vector2(Screen.width - 20.0f, 30.0f);
        scoreTexts[1].sizeDelta = new Vector2(Screen.width - 100.0f, 30.0f);
    }

    void Update()
    {
        
    }
}
