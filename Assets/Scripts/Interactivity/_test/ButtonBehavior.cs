using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{

    public GameObject target;
    private BoxCollider collider;
    private SpriteRenderer renderer;
    private GameObject bttnText;

    void Start()
    {
        collider = GetComponent<BoxCollider>();
        renderer = GetComponent<SpriteRenderer>();
        bttnText = transform.GetChild(0).gameObject;
    }


    void Update()
    {
        if(target != null)
        {
            if(target.activeSelf)
            {
                if (collider.enabled) { 
                    collider.enabled = false;
                    renderer.enabled = false;
                    bttnText.SetActive(false);
                }  
            }else
            {
                if (!collider.enabled)
                {
                    collider.enabled=true;
                    renderer.enabled = true;  
                    bttnText.SetActive(true);
                }
            }

        } 
    }
}
