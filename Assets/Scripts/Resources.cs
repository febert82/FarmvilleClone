using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Resources : MonoBehaviour
{
    public float wood;
    public float stones;
    public float food;

    [Header("UI Reference")]
    public Text resourcesText;

    private void FixedUpdate()
    {
        resourcesText.text = "Wood: " + wood.ToString("F0") + " | Stones: "
           + stones.ToString("F0") + " | Food: " + food.ToString("F0");
    }
}
