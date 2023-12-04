using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale3D : MonoBehaviour
{
    public GameObject targetObj;
    private Animator anim;

    void Start()
    {
        anim = targetObj.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (anim != null)
            {
                anim.Play("ScaleUp", 0, 0f);
            }
        }
    }
}
