using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class NarrationManager : MonoBehaviour
{
    //index for narrative lines
    private int index;

    //interface objects
    [SerializeField] private TMP_Text narrativeText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private TMP_Text nextButtonText;
    [SerializeField] private GameObject narrativePanel;

    //list of narrative lines
    private static string narrationTextFile;
    private List<string> narrationLines;

    //tutorial class example
    public TutorialScript tutorial;

    public void ChangeText() //For use on next button
    {
        if (index < narrationLines.Count - 1)
        {
            index++;
            UpdateText();
        }
        else
        {
            narrativePanel.SetActive(false);
            tutorial.ManageTutorial(true);
        }
        
        if (index == narrationLines.Count - 1)
        {
            nextButtonText.text = "Begin";
        }
        
    }

    void Start()
    {
        SetLines();
        RestartNarrative();
    }

    public void RestartNarrative()//Restarting narration from line 1 (index 0)
    {
        index = 0;
        nextButtonText.text = "Next";
        UpdateText();
    }

    private void UpdateText()
    {
        narrativeText.text = narrationLines[index];
    }

    private void SetLines()//Setting lines from text file
    {
        narrationTextFile = Resources.Load("IntroText").ToString();
        narrationLines = new List<string>(narrationTextFile.Split("|"));
    }
}
