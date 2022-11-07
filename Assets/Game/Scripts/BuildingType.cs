using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingType : MonoBehaviour
{
    public enum BuildingTypes
    {
        powerPlant,
        barracks,
        empty,
    }
    public static BuildingTypes selectedBuilding;
    [SerializeField] private TMP_Text textSelectedBuildingText;
    private void OnEnable()
    {
        
        GridController.OnBuildingTypeChanged += SelectBuilding;
    }
    public void SelectBuilding(BuildingTypes type)
    {
        switch(type) 
        {
        case BuildingTypes.powerPlant:
            textSelectedBuildingText.text = "Power Plant";
            selectedBuilding = BuildingTypes.powerPlant;
            break;
        case BuildingTypes.barracks:
            textSelectedBuildingText.text = "Barracks";
            selectedBuilding = BuildingTypes.barracks;
            break;
        case BuildingTypes.empty:
            textSelectedBuildingText.text = "-";
            selectedBuilding = BuildingTypes.empty;
            break;
        default:
            break;
        }
    }
}
