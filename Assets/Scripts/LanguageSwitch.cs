using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///      
/// </summary>

public class LanguageSwitch : MonoBehaviour
{
	// Variables
	// [Header("SectionTitle")]	[Tooltip("HighlightInfo")]
	public enum LanguageAvailable
	{
		Default,
		English,
		French,
		Spanish,
	};
	public LanguageAvailable currentLanguage;

	void Start()
	{
		///***  Start declaration
		#region			<-- TOP
		
		///
		
		
		#endregion		<-- BOTTOM
		
		

	}


	void Update()
	{
		///***  Update declaration
		#region		<-- TOP
		
		///
		
		
		#endregion		<-- BOTTOM

		

	}


	// Switch Language setup
	#region		<-- TOP

	///  Switch language
	#region		<-- TOP
	public void LanguageAvailableSwitch()
	{
		switch (currentLanguage)
		{
			case LanguageAvailable.Default:
				LanguageEnglish();
				break;
			case LanguageAvailable.English:
				LanguageEnglish();
				break;
			case LanguageAvailable.French:
				LanguageFrench();
				break;
			case LanguageAvailable.Spanish:
				LanguageSpanish();
				break;
			default:
				break;
		}
	}
	#endregion		<-- BOTTOM


	///  Switch language functions
	#region		<-- TOP
	public void LanguageDefault()
	{
		LanguageEnglish();

	}

	public void LanguageEnglish()
	{
		// Activate Language function

	}

	public void LanguageFrench()
	{
		// Activate Language function


	}

	public void LanguageSpanish()
	{
		// Activate Language function

	}

	public void EnemyIdle()
	{
		// Activate Language function

	}
	#endregion		<-- BOTTOM

	#endregion		<-- BOTTOM


}