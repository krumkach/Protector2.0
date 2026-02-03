using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class TutorialScript : MonoBehaviour
{
    //Interface objects
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private TMP_Text nextButtonText;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject textPanel;
    [SerializeField] private GameObject permissionPanel;
    [SerializeField] private GameObject circleImage;

    //List of objects to attach the pointer (circle image) to
    [SerializeField] private List<GameObject> objects = new List<GameObject>();

    private int indx;//for tutorial lines

    //list of tutorial lines
    private static string tutorialTextFile;
    private List<string> tutorialLines;

    //for time control
    public TimerManager timer;

    void Start()
    {
        SetLines();
        ResetTutorial();
    }

    public void ChangeText()//to be used on next button press
    {
        if (indx < (tutorialLines.Count() - 1))
        {
            indx++;

            if (indx == (tutorialLines.Count() - 1))
            {
                nextButtonText.text = "Play";
            }
        }
        else
        {
            ResetTutorial();
            ManageTutorial(false);
            timer.ResetTime();//to start the timer at the beginning of the game;
            timer.ChangePause(false);//to set pause status as unpaused at the beginning
        }

        UpdateText();
        ChangeCirclePosition();
    }

    public void UpdateText()//Renewing text according to line index
    {
        tutorialText.text = tutorialLines[indx];
        
    }

    public void ResetTutorial()//Setting tutorial to default (starting with permission panel, first line index is zero, circle position is on peasant panel
    {
        textPanel.SetActive(false);
        permissionPanel.SetActive(true);
        indx = 0;
        UpdateText();
        ChangeCirclePosition();
        circleImage.SetActive(false);
        nextButtonText.text = "Next";
    }

    private void SetLines()//Setting lines from text file
    {
        tutorialTextFile = Resources.Load("TutorialText").ToString();
        tutorialLines = new List<string>(tutorialTextFile.Split("|"));

    }

    public void ManageTutorial(bool active)//enabling/disabling tutorialPanel
    {
        tutorialPanel.SetActive(active);
    }

    public void ChangeCirclePosition()//Changing circle position according to current tutorial line
    {
        circleImage.transform.position = objects[indx].transform.position;
    }
}
