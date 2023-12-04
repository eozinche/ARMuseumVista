using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class infoPanelManager : MonoBehaviour
{
    public Tap3D tap3D;
    public Tap2D tap2D;
    public bool flatTargetMode; 
    public bool panelInPlace;
    
    public TextMeshProUGUI info;
    public Michsky.UI.ModernUIPack.ButtonManagerBasic buttonManager;
    public TextMeshProUGUI locationInfo;
    public GameObject activeObj; 

    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) panelIn(); 
        if (Input.GetKeyDown(KeyCode.Alpha2)) panelOut(); 
    }

    public void updateInfo(){

        Interactable thisItem = null;

        if (flatTargetMode) { if (tap2D != null) thisItem = tap2D.selected.GetComponent<Interactable>(); }
        else { 
            if (tap3D != null) {
                thisItem = tap3D.selected.GetComponent<Interactable>(); 
                activeObj = tap3D.selected;
                }
            }

        if (thisItem != null) {
        info.text = "<size=58><b>" + thisItem.item.fullName + "</b></size>" + "\n\n" + "<b>" + thisItem.item.tagLine + "</b>" + "\n\n"  + thisItem.item.description;
        buttonManager.buttonText = thisItem.item.location;
        locationInfo.text = thisItem.item.location;

        Scene scene = SceneManager.GetActiveScene();
        string sceneName = scene.name;
        if(sceneName.Contains("theMet")) {
            info.text = "<size=58><b>" + thisItem.item.fullName + "</b></size>" + "\n\n" + "<b>" + thisItem.item.tagLine + "\n\n" + thisItem.item.description + "</b>";
            buttonManager.buttonText = "<b>" + thisItem.item.location + "</b>";
            locationInfo.text = "<b>" + thisItem.item.location + "</b>";
        }
        }
    }

    public void panelIn(){
        if (anim != null) {
            anim.Play("BottomPanelIn", 0, 0f);
            panelInPlace = true;
        }
    }

    public void panelOut(){
        if (anim != null) {
            anim.Play("BottomPanelOut", 0, 0f);
            panelInPlace = false;
        }
    }
}
