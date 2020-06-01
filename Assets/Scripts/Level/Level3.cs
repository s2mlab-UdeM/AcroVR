using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level3 : MonoBehaviour
{
    bool isPaused = false;
    bool isTakeOff = false;

    bool bFirstView = false;

    //    public SwitchVR vr;

    public GameObject cube;
    public GameObject TabCanvas;
    public GameObject MenuCanvas;
    public GameObject MainFloor;

    void Start()
    {
        ToolBox.GetInstance().GetManager<StatManager>().ProfileLoad("Student1");
        ToolBox.GetInstance().GetManager<DrawManager>().SetAnimationSpeed(3);  // 1(fast) ~ 5(slow)

//        TabCanvas.SetActive(false);
    }

    /*    void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width - 200, 10, 100, 50), "Load"))
            {
                ToolBox.GetInstance().GetManager<GameManager>().MissionLoad();
                ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar(1);
            }

            if (GUI.Button(new Rect(Screen.width - 200, 70, 100, 50), "Load 2"))
            {
                ToolBox.GetInstance().GetManager<AniGraphManager>().TaskOffGraphOff();
                ToolBox.GetInstance().GetManager<GameManager>().MissionLoad();
                ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar(2);
            }
            if (GUI.Button(new Rect(Screen.width - 200, 130, 100, 50), "Result"))
            {
                ToolBox.GetInstance().GetManager<AniGraphManager>().TaskOffGraphOff();
                ToolBox.GetInstance().GetManager<AniGraphManager>().ResultGraphOn();
            }
            if (GUI.Button(new Rect(Screen.width - 200, 190, 100, 50), "Save"))
            {
                ToolBox.GetInstance().GetManager<GameManager>().SaveFile();
            }
            if (GUI.Button(new Rect(Screen.width - 200, 250, 100, 50), "Replay"))
            {
                ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar(1);
            }

            if (GUI.Button(new Rect(Screen.width - 200, 310, 100, 50), "Pause"))
            {
                isPaused = !isPaused;
                ToolBox.GetInstance().GetManager<DrawManager>().PauseAvatar(isPaused);
            }

            if (GUI.Button(new Rect(Screen.width - 200, 370, 100, 50), "TakeOffGraph"))
            {
                if (!isTakeOff)
                {
                    isTakeOff = true;
                    ToolBox.GetInstance().GetManager<AniGraphManager>().TaskOffGraphOn();
                }
                else
                {
                    isTakeOff = false;
                    ToolBox.GetInstance().GetManager<AniGraphManager>().TaskOffGraphOff();
                }
            }

            GUI.TextField(new Rect(Screen.width - 200, 470, 100, 30), ToolBox.GetInstance().GetManager<StatManager>().dofName);
        }*/

    public void ShowTab()
    {
        MenuCanvas.SetActive(false);
//        GetComponent<Rotation>().isRotating = false;
//        GetComponent<Rotation>().target.transform.rotation = Quaternion.identity;
//        GetComponent<Rotation>().target.transform.Translate(6, 0, 0);
//        GetComponent<Rotation>().enabled = false;

        TabCanvas.SetActive(true);
//        MainFloor.SetActive(true);
//        SceneManager.LoadScene("AcroVR-UI05");
    }

    public void CloseTab()
    {
        TabCanvas.SetActive(false);
//        GetComponent<Rotation>().enabled = true;
//        GetComponent<Rotation>().isRotating = true;
//        GetComponent<Rotation>().target.transform.Translate(-6, 0, 0);

        MenuCanvas.SetActive(true);
        //        MainFloor.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            bFirstView = !bFirstView;
        }
        if (bFirstView)
        {
            Camera.main.transform.position = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.position;
            Camera.main.transform.rotation = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.rotation;
        }

        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 5.0f;
        cube.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

        /*        if (Input.GetKeyDown(KeyCode.B))
                {
                    vr.switchTo2DMode();
                }

                if (Input.GetKeyDown(KeyCode.V))
                {
                    vr.switchToVrMode();
                }*/

        //        transform.Rotate(new Vector3(0,0,1), 20.0f * Time.deltaTime);

        //        if (MainParameters.Instance.joints.nodes == null) return;

        //        if (!ToolBox.GetInstance().GetManager<DrawManager>().animateON)
        //        {
        //            ToolBox.GetInstance().GetManager<AniGraphManager>().TaskOffGraphOn();
        //        }

        /*        if (Input.GetKeyDown(KeyCode.A)) ToolBox.GetInstance().GetManager<DrawManager>().PlayAvatar();
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (Time.timeScale == 0)
                        Time.timeScale = 0.2f;
                    else
                        Time.timeScale = 0;
                }*/
    }
}
