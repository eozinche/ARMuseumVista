using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject tag;


    [Serializable]
    public struct itemInfo {
        public string location;
        public string fullName;
        public string tagLine;
        public string description;
    }

    [SerializeField] public itemInfo item; 

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
