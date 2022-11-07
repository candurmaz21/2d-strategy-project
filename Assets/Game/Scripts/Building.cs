using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buildings", menuName = "ScriptableObjects/Buildings")]
public class Building : ScriptableObject
{
    public BuildingType.BuildingTypes types; 
    public int buildingNumber;
    public int buildingXValue;
    public int buildingYValue;
    public Color buildingColor;
    public string buildingName;
    public string productionName;
    //...
    //Other values that can be added.
}
