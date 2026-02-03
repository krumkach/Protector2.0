using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameScript : MonoBehaviour
{
    //Unit class examples
    public Unit warrior;
    public Unit peasant;

    //MessageScript class examples
    public MessageScript systemMessage;
    public MessageScript warriorMessage;
    public MessageScript peasantMessage;

    //Building class examples
    public Building cityHall;
    public Building granary;

    //Resource value variable
    public int wheatCount;

    //Class examples
    public TimerManager timer;
    public FightScript fight;
    public EventScript events;
    public SoundManager sound;

    //Unit hire Buttons
    [SerializeField] private Button warriorHireButton;
    [SerializeField] private Button peasantHireButton;

    //Building build buttons
    [SerializeField] private Button cityHallButton;
    [SerializeField] private Button granaryButton;

    //Building text on buttons
    [SerializeField] private TMP_Text cityHallButtonText;
    [SerializeField] private TMP_Text granaryButtonText;

    //Interface info elements
    [SerializeField] private TMP_Text wheatText;
    [SerializeField] private TMP_Text warriorNumberText;
    [SerializeField] private TMP_Text peasantNumberText;

    //Messages
    [SerializeField] private TMP_Text systemMessageText;
    [SerializeField] private TMP_Text warriorMessageText;
    [SerializeField] private TMP_Text peasantMessageText;

    //limits for getting new resources
    private int wheatLimit = 200;
    private bool notEnoughSpace = false;

    //Game panel objects
    [SerializeField] private GameObject summary;
    [SerializeField] private GameObject winPanel;

    void Start()
    {
        SetUnits();
        SetBuildings();
        SetMessages();
    }

    void Update()
    {
        //Show messages if showmessage is on
        systemMessage.ShowMessage();
        warriorMessage.ShowMessage();
        peasantMessage.ShowMessage();

        //Those are setting bool variables for building/hiring on
        IsEnoughWheat(warrior);
        IsEnoughWheat(peasant);

        IsEnoughForBuilding(cityHall);
        IsEnoughForBuilding(granary);

        //Those are executing hiring/building according to data set in previous four lines
        warrior.HiringProcess();
        peasant.HiringProcess();

        cityHall.BuildingProcess();
        granary.BuildingProcess();

        ChangeUnitMessage(warrior, 5, warriorMessage);
        ChangeUnitMessage(peasant, 4, peasantMessage);

        ChangeBuildingMessage(cityHall);
        ChangeBuildingMessage(granary);

        //limiting producing (depends on status of quest buildings)
        if (!(granary.GetIsBuilt()) && wheatCount >= wheatLimit)
        {
            systemMessage.ChangeMessage("Granary is required!");
            notEnoughSpace = true;
        }
        else
        {
            notEnoughSpace = false;
        }

        UpdateInfo();
    }

    public void Hire(Button pressedButton) //Initializing hiring process by pressing the button
    {
        Unit currentUnit;

        if (pressedButton == warriorHireButton)
        {
            currentUnit = warrior;
        }
        else
        {
            currentUnit = peasant;
        }

        wheatCount = currentUnit.WheatSpend(wheatCount); //Spending wheat on hiring
        currentUnit.StartHiring(); //Starting the process
        UpdateInfo(); 
    }

    public void Build(Button pressedButton) //Initializing building process by pressing the button
    {
        sound.PlaySoundEffect(8);
        Building currentBuilding;

        if (pressedButton == cityHallButton)
        {
            currentBuilding = cityHall;
        }
        else
        {
            currentBuilding = granary;
        }

        wheatCount = currentBuilding.WheatSpend(wheatCount); //Spending wheat on building
        currentBuilding.StartBuilding(); //Starting the process
        UpdateInfo();
    }

    public void WheatChange() //Changing the wheat each cycle according to number of different units
    {
        wheatCount = peasant.WheatChange(wheatCount); //Peasants come first, so wheat count will be increased by production before decreasing by consumption
        wheatCount = warrior.WheatChange(wheatCount);
        if (notEnoughSpace)
        {
            wheatCount = wheatLimit;
        }

        if (warrior.GetCount() > 0 && wheatCount < 0) //If wheat count turns negative it becomes null and one warrior deserts
        {
            wheatCount = 0;
            warrior.Desertion();
            systemMessage.ChangeMessage("Not enough food! Warriors are deserting.");
        }

        if (peasant.GetWheatChange() == warrior.GetWheatChange() && !warrior.GetHireButton().interactable && !peasant.GetHireButton().interactable) //if there's no changes in wheat count and both hiring buttons are inactive the event is initialized
        {
            timer.isEventOn = true;
        }

        UpdateInfo();
    }

    public void UpdateInfo()
    {
        wheatText.text = wheatCount.ToString();
        warriorNumberText.text = warrior.GetCount().ToString();
        peasantNumberText.text = peasant.GetCount().ToString();

    }

    //Setters
    private void SetUnits()
    {
        warrior = new Unit("Warriors", 10.0f, 0, 10, 1, false, warriorNumberText, warriorHireButton);
        peasant = new Unit("Peasants", 10.0f, 1, 4, 1, true, peasantNumberText, peasantHireButton);
    }

    private void SetBuildings()
    {
        cityHall = new Building("City hall", 200, 100.0f, cityHallButton, cityHallButtonText);
        granary = new Building("Granary", 100, 50.0f, granaryButton, granaryButtonText);
    }

    private void SetMessages()
    {
        systemMessage = new MessageScript(systemMessageText);
        warriorMessage = new MessageScript(warriorMessageText);
        peasantMessage = new MessageScript(peasantMessageText);
    }

    private void IsEnoughWheat(Unit unit) //Checking if there's enough wheat for hiring (enabling/disabling hiring buttons)
    {
        if (wheatCount < unit.GetCost())
        {
            unit.GetHireButton().interactable = false;
        }
        else
        {
            unit.GetHireButton().interactable = true;
        }
    }

    private void IsEnoughForBuilding(Building building)//Checking ability of building
    {
        if (!(building.GetIsBuilt()) && wheatCount >= building.GetCost())
        {
            building.GetBuildButton().interactable = true;
        }
        else
        {
            building.GetBuildButton().interactable = false;
        }
    }

    public void Event()//For preventing game stuck when there's no wheat produce/spending and not enough wheat to hire more peasants;
    {
        peasant.EventPeopleArrive(events.PeasantsArrive());
        systemMessage.ChangeMessage("New settlers has arrived!");
        UpdateInfo();
    }

    public void RestartGame() //Pack of resetters
    {
        notEnoughSpace = false;
        sound.ResetSound();
        sound.SetGameIsOn(false);
        ResetUnits();
        ResetBuildings();
        ResetMessages();
        winPanel.SetActive(false);
        fight.RestartRaids();
        timer.ResetTimer();
        wheatCount = 0;
        UpdateInfo();
    }

    //Resetters for starting over
    private void ResetUnits() 
    {
        warrior.ResetCount(0);
        peasant.ResetCount(1);
    }

    private void ResetBuildings()
    {
        cityHall.ResetBuilding();
        granary.ResetBuilding();
    }

    private void ResetMessages()
    {
        systemMessage.ResetMessage();
        warriorMessage.ResetMessage();
        peasantMessage.ResetMessage();
    }

    public void Win() //When goals achieved
    {
        sound.PlayMusic(1);
        winPanel.SetActive(true);
    }

    //methods for correct displaying messages
    private void ChangeUnitMessage(Unit unit, int indx, MessageScript message)
    {
        if (unit.GetShowMessage())
        {            
            message.ChangeMessage(unit.GetMessage());
            unit.ChangeShowMessage(false);
            sound.PlaySoundEffect(indx);
        }
    }

    private void ChangeBuildingMessage(Building building)
    {
        if (building.GetShowMessage())
        {
            sound.PlaySoundEffect(9);
            systemMessage.ChangeMessage(building.GetMessage());
            building.ChangeShowMessage(false);
        }
    }
}
