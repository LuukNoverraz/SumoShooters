using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject LevelSelection;
    public GameObject Customization;
    public GameObject MadeByText;
    public GameObject Background;
    public GameObject Background2;
    void Start()
    {
        SettingsMenu.GetComponent<RectTransform>().localPosition = LevelSelection.GetComponent<RectTransform>().localPosition = Customization.GetComponent<RectTransform>().localPosition = new Vector3(0, -Screen.height, 0);
        Customization.GetComponent<RectTransform>().sizeDelta = Background.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        MadeByText.GetComponent<RectTransform>().localPosition = new Vector3(0, -Screen.height + (Screen.height / 2) + 30, 0);
        Background2.transform.localScale = new Vector3(Screen.height, Screen.height, 1.0f);
        Background2.GetComponent<RectTransform>().localPosition = new Vector3(Screen.width/2, Screen.height/2, 1);
    }
}
