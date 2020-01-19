using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed;
    public Material[] backgrounds;
    private Material backgroundMaterial;
    private int randomBackground;
    public Image imageComponent;

    void Start()
    {
        randomBackground = (int) Random.Range(0, 0);
        backgroundMaterial = backgrounds[randomBackground];
        imageComponent.material = backgroundMaterial;
    }

    void Update()
    {
        backgroundMaterial.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, 0);
    }
}
