using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightScript : MonoBehaviour
{
    //neccesary script examples
    public TimerManager timer;
    public GameScript game;
    public SoundManager sound;

    //raid properties
    private int enemiesCount;
    private int defendersCount;
    private float raidTime;
    private int raidCount;
    private float multiplier;//For making raids more difficult to survive

    //for raid coming time
    private float currentTime;
    private float fillAmount;

    //for attack enabling and attack outcome
    private bool attack;
    private bool defeat;

    //raid interface
    [SerializeField] private Image raidTimeImage;
    [SerializeField] private TMP_Text raidCountText;
    [SerializeField] private TMP_Text defendersCountText;
    [SerializeField] private TMP_Text enemiesCountText;

    //panel for attack
    [SerializeField] private GameObject attackPanel;

    //for attack outcome interface
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject gameOverButton;

    void Start()
    {
        RestartRaids();
        multiplier = 1.5f;
    }

    void Update()
    {
        RaidIsComing(game.warrior.GetCount());
    }

    public void RaidIsComing(int warriors) //raid coming 
    {
        if (!attack)
        {
            currentTime -= (1.0f * Time.deltaTime);
            fillAmount = currentTime / raidTime;
            raidTimeImage.fillAmount = 1.0f - fillAmount;
            defendersCount = warriors;

            if (currentTime < 0.0f)
            {
                Attack();
            }
        }        
    }

    private void Attack() //attack event is on
    {
        sound.PlayMusic(3);
        sound.PlaySoundEffect(6);
        timer.StopTime(); //stopping the time
        attack = true; //enabling attack to stop RaidIsComing method
        UpdateText(); //renewing defenders number according to current warriors count
        attackPanel.SetActive(true); //showing attackPanel
        game.UpdateInfo();

        //Changing outcome according to result of the battle
        if (enemiesCount > defendersCount)
        {
            defeat = true;
        }
        else
        {
            //Changing next raid properties (more enemies, less time to prepare
            defeat = false;
            raidCount++;
            if (enemiesCount < 15)
            {
                enemiesCount = (int)(enemiesCount * multiplier);
            }
            else
            {
                enemiesCount += 5;
            }

            if (raidTime > 60.0f)
            {
                raidTime /= multiplier;
            }
            else
            {
                raidTime = 55.0f;//Time less than 50 secs will be not enough to hire enough warriors
            }
        }

        SetButton(defeat);
    }

    public void ResetRaid() //changing raid from attack mode to raid coming mode
    {
        attack = false;
        currentTime = raidTime;
        UpdateText();
        attackPanel.SetActive(false);
    }

    private void UpdateText() //displaying correct text info
    {
        raidCountText.text = enemiesCount.ToString();
        enemiesCountText.text = enemiesCount.ToString();
        defendersCountText.text = defendersCount.ToString();
    }

    private void SetButton(bool defeat) //setting buttons according to attack outcome
    {
        continueButton.SetActive(!defeat);
        gameOverButton.SetActive(defeat);
    }

    public void RestartRaids() //setting raid properties to default
    {
        raidCount = 0;
        raidTime = 120.0f;
        defendersCount = 0;
        enemiesCount = 5;
        fillAmount = 1.0f;
        ResetRaid();
        attackPanel.SetActive(false);
        currentTime = raidTime;
    }

    //Getter for summary
    public int GetRaidCount()
    {
        return raidCount;
    }
}
