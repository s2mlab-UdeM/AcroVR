using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

public enum SceneState
{
    Logo,
    MainMenu,
    BaseLevel1,
    BaseLevel2,
    BaseLevel3,
    EndScreen,
    Profile,
    Training,
}

public class LevelManager : MonoBehaviour
{
    public SceneState currentState = 0;
    Stack<GameObject> sceneStack = new Stack<GameObject>();

//    private GameObject trampoline;

    public bool IsLevelScene()
    {
        if (currentState == SceneState.MainMenu || currentState == SceneState.EndScreen)
            return false;
        else
            return true;
    }

    private void Awake()
    {
//        trampoline = Resources.Load<GameObject>("trampoline");
        currentState = SceneState.Logo;
    }

    public void Init()
    {
        currentState = SceneState.MainMenu;
    }

    public void NextLevel()
    {
        if (currentState == SceneState.EndScreen) Init();
        else
        {
            currentState++;
        }

        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(currentState.ToString());
        while (!op.isDone)
        {
            yield return null;
        }

        switch (currentState)
        {
            case SceneState.MainMenu:
                CreateLevel<BaseMenu>();
                break;
            case SceneState.BaseLevel1:
                CreateLevel<BaseLeve1>();
                break;
            case SceneState.BaseLevel2:
                break;
            case SceneState.BaseLevel3:
                break;
            case SceneState.Profile:
                CreateLevel<BaseProfile>();
                break;
            case SceneState.Training:
                CreateLevel<BaseTraining>();
                break;
            default:
                break;
        }
    }

    private void CreateLevel<T>() where T : LevelBase
    {
        var go = new GameObject(typeof(T).ToString());
        PushScene(go);

        sceneStack.Peek().transform.parent = this.gameObject.transform;
        sceneStack.Peek().AddComponent<T>();
//        sceneStack.Peek().AddComponent<T>().SetPrefab(trampoline);
//        sceneStack.Peek().GetComponent<T>().CreateLevel();
    }

    private void PushScene(GameObject go)
    {
        if (sceneStack.Count > 0)
        {
            sceneStack.Pop();
            GameObject.Destroy(transform.GetChild(0).gameObject);
        }
        sceneStack.Push(go);
    }

    public void CurrentScreen()
    {
        StartCoroutine(LoadScene());
    }

    public void GotoScreen(string screen)
    {
        if (screen == "Profile") currentState = SceneState.Profile;
        if (screen == "MainMenu") currentState = SceneState.MainMenu;
        if (screen == "Training") currentState = SceneState.Training;
        if (screen == "BaseLevel1") currentState = SceneState.BaseLevel1;
        StartCoroutine(LoadScene());
        //        if (screen == "EndScreen") currentState = SceneState.EndScreen;
        //        SceneManager.LoadScene(screen);
    }
}
