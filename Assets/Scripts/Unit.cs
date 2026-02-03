using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Unit
{
    //properties that are set in example declaring
    private string name;
    private float hiringTime;
    private int count;
    private int cost;
    private int wheatCountChange;
    private bool isProducing; //for dividing wheat production and consumption
    private TMP_Text countText;
    private Button hireButton;

    //properties same for each example, for change during playing process
    private bool isHiringOn = false;
    private float currentTime;
    private float fillAmount = 1.0f;

    private string message = "";
    private bool showMessage = false;

    public Unit(string name, float time, int count, int cost, int change, bool isProducing, TMP_Text countText, Button hireButton)
    {
        this.name = name;
        hiringTime = time;
        this.count = count;
        this.cost = cost;
        wheatCountChange = change;
        this.isProducing = isProducing;
        this.countText = countText;
        this.hireButton = hireButton;
    }

    public void Hire() //Increases number of units by one
    {
        count++;
        message = "+1";
        showMessage = true;
    }

    public int WheatChange(int wheatCount) //Changing wheat count depends on number and type of units
    {
        if (isProducing)
        {
            wheatCount += GetWheatChange();
        }
        else
        {
            wheatCount -= GetWheatChange();
        }

        return wheatCount;
    }

    public int WheatSpend(int wheatCount) //spending wheat for hiring
    {
        wheatCount -= cost;

        return wheatCount;
    }

    public void Desertion() //Only for fighters, decreasing number by one in case of insufficient wheat
    {
        count--;
        message = "-1";
        showMessage = true;
    }

    public void EventPeopleArrive(int number) //For event
    {
        count += number;
        message = $"+{number}";
        showMessage = true;
    }

    public void StartHiring() //Initializing hiring process
    {
        currentTime = hiringTime;
        isHiringOn = true;
        fillAmount = 1.0f;
    }

    public void HiringProcess() //To be used in Update
    {
        if (isHiringOn)
        {
            currentTime -= (1.0f * Time.deltaTime);
            fillAmount = currentTime / hiringTime;
            hireButton.image.fillAmount = 1.0f - fillAmount;
            hireButton.interactable = false;

            if (currentTime < 0.0f)
            {
                isHiringOn = false;
                Hire();
                countText.text = count.ToString();
            }
        }
    }

    public void ResetCount(int startCount) //Resetting count without making new class example (for start over in case of defeat)
    {
        SetCount(startCount);
        isHiringOn = false;
        hireButton.image.fillAmount = 1.0f;
        message = "";
        showMessage = false;
    }

    //setter for peasant count (for restart)
    public void SetCount(int number)
    {
        count = number;
    }

    public void ChangeShowMessage(bool isShown)//for activating showing message
    {
        showMessage = isShown;
    }

    //couple of getters for accessing private variables
    public int GetCount()
    {
        return count;
    }

    public int GetCost()
    {
        return cost;
    }

    public int GetWheatChange()
    {
        return count * wheatCountChange;
    }

    public Button GetHireButton()
    {
        return hireButton;
    }

    public bool GetShowMessage()
    {
        return showMessage;
    }

    public string GetMessage()
    {
        return message;
    }
}
