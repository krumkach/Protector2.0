using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;

public class EventScript : MonoBehaviour
{
    private static System.Random rnd = new System.Random();
    private int newPeasants;

    public int PeasantsArrive() //random number of peasants (from 1 to 5) will appear in case of event
    {
        newPeasants = rnd.Next(1, 6);

        return newPeasants;
    }
}
