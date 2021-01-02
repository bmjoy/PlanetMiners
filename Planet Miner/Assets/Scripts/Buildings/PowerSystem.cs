using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSystem : MonoBehaviour
{
    public static PowerSystem powerSystem;

    private void Awake()
    {
        powerSystem = this;
    }

    private void Start()
    {
        setPowerNetwork();
    }

    private static int powerNeedTotal = 0;
    private static int powerGenerationTotal = 0;

    private List<Building> objectsInNetwork = new List<Building>();

    public void addToNetwork(Building building)
    {
        objectsInNetwork.Add(building);
        powerNeedTotal += building.powerNeed;
        setPowerNetwork();
    }

    public void removeFromNetwork(Building building)
    {
        objectsInNetwork.Remove(building);
        powerNeedTotal -= building.powerNeed;
        setPowerNetwork();
    }

    public  void addGeneration(int amount)
    {
        powerGenerationTotal += amount;
        setPowerNetwork();
    }

    public void subtractGeneration(int amount)
    {
        powerGenerationTotal -= amount;
        setPowerNetwork();
    }

    public bool hasCapacity { get => powerGenerationTotal >= powerNeedTotal; }

    public void setPowerNetwork()
    {

        UIControl.uIControl.updatePowerGridText($"Generating: {powerGenerationTotal} \nConsuming: {powerNeedTotal}");
        bool hasCap = hasCapacity;
            foreach (Building building in objectsInNetwork)
                building.setPower(hasCap);
    }

}
