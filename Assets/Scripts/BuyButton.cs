using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public int connectedBuildingId;

    public Building connectedBuilding;
    public Text resourcesText;

    private Button btn;
    private Resources resources;

    private void Awake()
    {
        btn = GetComponent<Button>();
        resources = FindObjectOfType<Resources>();

        Buildings buildings = FindObjectOfType<Buildings>();

        foreach (GameObject gO in buildings.buildables)
        {
            Building b = gO.GetComponent<Building>();

            if (b.info.id == connectedBuildingId)
            {
                connectedBuilding = b;
                break;
            }
        }

        resourcesText.text = connectedBuilding.price.price_wood + " Wo. | "
            + connectedBuilding.price.price_stone + " St. | "
            + connectedBuilding.price.price_food + " Fo. ";
    }

    // Update is called once per frame
    void Update()
    {
        bool result = false;

        if (resources.wood >= connectedBuilding.price.price_wood
            && resources.stones >= connectedBuilding.price.price_stone
            && resources.food >= connectedBuilding.price.price_food)
        {
            result = true;
        }

        btn.interactable = result;
    }
}
