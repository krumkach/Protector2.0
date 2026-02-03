using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachine : MonoBehaviour
{
    //screen objects
    [SerializeField] private GameObject mainMenuScreen;
    private GameObject currentScreen;
    
    void Start()//starting always with main menu
    {
        currentScreen = mainMenuScreen;
        currentScreen.SetActive(true);
    }

    public void ChangeScreen(GameObject newScreen)//Changing screen
    {
        currentScreen.SetActive(false);
        currentScreen = newScreen;
        currentScreen.SetActive(true);
    }
}
