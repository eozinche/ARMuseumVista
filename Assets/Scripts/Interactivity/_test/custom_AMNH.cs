using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class custom_AMNH : MonoBehaviour
{
    private infoPanelManager iPanel;
    private GameObject selectedItem;
    private string entryPrompt, entryLocation;
    public string[] entrances;
    public string[] entryLocations;
    //public GameObject[] directions;
    

    [Serializable]
    public struct Directions
    {
        public GameObject direction;
        public GameObject[] dirGraphics;
    }

    public Directions[] dir;

    void Start()
    {
        iPanel = GetComponent<infoPanelManager>();
        HideAllDirections();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ShowEntryInfo();
        }
    }

    // Fastest entry to each exhibited artifacts/specimens
    public void ShowEntryInfo() 
    {
        Interactable thisItem = iPanel.activeObj.GetComponent<Interactable>();

        HideAllDirections();

        /*
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.Log("--------------check point 2: index = " + i + "------------------");
            directions[i].transform.GetChild(0).gameObject.SetActive(false);
            directions[i].transform.GetChild(1).gameObject.SetActive(false);
            /*
            for (int j = 0; j < 1; j++)
            {
                directions[i].GetChild(j).gameObject.SetActive(true);
            }
        }*/

        int index = 0;
        
        if (iPanel.activeObj.tag == "Trex") // T.rex: Main entry
        {
            index = 0;
            entryPrompt = "Rapidly access through the <b>" + entrances[index] + "</b>!";

        } else if (iPanel.activeObj.tag == "BlueWhale") // Whale: Gilder entry
        {
            index = 1;
            entryPrompt = "Effortlessly enter through the <b>" + entrances[index] + "</b>—no waiting required!";
        } else if (iPanel.activeObj.tag == "Moai") // Moai: North entry
        {
            index = 2;
            entryPrompt = "Head to the <b>" + entrances[index] + "</b>—the line is shorter there!";
        }

        entryLocation = entryLocations[index];

        foreach (GameObject obj in dir[index].dirGraphics)
        {
            obj.gameObject.SetActive(true);
        }

        /*
        for (int j = 0; j < directions[index].transform.childCount; j++)
        {
            Debug.Log("--------------check point 3: index = " + index + "------------------");
            directions[index].transform.GetChild(j).gameObject.SetActive(true);
        }*/

        int waitTime = UnityEngine.Random.Range(0, 10);
        if (entryPrompt.Contains("—no waiting")) waitTime = 0;
        iPanel.info.text = "<size=60>" + entryPrompt + "</size>" + "\n\n<size=40>"  + entryLocation + "\n<b>Wait Time:</b> " + waitTime + " minutes</size>";
    }

    public void HideAllDirections()
    {
        print("  Hide All Directions!! ");
        foreach (Directions d in dir)
        {
            foreach (GameObject obj in d.dirGraphics)
            {
                obj.gameObject.SetActive(false);
            }
        }
    }
}
