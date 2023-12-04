using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tap3D : MonoBehaviour
{
    public GameObject[] items;
    public GameObject selected; 
    public infoPanelManager infoPanel;
    public bool hitNothing; 

    void Start()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetComponent<Interactable>().tag != null) items[i].GetComponent<Interactable>().tag.SetActive(false);
        }
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0)) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
        //if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) //if (Input.GetMouseButtonDown(0))
        {
            for(int i = 0; i < items.Length; i++){
                items[i].GetComponent<Outline>().enabled = false;
               if(items[i].GetComponent<Interactable>().tag != null) items[i].GetComponent<Interactable>().tag.SetActive(false);

                if(items[i] == getClickedObject(out RaycastHit hit)) {
                   print("clicked on a 3D item: " + items[i].name); 
                   selected = items[i];

                    //activate a tag and outline
                   if (selected.GetComponent<Interactable>().tag != null) selected.GetComponent<Interactable>().tag.SetActive(true);
                   selected.GetComponent<Outline>().enabled = true;

                   infoPanel.updateInfo();
                  
                   if(!infoPanel.panelInPlace) infoPanel.panelIn();  //slide in the info panel
                }
            }
            
            if ((!isPointerOverUIObject() && hitNothing) && infoPanel.panelInPlace) {
                infoPanel.panelOut();  //slide out the info panel
                selected = null;
                infoPanel.activeObj = null;
                infoPanel.gameObject.SendMessage("HideAllDirections", SendMessageOptions.DontRequireReceiver);
            }
        }

        //if((Input.GetMouseButtonUp(0)) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)) print("click is off");
    }


    // MARK GET CLICKED OBJECT
    GameObject getClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray;

        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        } else
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        }

        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if (!isPointerOverUIObject()) {
                for (int i = 0; i < items.Length; i++)
                {
                    if (hit.collider.gameObject.tag == items[i].name) target = items[i];
                    else print("Hit something called " + hit.collider.gameObject.name);
                }
            }
            hitNothing = false;
        } else hitNothing = true;
        return target;
    }

    
    private bool isPointerOverUIObject()
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);

        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            ped.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }else {
            ped.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
        }
            
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        return results.Count > 0;
    }

}



