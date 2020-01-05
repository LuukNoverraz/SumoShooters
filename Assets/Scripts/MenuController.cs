using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject LevelSelection;
    void Start()
    {
        SettingsMenu.GetComponent<RectTransform>().localPosition = LevelSelection.GetComponent<RectTransform>().localPosition = new Vector3(0, -9999, 0);
    }
    void Update()
    {
        
    }
}
