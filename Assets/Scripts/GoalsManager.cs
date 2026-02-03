using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalsManager : MonoBehaviour
{
    //Toggles for goal panel (for checking the progress)
    [SerializeField] Toggle cityHallToggle;
    [SerializeField] Toggle granaryToggle;
    [SerializeField] Toggle peasantsToggle;
    [SerializeField] Toggle wheatToggle;

    //two goals and one variable detecting their statuses
    private bool peasantGoal;
    private bool wheatGoal;
    private bool isComplete;

    //actual goal variables
    private int requiredWheat = 500;
    private int requiredPeasants = 50;

    //wheat image filling up
    [SerializeField] private Image grainImage;
    private float fillAmount;

    //game script example
    public GameScript game;

    void Start()
    {
        ResetGoals();
    }

    void Update()
    {
        //Changing the fill of wheat image and checking the goals statuses
        fillAmount = (float)game.wheatCount / (float)requiredWheat;
        grainImage.fillAmount = fillAmount;

        if (game.wheatCount >= requiredWheat)
        {
            wheatGoal = true;
        }
        else
        {
            wheatGoal = false;
        }

        if (game.peasant.GetCount() >= requiredPeasants)
        {
            peasantGoal = true;
        }

        CheckGoal(cityHallToggle, game.cityHall.GetIsBuilt());
        CheckGoal(granaryToggle, game.granary.GetIsBuilt());
        CheckGoal(wheatToggle, wheatGoal);
        CheckGoal(peasantsToggle, peasantGoal);

        //Overall victory (all goals complete)
        if (peasantGoal && wheatGoal && game.cityHall.GetIsBuilt() && game.granary.GetIsBuilt())
        {
            isComplete = true;
        }
    }

    public bool GetIsComplete() //getter for goals' status
    {
        return isComplete;
    }

    public void ResetGoals() //setting all goals to false
    {
        peasantGoal = false;
        wheatGoal = false;
        isComplete = false;
        fillAmount = 0;
        ClearToggles();
    }

    private void CheckGoal(Toggle toggle, bool isComplete) //Changing toggle statuses according to goals
    {
        if (isComplete)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

    private void ClearToggles() //Setting all toggles to negative
    {
        cityHallToggle.isOn = false;
        granaryToggle.isOn = false;
        peasantsToggle.isOn = false;
        wheatToggle.isOn = false;
    }
}
