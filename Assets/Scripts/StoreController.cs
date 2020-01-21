using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour
{
    public Color color;
    public Renderer renderer;
    public Image image;
    public Sprite locked;
    public Sprite unlocked;
    public Sprite checkmark;

    public void OnPointerDown()
    {
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Bought"))
        {
            if (item.GetComponent<Image>().sprite == checkmark)
            {
                
                item.GetComponent<Image>().sprite = unlocked;
            }
        }
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Buyable"))
        {
            if (item.GetComponent<Image>().sprite == checkmark)
            {
                
                item.GetComponent<Image>().sprite = locked;
            }
        }
        renderer.material.color = color;
        image.sprite = checkmark;
        // PlayerPrefs.SetString("ChosenColor", );
    }
}
