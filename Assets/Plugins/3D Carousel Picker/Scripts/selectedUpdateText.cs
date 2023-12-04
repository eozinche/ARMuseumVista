using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class selectedUpdateText : MonoBehaviour 
{
	public CarouselPicker carouselPicker;
    public TextMeshProUGUI m_ItemName;
    public TextMeshProUGUI m_ItemNameHelper;

    void Start()
	{
		carouselPicker.OnSelectedObjectChange += UpdateText;
		UpdateText();
	}

	void UpdateText()
	{
		if (carouselPicker.getSelectedObject().name.Contains("_0")){
			m_ItemName.text = "Thannhauser Collection";
			
		} else if (carouselPicker.getSelectedObject().name.Contains("_1")){
			m_ItemName.text = "Going Dark";

		} else {
			m_ItemName.text = "Only the Young: Art in Korea";

		}
        m_ItemNameHelper.text = m_ItemName.text;
		//m_ItemName.text = carouselPicker.getSelectedObject().name;
        //m_ItemNameHelper.text = carouselPicker.getSelectedObject().name;
    }
}
