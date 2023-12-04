using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class custom_theMet : MonoBehaviour
{
    public GameObject button;
    private Animator buttonAnim;
    public float mDelay;
    private IEnumerator mRoutine;
    

    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (buttonAnim == null) buttonAnim = button.GetComponent<Animator>();
        startFadein(buttonAnim, mDelay);
    }

    private void OnDisable()
    {
        //StopAllCoroutines();
        if(mRoutine != null) StopCoroutine(mRoutine);
        buttonAnim.Play("buttonDisappear", 0, 0f);
        print("button Disappeared!");
    }

    private void startFadein(Animator anim, float delay)
    {
        mRoutine = Appear(anim, delay);
        StartCoroutine(mRoutine);
    }

    IEnumerator Appear(Animator anim, float delay)
    {
        yield return new WaitForSeconds(delay);

        buttonAnim.Play("buttonAppear", 0, 0f);
    }

       
}
