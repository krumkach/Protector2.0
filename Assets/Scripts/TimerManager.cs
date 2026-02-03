using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerManager : MonoBehaviour
{
    //Time speed interface
    [SerializeField] public Button pauseButton;
    [SerializeField] private Button speedUpButton;
    [SerializeField] private Button speedDownButton;

    //Variables for changing time speed
    private bool isPaused;
    private float currentTimeScale;

    //examples of required scripts
    public GameScript game;
    public FightScript fight;
    public GoalsManager goal;

    //variables for cycle passing
    private float cycleTime = 3.0f;
    private float currentTime;
    private int cycle;

    //timer interface elements
    [SerializeField] Image timerImage;
    [SerializeField] TMP_Text timerText;

    //for events to occure
    public bool isEventOn;

    //For correct timer animation
    private float timerFillAmount;

    void Start() //Timer is on stop by the start
    {
        StopTime();
        currentTimeScale = 1.0f;
        isPaused = true;
    }

    void Update()
    {
        if (goal.GetIsComplete()) //Stopping timer and showing win screen if both goals are completed
        {
            StopTime();
            game.Win();
        }

        SetTimerControllers();
        CyclePass();

        if (isEventOn)
        {
            if (cycle % 5 == 0)
            {
                game.Event();
                isEventOn = false;
            }
        }
    }

    private void CyclePass() //Whenever timer is on, cycles pass one by one
    {
        currentTime += (1.0f * Time.deltaTime);
        timerFillAmount = currentTime / cycleTime;
        timerImage.fillAmount = 1.0f - timerFillAmount;

        if (currentTime >= cycleTime) //end of cycle, increasing cycle count and changing wheat count
        {
            currentTime = 0.0f;
            cycle++;
            timerText.text = cycle.ToString();

            game.WheatChange();
        }
    }

    public void ChangeTime(Button pressedButton) //time speed control script
    {
        if (pressedButton == pauseButton)
        {
            if (isPaused)
            {
                ResetTime(); //restoring last chosen speed
                game.systemMessage.ChangeMessage("Time is unpaused");
            }
            else
            {
                StopTime(); //reducing time speed to 0
                game.systemMessage.ChangeMessage("Time is paused");
            }

            isPaused = !isPaused; //changing the bool variable
        }
        else if (pressedButton == speedUpButton) //only 1 and 2 values of time scale is avaliable by pressing speed up and down buttons
        {
            currentTimeScale = 2;
            ResetTime();
            game.systemMessage.ChangeMessage("Speed up");
        }
        else
        {
            currentTimeScale = 1;
            ResetTime();
            game.systemMessage.ChangeMessage("Speed down");
        }
    }

    private void SetTimerControllers() //enabling/disabling timer control interface according to current time speed
    {
        if (Time.timeScale > 1)
        {
            SetInteractable(false, true);
        }
        else if (Time.timeScale == 0)
        {
            SetInteractable(false, false);
        }
        else
        {
            SetInteractable(true, false);
        }
    }

    private void SetTimeSpeed(float speed)//changing time speed
    {
        Time.timeScale = speed;
    }

    public void ResetTimer()
    {
        cycle = 0; //nullifying cycle count
        currentTime = 0.0f; //nullifying current cycle
        timerFillAmount = 1.0f; //changing cycle icon to full image
        currentTimeScale = 1.0f; //setting default speed
        StopTime();
        isPaused = true; //isPaused is true because the game hadn't been started
        isEventOn = false; //no events without need
        goal.ResetGoals(); //setting goals to false;
        timerText.text = cycle.ToString();
    }

    private void SetInteractable(bool speedUp, bool speedDown) //enabling and disablind speed up and down buttons (pause/play button is always enabled)
    {
        speedUpButton.interactable = speedUp;
        speedDownButton.interactable = speedDown;
    }

    public void StopTime() //stopping the timer
    {
        SetTimeSpeed(0);
    }

    public void ResetTime()//enabling the timer again with last chosen speed (1 by default)
    {
        SetTimeSpeed(currentTimeScale);
    }
    
    //Getter for summary
    public int GetCycleCount()
    {
        return cycle;
    }

    public void ChangePause(bool status) //For correct ispaused status after tutorial
    {
        isPaused = status;
    }
}
