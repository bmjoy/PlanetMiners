using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : WorldBlock
{
    protected string buildingName;

    private float _maxHealth = 0;

    protected int _powerNeed;
    private bool _hasPower = false;
    public int powerNeed { get => _powerNeed; }

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

    public void setPower(bool hasPower)
    {
        _hasPower = hasPower;
    }
}
