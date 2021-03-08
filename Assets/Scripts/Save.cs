using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.VFX;
using UnityEditor.UIElements;

[System.Serializable]
public class SavedProfile
{
    public float s_wood;
    public float s_stones;
    public float s_food;

    public List<BuildingInfo> buildingsSavedData = new List<BuildingInfo>();
}

public class Save : MonoBehaviour
{
    public SavedProfile profile;

    private Resources resources;
    private Buildings buildings;
    private Build build;

    private void Awake()
    {
        build = FindObjectOfType<Build>();
        resources = FindObjectOfType<Resources>();
        buildings = FindObjectOfType<Buildings>();

        //LoadGame();
    }

    private void SaveGame()
    {
        if (profile == null)
        {
            profile = new SavedProfile();
        }

        profile.s_wood = resources.wood;
        profile.s_stones = resources.stones;
        profile.s_food = resources.food;

        foreach (GameObject g in buildings.builtObjects)
        {
            BuildingInfo b = g.GetComponent<Building>().info;
            profile.buildingsSavedData.Add(b);
        }

        BinaryFormatter bf = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save.dat";

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        FileStream fs = File.Open(path, FileMode.OpenOrCreate);
        bf.Serialize(fs, profile);

        fs.Close();
    }

    private void LoadGame()
    {
        string pathToLoad = Application.persistentDataPath + "/save.dat";

        if (!File.Exists(pathToLoad))
        {
            Debug.Log("No saved profile found! Have you saved yet?");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(pathToLoad, FileMode.Open);
        SavedProfile loadedProfile = bf.Deserialize(fs) as SavedProfile;
        fs.Close();

        resources.wood = loadedProfile.s_wood;
        resources.stones = loadedProfile.s_stones;
        resources.food = loadedProfile.s_food;

        for (int i = 0; i < loadedProfile.buildingsSavedData.Count; i++)
        {
            BuildingInfo buildingFromSave = loadedProfile.buildingsSavedData[i];
            build.RebuildBuilding(buildingFromSave.id, buildingFromSave.connectedGridId, (int)buildingFromSave.level, buildingFromSave.yRot);
            Debug.Log(buildingFromSave.id);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) {
            SaveGame();
            Debug.Log("Game saved! in " + Application.persistentDataPath);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
            Debug.Log("Game loaded! from " + Application.persistentDataPath);
        }
    }
}
