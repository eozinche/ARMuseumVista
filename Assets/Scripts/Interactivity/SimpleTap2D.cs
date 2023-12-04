using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleTap2D : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject[] targets;
    public GameObject pressed;
    public bool hitNothing;


    float lastTapTime = 0;
    float doubleTapThreshold = 0.3f;

    void Start()
    {
        /*
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].SetActive(false);
        }
        */
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0)) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
        {
            
            for (int i = 0; i < targets.Length; i++)
            {
                if (buttons[i] == getClickedObject(out RaycastHit hit))
                {
                    pressed = buttons[i];
                    if (pressed.tag != "Exception")
                    {
                        for (int j = 0; j < targets.Length; j++)
                        {
                            targets[j].SetActive(false);
                        }
                    }
                    
                    targets[i].SetActive(true);
                }
            }

            if ((isPointerOverUIObject() || hitNothing))
            {
                pressed = null;
            }
        }

        DoubleTap();

    }


    void DoubleTap()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (Time.time - lastTapTime <= doubleTapThreshold)
                {
                    lastTapTime = 0;
                    //double tap deteceted. Do action.
                    for (int i = 0; i < targets.Length; i++)
                    {
                        targets[i].SetActive(false);
                    }
                }
                else lastTapTime = Time.time;
            }
        }
    }

    // MARK GET CLICKED OBJECT
    GameObject getClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray;

        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        }

        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if (!isPointerOverUIObject())
            {
                target = hit.collider.gameObject;
                print("which button is pressed: " + target.name);

                //if(target != null) Debug.Log("new target: " + hit.collider.gameObject.name + "     parent name: " + target.name); // + "    in-btw2: " + hit.collider.transform.parent.parent.name
            }
            hitNothing = false;
        }
        else hitNothing = true;
        return target;
    }


    private bool isPointerOverUIObject()
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);

        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            ped.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else
        {
            ped.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
        }

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        return results.Count > 0;
    }
}




