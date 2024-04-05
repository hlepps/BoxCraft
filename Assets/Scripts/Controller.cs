using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Controller : MonoBehaviour 
{
    // to mia³a byæ zmienna o lekkim typie, ale zapomia³em ¿e istnieje byte xdd
    [SerializeField] char teamID;

    public char GetTeamID()
    {
        return teamID;
    }
}
