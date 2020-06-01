using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
//    public GameObject mainCanvas;
    public Animator animatorComponent;

    private void Start()
    {
        animatorComponent.GetComponent<Animator>();
    }

    void ToMainMenu()
    {
        ToolBox.GetInstance().GetManager<LevelManager>().NextLevel();
        Destroy(this.gameObject);
    }

    void Update()
    { 
        if (Input.anyKeyDown)
        {
            animatorComponent.Play("Curaphic Splash Fade-out");
            //            mainCanvas.SetActive(true);
            Invoke("ToMainMenu", 0.5f);
        }
    }
}
