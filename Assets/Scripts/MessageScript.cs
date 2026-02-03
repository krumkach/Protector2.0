using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageScript
{
    //Text object
    private TMP_Text messageText;

    //Properties for showing message
    private bool messageIsOn;
    private float messageTimer = 3.0f;
    private float currentMessageTime;

    public MessageScript(TMP_Text messageText)
    {
        this.messageText = messageText;
        ClearMessage();
    }

    public void ShowMessage() //Process of showing message during the 3 secs timer 
    {
        if (messageIsOn)
        {
            currentMessageTime += (1.0f * Time.deltaTime);
            if (currentMessageTime >= messageTimer)
            {
                ClearMessage();
            }
        }
    }

    public void ChangeMessage(string message) //Changing text and initializing showing process
    {
        messageText.text = message;
        messageIsOn = true;
    }

    private void ClearMessage()//resetting message timer and clearing message text
    {
        messageText.text = "";
        currentMessageTime = 0.0f;
        messageIsOn = false;
    }

    public void ResetMessage() //public method to use in GameScript
    {
        ClearMessage();
    }
}
