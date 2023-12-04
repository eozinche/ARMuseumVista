using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tap2D : MonoBehaviour
{

    public GameObject[] items;
    public GameObject selected; 
    public infoPanelManager infoPanel;
    public bool hitNothing; 

    void Start()
    {
        for (int i = 0; i < items.Length; i++)
        {
            //items[i].GetComponent<Interactable>().tag.SetActive(false);
        }
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0)) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
        //if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) //if (Input.GetMouseButtonDown(0))
        {
            for(int i = 0; i < items.Length; i++){
                items[i].GetComponent<Outline>().enabled = false;
                //items[i].GetComponent<Interactable>().tag.SetActive(false);

                if(items[i] == getClickedObject(out RaycastHit hit)) {
                   print("clicked on a 3D item: " + items[i].name); 
                   selected = items[i];

                   //activate a tag and outline
                   //selected.GetComponent<Interactable>().tag.SetActive(true);
                   if(selected.GetComponent<Outline>() != null) selected.GetComponent<Outline>().enabled = true;

                   infoPanel.updateInfo();
                   //slide in the info panel
                   if(!infoPanel.panelInPlace) infoPanel.panelIn();
                }
            }
            
            if ((isPointerOverUIObject() || hitNothing) && infoPanel.panelInPlace) {
                //slide out the info panel
                infoPanel.panelOut();
                selected = null;
            } 
        }

        /*
        if((Input.GetMouseButtonUp(0)) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)) //if (Input.GetMouseButtonUp(0))
        {
            print("click is off");
        }*/
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
                // checkParent(hit.collider.gameObject);
                // target = hit.collider.transform.root.gameObject; //hit.collider.gameObject; 

                for (int i = 0; i < items.Length; i++)
                {
                    target = hit.collider.gameObject;
                }

                //if(target != null) Debug.Log("new target: " + hit.collider.gameObject.name + "     parent name: " + target.name); // + "    in-btw2: " + hit.collider.transform.parent.parent.name
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



