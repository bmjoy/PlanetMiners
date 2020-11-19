using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : WorldBlock
{
    protected string buildingName;
    private float _maxHealth = 0;


    public float maxhealth
    {
        set => _maxHealth = value;
        get => _maxHealth;
    }


    public void repair(float repairAmount)
    {
        if (health < maxhealth)
            health += repairAmount;
    }
}
