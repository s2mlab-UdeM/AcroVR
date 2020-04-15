using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///	 
/// </summary>

public class SliderPlayAnimation : MonoBehaviour
{
	// Variables
	DrawManager drawManager;
	public Slider slider;

	void Start()
	{
		drawManager = ToolBox.GetInstance().GetManager<DrawManager>();
	}

	void Update()
	{
		slider.value = drawManager.frameN;
	}


	///===///  OnClick() functions
	#region		<-- TOP

	public void OnPlayAnimationSlider()
	{
		slider.minValue = 1f;
		slider.maxValue = (float)drawManager.numberFrames;

		drawManager.frameN = (int)slider.value;
	}

	#endregion		<-- BOTTOM

}