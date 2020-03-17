using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
///     Scene Manager and UI Manager
/// </summary>

public class LoadSceneManager : MonoBehaviour {

	protected Stack<int> sceneHistoryStack;		//-- Linked to "Back" button Function
	protected int currentSceneIndex;	//-- Linked to "Back" button Function

	//-- Quits Build Function
	public void QuitBuild()
	{
		Application.Quit();
	}

	//**** Load Scene Section
	//-- onClick() Load Scene Functions		//-- Add scenes from "Scenes In Build" here		//-- Please, organize scenes alphabetically
	#region <-- TOP
	public void LoadScenePlayerProfile()
	{
		LoadRequestedScene(2);
	}

	public void LoadSceneHallOfFame()
	{
		LoadRequestedScene(1);
	}

	public void LoadSceneLastPlayerProfile()
	{
		LoadRequestedScene(3);
	}

	public void LoadSceneMainMenu()
	{
		LoadRequestedScene(0);
	}

	public void LoadSceneNewPlayerProfile()
	{
		LoadRequestedScene(4);
	}

	public void LoadSceneOptions()
	{
		LoadRequestedScene(6);
	}

	public void LoadSceneTraining()
	{
		LoadRequestedScene(5);
	}
	#endregion <-- BOTTOM

	//-- Load Scene Functions
	#region <-- TOP
	//-- Load onClick() Funtion		//-- Linked with CreateLastSceneStack
	public void LoadRequestedScene(int loadRequestedScene)
	{
		if (sceneHistoryStack == null)
		{
			CreateLastSceneStack();     //-- Creates sceneHistoryStack
		}

		currentSceneIndex = SceneManager.GetActiveScene().buildIndex;	//-- Save loadRequestedScene variable
		sceneHistoryStack.Push(currentSceneIndex);	//-- Stack loadRequestedScene variable
		SceneManager.LoadScene(loadRequestedScene);	//-- lOAD Requested Scene
	}

	//-- "Back" button Function		//-- Linked with LoadRequestedScene & CreateLastSceneStack functions
	public void LoadLastScene()
	{
		SceneManager.LoadScene(sceneHistoryStack.Pop());
	}

	//-- Create Stack<> button Function		//-- Linked with LoadRequestedScene
	void CreateLastSceneStack()
	{
		sceneHistoryStack = new Stack<int>();
	}
	#endregion <-- BOTTOM


	//**** TEST	Section
	#region <-- Deleted TOP
	//-- Deleted When finished
	//-- Loads last scene found in Unity menu
	public void LoadFinalScene()
	{
		LoadRequestedScene(SceneManager.sceneCountInBuildSettings-1);
	}
	#endregion <-- Deleted BOTTOM
}