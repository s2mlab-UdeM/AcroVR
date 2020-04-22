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
	public PlayController playController;

	void Awake()
	{

	}

	void Start()
	{

		drawManager = ToolBox.GetInstance().GetManager<DrawManager>();
		//slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

		//isPaused = !isPaused;
		//ToolBox.GetInstance().GetManager<DrawManager>().PauseAvatar(isPaused);
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