using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Building
{
    private bool _generating = false;
    private int _storedCrystals = 0;
    private int _genTimePerCrystal = 10;


    private void Start()
    {
        StartCoroutine(generatePower());
    }
    public void depositCrystal(Resource crystal)
    {
            if (_generating)
                _storedCrystals++;
            else
                StartCoroutine(generatePower());

            Destroy(crystal.gameObject);
        
    }


    IEnumerator generatePower()
    {
        _generating = true;
        PowerSystem.powerSystem.addGeneration(10);
        yield return new WaitForSeconds(_genTimePerCrystal);
        PowerSystem.powerSystem.subtractGeneration(10);

        if (_storedCrystals > 0)
        {
            _storedCrystals--;
            StartCoroutine(generatePower());
        }
        else
            _generating = false;

    }
}
