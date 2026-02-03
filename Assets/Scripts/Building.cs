using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Building
{
    //Properties set in example declaring
    private string name;
    private int buildCost;
    private float buildTime;
    private Button buildButton;
    private bool isBuilt = false;
    private TMP_Text buttonText;

    //properties for button image change
    public bool isBuildingOn = false;
    private float currentTime;
    private float fillAmount;

    private string message = "";
    private bool showMessage = false;

    public Building(string name, int buildCost, float buildTime, Button buildButton, TMP_Text buttonText)
    {
        this.name = name;
        this.buildCost = buildCost;
        this.buildTime = buildTime;
        this.buildButton = buildButton;
        buildButton.interactable = false;
        this.buttonText = buttonText;
        buttonText.text = $"Build ({buildCost} wheat)";
    }

    public void StartBuilding()//intitalizing building process
    {
        currentTime = buildTime;
        isBuildingOn = true;
        fillAmount = 1.0f;
    }

    public void BuildingProcess() //For update method (build button fill)
    {
        if (isBuildingOn)
        {
            currentTime -= (1.0f * Time.deltaTime);
            fillAmount = currentTime / buildTime;
            buildButton.image.fillAmount = 1.0f - fillAmount;
            buildButton.interactable = false; 

            if (currentTime < 0.0f)
            {
                isBuildingOn = false;
                Build();
            }
        }
    }

    private void Build()//changing bool var for completing a goal
    {
        isBuilt = true;
        message = $"{name} was built";
        showMessage = true;
        buttonText.text = "Built";
    }

    //couple of getters
    public bool GetIsBuilt()
    {
        return isBuilt;
    }

    public int GetCost()
    {
        return buildCost;
    }

    public Button GetBuildButton()
    {
        return buildButton;
    }

    //resetting for start over
    public void ResetBuilding()
    {
        isBuilt = false;
        isBuildingOn = false;
        message = "";
        showMessage = false;
        buttonText.text = $"Build ({buildCost} wheat)";
        buildButton.image.fillAmount = 1.0f;

    }

    public void ChangeShowMessage(bool isShown) //For enabling/disabling message display
    {
        showMessage = isShown;
    }

    //Getters
    public bool GetShowMessage()
    {
        return showMessage;
    }

    public string GetMessage()
    {
        return message;
    }

    public int WheatSpend(int wheatCount) //spending wheat for hiring
    {
        wheatCount -= buildCost;

        return wheatCount;
    }
}
