using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Scene Manager
/// </summary>

public class LoadSceneManager : MonoBehaviour
{
    ///*** Variable
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

    ///*** onClick() Load Scene Functions	
    #region		<=== TOP

    /// Please, organize scenes alphabetically
    public void LoadScenePlayerProfile()
    {
        LoadRequestedScene(2);
    }

    public void LoadSceneHighScore()
    {
        LoadRequestedScene(1);
    }

    public void LoadSceneLastPlayer()
    {
        LoadRequestedScene(3);
    }

    public void LoadSceneMainMenu()
    {
        LoadRequestedScene(0);
    }

    public void LoadSceneNewPlayer()
    {
        LoadRequestedScene(4);
    }

    public void LoadSceneOptions()
    {
        LoadRequestedScene(6);
    }

    public void LoadSceneTraining()
    {
        LoadRequestedScene(7);
    }
    #endregion		<=== BOTTOM


    ///*** Load Scene functions
    #region		<=== TOP

    /// Load onClick() && create back Button function		
    /// Linked with CreateLastSceneStack
    public void LoadRequestedScene(int loadRequestedScene)
    {
        if (sceneHistoryStack == null)
        {
            /// Create Stack<> button Function
            sceneHistoryStack = new Stack<int>();
        }

        /// Save current scene reference
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        /// Save currentSceneIndex to stack
        sceneHistoryStack.Push(currentSceneIndex);
        /// lOAD Requested Scene
        SceneManager.LoadScene(loadRequestedScene);
    }

    /// Back button Function
    /// Linked with LoadRequestedScene & CreateLastSceneStack functions
    public void LoadPreviousSceneStack()
    {
        /// null == true, go to main menu
        if (sceneHistoryStack == null)
        {
            LoadRequestedScene(0);
        }
        else
        {
            SceneManager.LoadScene(sceneHistoryStack.Pop());
        }
    }

    #endregion		<=== BOTTOM


    ///*** TEST Section
    #region		<=== TOP - Deleted when done 

    /// Deleted section When finished

    /// Loads last scene found in Unity menu
    public void TEST_LoadFinalScene()
    {
        SceneManager.LoadScene(loadTestScene.name);
    }
    #endregion		<=== BOTTOM - Deleted when done
}