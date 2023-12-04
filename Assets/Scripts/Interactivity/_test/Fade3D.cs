using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade3D : MonoBehaviour
{

    //public GameObject targetObj;
    public Renderer targetObj;
    public float FadeRate;
    public float targetAlpha;

    void Start()
    {
        /*
        Color color = targetObj.material.color; // targetObj.GetComponent<Material>();
        color.a = targetAlpha;
        targetObj.material.color = color;
*/
    }

    void Update()
    {
       if( Input.GetKeyDown(KeyCode.F) ){
            print("F pressed!");
            //   Invoke("startFadein", 1);
            Color color = targetObj.material.color; // targetObj.GetComponent<Material>();
            color.a = targetAlpha;
            targetObj.material.color = color;
        }

    }

    IEnumerator FadeIn(Material mat)
    {
        targetAlpha = 1.0f;
        Color curColor = mat.color;
        while (Mathf.Abs(curColor.a - targetAlpha) > 0.0001f)
        {
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, FadeRate * Time.deltaTime);
            mat.color = curColor;
            yield return null;
        }
    }

    void startFadein()
    {
       // StartCoroutine(FadeIn());
    }



    

}
