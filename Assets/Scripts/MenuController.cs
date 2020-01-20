using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public RectTransform MainMenu;
    public RectTransform SettingsMenu;
    public RectTransform Customization;
    public GameObject Players;
    public RectTransform Colors;
    public RectTransform ColorLocks;
    public RectTransform MadeByText;
    public RectTransform Background;
    Vector3 offScreen = new Vector3(0, 9999, 0);
    
    void Start()
    {
        Customization.sizeDelta = Background.sizeDelta = new Vector2(Screen.width, Screen.height);
        Players.transform.position = offScreen;
        SettingsMenu.localPosition = Customization.localPosition = offScreen;
        MadeByText.localPosition = new Vector3(0, -Screen.height + (Screen.height / 2) + 40, 0);
    }
    
    public void LocalPlayButton()
    {
        SceneManager.LoadScene("LocalMultiplayer");
    }
    
    public void CustomizationButton()
    {
        MainMenu.localPosition = offScreen;
        Customization.localPosition = Vector3.zero;
        Players.transform.position = Vector3.zero;
        Colors.localPosition = ColorLocks.localPosition = new Vector3(0, (-Screen.height / 2) + (Screen.height / 10), 0);
        Colors.sizeDelta = ColorLocks.sizeDelta = new Vector2(Screen.width, 232.5f);
    }

    public void CustomizationBack()
    {
        MainMenu.localPosition = Vector3.zero;
        Customization.localPosition = offScreen;
        Players.transform.position = offScreen;
    }

    public void SettingsButton()
    {  
        MainMenu.localPosition = offScreen;
        SettingsMenu.localPosition = Vector3.zero;
    }

    public void SettingsBack()
    {  
        MainMenu.localPosition = Vector3.zero;
        SettingsMenu.localPosition = offScreen;
    }
}