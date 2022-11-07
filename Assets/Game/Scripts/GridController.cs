using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridController : MonoBehaviour
{
    //Essentials.
    public static event Action<BuildingType.BuildingTypes> OnBuildingTypeChanged;
    private Grid grid;

    //Serializable Fields.
    [Header("Square Infos")]
    [Tooltip("32x32 square icon for place.")]
    [SerializeField] GameObject squareIcon;
    [Tooltip("Square default position.")]
    [SerializeField] Vector3 squareStartPos;
    [Tooltip("Alpha of the square value.")]
    [SerializeField] float alphaValue;
    [Tooltip("Building Infos.")]
    [SerializeField] Building[] buildings;
    //Helpers
    private Vector3[] currentMidPoint = new Vector3[20];
    private Vector3 activeMidPoint;
    private GameObject squareIconSpawned;
    float cellSize = .32f;
    float buildingValue;
    int redCount;
    Color buildingColor;
    private List<GameObject> squaresSpawned = new List<GameObject>();

    //Start
    private void Start()
    {
        grid = new Grid(24, 25, cellSize, this.transform.position);
        for (int i = 0; i < 16; i++)
        {
            squareIconSpawned = Instantiate(squareIcon, squareStartPos, Quaternion.identity);
            squareIconSpawned.GetComponent<SpriteRenderer>().sortingOrder = 2;
            squaresSpawned.Add(squareIconSpawned);
        }

    }
    private void Update()
    {
        //On Mouse Left click.
        if (Input.GetMouseButtonDown(0))
        {
            //Buildings selected.
            if (redCount < 1 && grid.GetValue(GetWorldPosOffset()) > -1)
            {
                int x, y;
                GetXYValuesOfBuildings(out x, out y);
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        Vector3 newVector = Vector3.zero;
                        newVector.x += cellSize * i;
                        newVector.y += cellSize * j;
                        Vector3 worldPos = GetWorldPosOffset() + newVector;
                        SetValueAndSpawnBuilding((grid.GetGridMidPoint(GetWorldPosOffset()) + newVector), j + y * i, worldPos);
                    }
                }
                ChangeBuildingToEmpty();
            }
            if (BuildingType.selectedBuilding == BuildingType.BuildingTypes.empty)
            {
                //Get info.
                Debug.Log("Selected: " + grid.GetValue(GetWorldPosOffset()));
                DetailsController.Instance.SetValues(grid.GetValue(GetWorldPosOffset()));
            }
        }
        //On Mouse Hover.
        if (grid.GetValue(GetWorldPosOffset()) > -1)
        {
            redCount = 0;
            int x, y;
            GetXYValuesOfBuildings(out x, out y);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Vector3 newVector = Vector3.zero;
                    newVector.x += cellSize * i;
                    newVector.y += cellSize * j;
                    Vector3 worldPos = GetWorldPosOffset() + newVector;
                    SelectGround((grid.GetGridMidPoint(GetWorldPosOffset()) + newVector), j + y * i, worldPos);
                }
            }
        }
        else
        {
            ResetSquares();
        }
    }
    //Set value of the grid and spawn building.
    private void SetValueAndSpawnBuilding(Vector3 checkPos, int squareNum, Vector3 worldPos)
    {
        GameObject buildingSquare = Instantiate(squareIcon, checkPos, Quaternion.identity);
        buildingSquare.GetComponent<SpriteRenderer>().color = buildingColor;
        grid.SetValue(worldPos, ((int)buildingValue));
    }
    //Check ground buildable.
    private void SelectGround(Vector3 checkPos, int squareNum, Vector3 worldPos)
    {
        //Debug.Log(squareNum);
        currentMidPoint[squareNum] = checkPos;
        if (currentMidPoint[squareNum] != activeMidPoint)
        {
            squaresSpawned[squareNum].transform.position = currentMidPoint[squareNum];
            activeMidPoint = currentMidPoint[squareNum];
            ChangeSquareColor(grid.GetValue(worldPos), squareNum);
        }
    }

    //Change color of square to show area availability.
    private void ChangeSquareColor(float value, int squareNum)
    {
        if (value > 0)
        {
            squaresSpawned[squareNum].GetComponent<SpriteRenderer>().color = Color.red;
            redCount++;
        }
        else
        {
            squaresSpawned[squareNum].GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (value == -1)
        {
            redCount++;
        }
    }
    //Get size of the building as XY.
    private void GetXYValuesOfBuildings(out int x, out int y)
    {
        x = y = 0;
        int buildingIdx = GetScriptableIdx();
        //Debug.Log(BuildingType.selectedBuilding);
        if (buildingIdx != -1)
        {
            x = buildings[buildingIdx].buildingXValue;
            y = buildings[buildingIdx].buildingYValue;
            //Set other datas.
            buildingColor = buildings[buildingIdx].buildingColor;
            buildingValue = buildings[buildingIdx].buildingNumber;
        }
    }
    private int GetScriptableIdx()
    {
        int returnValue = -1;
        for (int i = 0; i < buildings.Length; i++)
        {
            if (buildings[i].types == BuildingType.selectedBuilding)
            {
                returnValue = i;
            }
        }
        return returnValue;
    }
    //Change to empty on click.
    private void ChangeBuildingToEmpty()
    {
        ResetSquares();
        OnBuildingTypeChanged.Invoke(BuildingType.BuildingTypes.empty);
        buildingValue = 0;
    }
    //Reset square positions.
    private void ResetSquares()
    {
        for (int i = 0; i < squaresSpawned.Count; i++)
        {
            squaresSpawned[i].transform.position = squareStartPos;
        }
    }
    public void SelectPowerPlant()
    {
        GridController.OnBuildingTypeChanged.Invoke(BuildingType.BuildingTypes.powerPlant);
    }
    public void SelectBarracks()
    {
        GridController.OnBuildingTypeChanged.Invoke(BuildingType.BuildingTypes.barracks);
    }
    //Get on click world pos.
    #region getWorldPos
    private Vector3 GetWorldPosOffset()
    {
        Vector3 vec = GetMouseWorldPos() - this.transform.position;
        vec.z = 0f;
        return vec;
    }
    private Vector3 GetMouseWorldPos()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    private Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    #endregion
}
