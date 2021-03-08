using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Build : MonoBehaviour
{
    public GridElement curSelectedGridElement;
    public GridElement curHoveredGridElement;

    public GridElement[] grid;

    [Header("Color")]
    public Color colOnHover = Color.white;
    public Color colOnOccupied = Color.red;

    public Buildings buildings;

    private RaycastHit mouseHit;

    private bool buildInProgress;

    private GameObject currentCreatedBuildable;

    private Color colOnNormal;

    private void Awake()
    {
        colOnNormal = grid[0].GetComponentInChildren<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out mouseHit))
        {
            GridElement g = mouseHit.transform.GetComponent<GridElement>();

            if (!g)
            {
                if (curHoveredGridElement)
                {
                    curHoveredGridElement.GetComponent<MeshRenderer>().material.color = colOnNormal;
                    return;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                curSelectedGridElement = g;
            }

            if (g != curHoveredGridElement)
            {
                if (!g.occupied)
                {
                    mouseHit.transform.GetComponent<MeshRenderer>().material.color = colOnHover;
                }
                else
                {
                    mouseHit.transform.GetComponent<MeshRenderer>().material.color = colOnOccupied;
                }
            }

            if (curHoveredGridElement && curHoveredGridElement != g)
            {
                curHoveredGridElement.GetComponent<MeshRenderer>().material.color = colOnNormal;
            }

            curHoveredGridElement = g;
        }
        else
        {
            if (curHoveredGridElement)
            {
                curHoveredGridElement.GetComponent<MeshRenderer>().material.color = colOnNormal;
            }
        }

        MoveBuilding();
        PlaceBuilding();
    }

    public void OnBtnCreateBuilding(int id)
    {
        if (buildInProgress)
        {
            return;
        }

        GameObject g = null;

        foreach (GameObject gO in buildings.buildables)
        {
            Building b = gO.GetComponent<Building>();

            if (b.info.id == id)
            {
                g = b.gameObject;
            }
        }

        currentCreatedBuildable = Instantiate(g);

        currentCreatedBuildable.transform.rotation = Quaternion.Euler(0, transform.rotation.y - 225, 0);

        buildInProgress = true;
        
    }

    public void MoveBuilding()
    {
        if (!currentCreatedBuildable)
        {
            return;
        }

        currentCreatedBuildable.layer = 2;

        if (curHoveredGridElement)
        {
            currentCreatedBuildable.transform.position = curHoveredGridElement.transform.position;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(currentCreatedBuildable);
            currentCreatedBuildable = null;
            buildInProgress = false;
        }

        if (Input.GetMouseButton(2))
        {
            currentCreatedBuildable.transform.Rotate(transform.up * 5);
        }
    }

    public void PlaceBuilding()
    {
        if (!currentCreatedBuildable || curHoveredGridElement.occupied)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            buildings.builtObjects.Add(currentCreatedBuildable);
            curHoveredGridElement.occupied = true;

            Building b = currentCreatedBuildable.GetComponent<Building>();

            curHoveredGridElement.connectedBuilding = b;
            b.placed = true;

            b.info.connectedGridId = curHoveredGridElement.gridId;
            b.info.yRot = b.transform.localEulerAngles.y;

            b.UpgradeBuilding();

            currentCreatedBuildable = null;
            buildInProgress = false;
        }
    }

    public void RebuildBuilding(int buildingId, int gridId, int buildingLevel, float rotY)
    {
        GameObject g = null;

        foreach (GameObject gO in buildings.buildables)
        {
            Building b = gO.GetComponent<Building>();

            if (b.info.id == buildingId)
            {
                g = b.gameObject;
            }
        }

        GameObject building = Instantiate(g);
        buildings.builtObjects.Add(building);

        Building loadedBuilding = building.GetComponent<Building>();
        loadedBuilding.info.level = buildingLevel;
        loadedBuilding.placed = true;
        loadedBuilding.info.connectedGridId = gridId;

        GridElement myElement = grid[gridId].GetComponent<GridElement>();
        building.transform.position = myElement.transform.position;
        building.transform.rotation = Quaternion.Euler(0, rotY, 0);
        loadedBuilding.info.yRot = rotY;
        myElement.occupied = true;

        myElement.connectedBuilding = loadedBuilding;
    }
}
