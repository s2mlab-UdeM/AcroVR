using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
///     Scene Manager and UI Manager
/// </summary>

public class LoadSceneManager : MonoBehaviour 
{
	///===/// Variable
	[Header("UI Developer Tool Bypass")]
	[Tooltip("Activate to load Test Scene")]
	public bool TestScene = false;
	/// Test variables and checks
	public Object loadTestScene;

	/// Linked to "Back" button Function
	protected Stack<int> sceneHistoryStack;

	/// Linked to "Back" button Function
	protected int currentSceneIndex;


	/// Quits Build Function
	public void QuitBuild()
	{
		Application.Quit();
	}

	///===/// Load Scene Section
	#region		<=== TOP

	///--- onClick() Load Scene Functions		
			/// Please, organize scenes alphabetically

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
	#endregion		<=== BOTTOM


	///===/// Load Scene functions
	#region		<=== TOP

	///--- Load onClick() Funtion		
			/// Linked with CreateLastSceneStack
	public void LoadRequestedScene(int loadRequestedScene)
	{
		if (sceneHistoryStack == null)
		{
			/// Creates sceneHistoryStack
			CreateLastSceneStack();     
		}

		/// Save loadRequestedScene variable
		currentSceneIndex = SceneManager.GetActiveScene().buildIndex;	
		/// Stack loadRequestedScene variable
		sceneHistoryStack.Push(currentSceneIndex);	
		/// lOAD Requested Scene
		SceneManager.LoadScene(loadRequestedScene);	
	}

	///--- Back button Function
			/// Linked with LoadRequestedScene & CreateLastSceneStack functions
	public void LoadLastScene()
	{
		SceneManager.LoadScene(sceneHistoryStack.Pop());
	}

	///-- Create Stack<> button Function
			/// Linked with LoadRequestedScene
	void CreateLastSceneStack()
	{
		sceneHistoryStack = new Stack<int>();
	}
	#endregion <-- BOTTOM


	///******/// TEST Section
	#region		<=== TOP - Deleted when done 

	///-- Deleted section When finished

	///-- Loads last scene found in Unity menu
	public void TEST_LoadFinalScene()
	{
		SceneManager.LoadScene(loadTestScene.name);
	}
	#endregion		<=== BOTTOM - Deleted when done
}