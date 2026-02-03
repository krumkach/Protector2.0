using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SummaryScript : MonoBehaviour
{
    //Info text object
    [SerializeField] private TMP_Text summaryText;

    //class examples
    public TimerManager timer;
    public FightScript fight;

    void Update()
    {
        UpdateText(timer.GetCycleCount(), fight.GetRaidCount());
    }

    private void UpdateText(int cycleCount, int raidCount)//Changing info according to relevant data
    {
        summaryText.text = $"Cycles passed: {cycleCount} \n Raids survived: {raidCount}";
    }
}
