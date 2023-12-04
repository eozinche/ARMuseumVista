using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getSelected : MonoBehaviour 
{
	/// <summary>
	/// The carousel picker.
	/// </summary>
	public CarouselPicker carouselPicker;

	/// <summary>
	/// The this text.
	/// </summary>
	public Text thisText;

	void Start()
	{
		thisText = GetComponent<Text>();

		carouselPicker.OnSelectedObjectChange += UpdateText;
		UpdateText();
	}


	void UpdateText()
	{
		thisText.text = carouselPicker.getSelectedObject().name;

	}
}
