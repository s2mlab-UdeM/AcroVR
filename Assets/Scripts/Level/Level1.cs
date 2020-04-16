using UnityEngine;

public class Level1 : MonoBehaviour
{
    bool isPaused = false;
    bool isTakeOff = false;

    bool bFirstView = false;

    void Start ()
    {
        ToolBox.GetInstance().GetManager<StatManager>().ProfileLoad("Student1");

        ToolBox.GetInstance().GetManager<DrawManager>().SetAnimationSpeed(1);  // 1(fast) ~ 5(slow)
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 200, 10, 100, 50), "Load"))
        {
            /*        float v = float.Parse(missionInput.text);
                    float difference  = Mathf.Abs(v - mission.solution.Velocity);
                    if (difference < 0.5) transform.parent.GetComponentInChildren<StatManager>().info.Score = 100;
                    else if(difference < 1.0) transform.parent.GetComponentInChildren<StatManager>().info.Score = 80;
                    else transform.parent.GetComponentInChildren<StatManager>().info.Score = 50;*/

//            ToolBox.GetInstance().GetManager<AniGraphManager>().TaskOffGraphOff();
            ToolBox.GetInstance().GetManager<GameManager>().MissionLoad();

            //        MainParameters.Instance.joints.duration = mission.goal.Duration;
            //        MainParameters.Instance.joints.takeOffParam.anteroposteriorSpeed = v;

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
//            ToolBox.GetInstance().GetManager<AniGraphManager>().TaskOffGraphOff();
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
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            bFirstView = !bFirstView;
        }
        if(bFirstView)
        {
            Camera.main.transform.position = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.position;
            Camera.main.transform.rotation = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.rotation;
        }

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
