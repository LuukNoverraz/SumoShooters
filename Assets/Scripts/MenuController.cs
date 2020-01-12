using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject LevelSelection;
    public GameObject Customization;
    public GameObject MadeByText;
    public GameObject Background;
    void Start()
    {
        Customization.GetComponent<RectTransform>().sizeDelta = Background.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        SettingsMenu.GetComponent<RectTransform>().localPosition = LevelSelection.GetComponent<RectTransform>().localPosition = Customization.GetComponent<RectTransform>().localPosition = new Vector3(0, -Screen.height, 0);
        MadeByText.GetComponent<RectTransform>().localPosition = new Vector3(0, -Screen.height + (Screen.height / 2) + 30, 0);
    }
    public void LocalPlayButton()
    {
        SceneManager.LoadScene("LocalMultiplayer");
    }
    public void CustomizationButton()
    {
        MainMenu.GetComponent<RectTransform>().localPosition = new Vector3(0, -Screen.height, 0);
        Customization.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    }
}
