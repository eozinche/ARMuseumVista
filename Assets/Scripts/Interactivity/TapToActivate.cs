using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class TapToActivate : MonoBehaviour
{

    public AudioClip[] notes;
    public AudioSource source;
    string button;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began){
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit Hit;
            if(Physics.Raycast(ray, out Hit)){
                button = Hit.transform.name;
                switch(button){
                    case "note1":
                        source.clip = notes[0];
                        source.Play();
                        break;
                    case "note2":
                        source.clip = notes[1];
                        source.Play();
                        break;
                    case "note3":
                        source.clip = notes[2];
                        source.Play();
                        break;
                    case "note4":
                        source.clip = notes[3];
                        source.Play();
                        break;
                }
            }
        }
    }
}
