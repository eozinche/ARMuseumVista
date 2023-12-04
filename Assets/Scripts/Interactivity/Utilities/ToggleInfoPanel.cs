using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Toggle))]
public class ToggleInfoPanel : MonoBehaviour
{

    [HideInInspector] public Toggle toggleObject;
    public GameObject InfoPanel;

    void Start()
    {
        if (toggleObject == null)
            toggleObject = gameObject.GetComponent<Toggle>();

        toggleObject.onValueChanged.AddListener(TogglePanel);

    }

    void OnEnable()
    {
        if (toggleObject == null)
            return;
    }

    void TogglePanel(bool value)
    {
        if (toggleObject.isOn)
            InfoPanel.SetActive(true);
        else
            InfoPanel.SetActive(false);
    }
}
