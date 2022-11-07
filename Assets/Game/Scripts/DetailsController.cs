using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetailsController : MonoBehaviour
{
    [Header("Details Information")]
    [SerializeField] private Building[] buildings;
    [SerializeField] private TMP_Text buildingName;
    [SerializeField] private TMP_Text productionName;
    private static DetailsController _instance;
    public static DetailsController Instance
    {
        get 
        {
            if(_instance == null)
            {
                Debug.Log("Details Controller is null.");
            }
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
    }
    // Update is called once per frame
    public void SetValues(float selectedValue)
    {
        for(int i = 0; i < buildings.Length; i++)
        {
            if(buildings[i].buildingNumber == selectedValue)
            {
                buildingName.text = buildings[i].buildingName;
                productionName.text = buildings[i].productionName;
            }
        }
        if(selectedValue < 1)
        {
            buildingName.text = "-";
            productionName.text = "-";
        }
    }
}
