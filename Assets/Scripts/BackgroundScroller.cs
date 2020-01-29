using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    // This script is used to select which background will be used and then to scroll it

    [SerializeField] private float scrollSpeed;
    [SerializeField] private Material[] backgrounds;
    private Material backgroundMaterial;
    private int randomBackground;
    [SerializeField] private Image imageComponent;

    void Start()
    {
        // Gets random int and uses it to determine which background will be used
        // randomBackground = (int) Random.Range(0, 5);     Use this when more backgrounds have been added
        randomBackground = 0;
        backgroundMaterial = backgrounds[randomBackground];
        imageComponent.material = backgroundMaterial;
    }

    void Update()
    {
        // Scroll background on the x axis by using += with a float scrollSpped multiplied by deltaTime
        backgroundMaterial.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, 0);
    }
}
