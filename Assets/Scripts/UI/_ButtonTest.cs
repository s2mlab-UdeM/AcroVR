using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///	 
/// </summary>

public class _ButtonTest : MonoBehaviour
{
	// Variables
	public int thePlayer = 0;
	public Slider sliderBar;
	public bool feetAvatarActive = false;
	public GameObject feetAvatar;
	public GameObject cloneAvatar;


	void Start()
	{
		//ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar(1);
		//DrawManager playerScript = ToolBox.GetInstance().GetManager<DrawManager>();
		//playerScript.frameN -= 10;

		//sliderBar.minValue = 0;
		//sliderBar.maxValue = 1;
		//sliderBar.wholeNumbers = false;
		//sliderBar.value = 0;
		feetAvatar.transform.gameObject.SetActive(false);
		feetAvatarActive = false;

	}

	void Update()
	{
		if (cloneAvatar == null)
		cloneAvatar = GameObject.Find("girl1(Clone)");

	}

	public void OnValueChanged(float value)
	{
		print("New Value: " + value);
	}

	/// CommentEmpty
	public void Test1()
	{
			ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar(1);

	}

	public void Test2()
	{
		ToolBox.GetInstance().GetManager<GameManager>().MissionLoad();

	}

	public void AvatarFeetTest()
	{

		feetAvatarActive = !feetAvatarActive;
		if (feetAvatarActive == false)
		{
			if (cloneAvatar != null)
			{
				if (cloneAvatar.activeInHierarchy == false)
				{
					cloneAvatar.SetActive(true);

				}
			}
			feetAvatar.transform.gameObject.SetActive(false);
/*			feetAvatar.transform.position = new Vector3(0, 1, 0)*/;
		}

		if (feetAvatarActive == true)
		{
			if (cloneAvatar != null)
			{
				if (cloneAvatar.activeInHierarchy == true)
				{
				cloneAvatar.SetActive(false);

				}
			}

			feetAvatar.transform.gameObject.SetActive(true);
		}

	}

}