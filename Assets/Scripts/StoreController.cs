using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour
{
    public Color color;
    public Renderer targetPlayerRenderer;
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
        if (tag == "Buyable" && PlayerPrefs.HasKey("Sumocoins") && PlayerPrefs.GetInt("Sumocoins", 0) > 1)
        {
            PlayerPrefs.SetInt("Sumocoins", PlayerPrefs.GetInt("Sumocoins", 0) - 1);
        }
        targetPlayerRenderer.material.color = color;
        image.sprite = checkmark;
        PlayerPrefs.SetFloat("ChosenColorR", color.r);
        PlayerPrefs.SetFloat("ChosenColorG", color.g);
        PlayerPrefs.SetFloat("ChosenColorB", color.b);
    }
    public void OnPointerDown2()
    {
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Bought2"))
        {
            if (item.GetComponent<Image>().sprite == checkmark)
            {
                
                item.GetComponent<Image>().sprite = unlocked;
            }
        }
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Buyable2"))
        {
            if (item.GetComponent<Image>().sprite == checkmark)
            {
                
                item.GetComponent<Image>().sprite = locked;
            }
        }
        targetPlayerRenderer.material.color = color;
        image.sprite = checkmark;
        PlayerPrefs.SetFloat("ChosenColorR2", color.r);
        PlayerPrefs.SetFloat("ChosenColorG2", color.g);
        PlayerPrefs.SetFloat("ChosenColorB2", color.b);
    }
}
