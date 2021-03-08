using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public Text nameText;
    public Button btnDestroy;
    public Button btnUpgrade;

    private Build build;
    private Building selectedBuilding;
    private Resources resources;

    private void Awake()
    {
        build = FindObjectOfType<Build>();
        resources = FindObjectOfType<Resources>();
    }

    // Update is called once per frame
    void Update()
    {
        if (build.curSelectedGridElement != null && build.curSelectedGridElement.connectedBuilding != null)
        {
            selectedBuilding = build.curSelectedGridElement.connectedBuilding;
            nameText.text = selectedBuilding.objName + "\nLevel: " + selectedBuilding.info.level;
        }
        else
        {
            nameText.text = "No building selected";
            selectedBuilding = null;
        }

        btnDestroy.interactable = selectedBuilding;

        bool result = false;

        if (selectedBuilding)
        {
            if (resources.wood >= selectedBuilding.price.price_wood
                && resources.stones >= selectedBuilding.price.price_stone
                && resources.food >= selectedBuilding.price.price_food)
            {
                result = true;
            }

            btnUpgrade.interactable = result;
        }
        else
        {
            btnUpgrade.interactable = false;
        }
    }

    public void OnBtnUpgrade()
    {
        if (selectedBuilding)
        {
            selectedBuilding.UpgradeBuilding();
        }
    }

    public void OnBtnDestroy()
    {
        if (selectedBuilding)
        {
            build.curSelectedGridElement.occupied = false;
            build.buildings.builtObjects.Remove(selectedBuilding.gameObject);
            Destroy(selectedBuilding.gameObject);
        }
    }
}
