using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public struct InputData
{
    public float Velocity;
    public float Distance;
    public float Duration;
}

[System.Serializable]
public struct PlayerInfo
{
    public string Name;
    public string Id;
    public float Score;
    public InputData input;
}

public class StatManager : MonoBehaviour
{
    public PlayerInfo info;
    private GameObject circlePrefab;
    private GameObject circlePrefab_shoulder;
    private GameObject circle;
    RaycastHit hit;

    private int previousFrameN = 0;
    public int previousFrameN2 = 0;

    public string dofName;

    void Start()
    {
        circlePrefab = (GameObject)Resources.Load("HandleCircle", typeof(GameObject));
        circlePrefab_shoulder = (GameObject)Resources.Load("HandleCircle_shoulder", typeof(GameObject));
    }

    public void ProfileLoad(string fileName)
    {
        ReadDataFromJSON(fileName);
    }

    public void ProfileSave()
    {
        if (info.Name != null)
        {
            WriteDataToJSON(info.Name);
        }
    }

    private void WriteDataToJSON(string fileName)
    {
        string jsonData = JsonUtility.ToJson(info, true);
        File.WriteAllText(fileName, jsonData);
    }

    private void ReadDataFromJSON(string fileName)
    {
        string dataAsJson = File.ReadAllText(fileName);
        info = JsonUtility.FromJson<PlayerInfo>(dataAsJson);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (!circle)
                {
                    transform.parent.GetComponentInChildren<DrawManager>().isEditing = true;
                    circle = Instantiate(circlePrefab, hit.collider.transform.position, Quaternion.identity);
                    circle.GetComponent<HandleCircle>().Init(transform.parent.GetComponentInChildren<DrawManager>().girl1, hit.collider.gameObject, transform.parent.GetComponentInChildren<DrawManager>().qf);
                    CameraRotate(hit.collider.gameObject.name);
                    AddNodeInDof();
                }
                else
                {
                    if (circle.transform.position == hit.collider.transform.position)
                    {
                        if (circle.GetComponent<HandleCircle>().target.name == "upper_arm.L")
                        {
                            if (transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation.eulerAngles.y != 90)
                            {
                                transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.Euler(Vector3.up * 90);
                                circle.transform.position = transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.Find("Petra.002/hips/spine/chest/chest1/shoulder.L/upper_arm.L").gameObject.transform.position;
//                                circle.GetComponent<HandleCircle>().rotateTarget = true;
                            }
                            else
                            {
                                DestroyHandleCircle();
                                transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.identity;
                            }
                        }
                        else if (circle.GetComponent<HandleCircle>().target.name == "upper_arm.R")
                        {
                            if (transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation.eulerAngles.y != 270)
                            {
                                transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.Euler(Vector3.up * -90);
                                circle.transform.position = transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.Find("Petra.002/hips/spine/chest/chest1/shoulder.R/upper_arm.R").gameObject.transform.position;
//                                circle.GetComponent<HandleCircle>().rotateTarget = true;
                            }
                            else
                            {
                                DestroyHandleCircle();
                                transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.identity;
                            }
                        }
                        else
                        {
                            DestroyHandleCircle();
                            transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.identity;
                        }
                    }
                    else
                    {
                        DestroyHandleCircle();
                        transform.parent.GetComponentInChildren<DrawManager>().isEditing = true;

                        circle = Instantiate(circlePrefab, hit.collider.transform.position, Quaternion.identity);
                        circle.GetComponent<HandleCircle>().Init(transform.parent.GetComponentInChildren<DrawManager>().girl1, hit.collider.gameObject, transform.parent.GetComponentInChildren<DrawManager>().qf);
                        AddNodeInDof();
                    }
                }
            }
        }

        if (Input.GetMouseButton(2))
        {
            if (transform.parent.GetComponentInChildren<DrawManager>().girl1 != null)
            {
                transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.Rotate(Vector3.up * 100f * Time.deltaTime);
                if (circle)
                    circle.transform.position = hit.collider.gameObject.transform.position;
            }
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
        }
    }

    private IEnumerator RotateLerp(float _goal, float _speed)
    {
        float curr = 0;
        while (_goal > curr)
        {
            transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(Vector3.up * _goal), _speed * Time.time);
            curr = transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation.eulerAngles.y;
            yield return new WaitForEndOfFrame();
        }
    }

    void CameraRotate(string _n)
    {
        if (_n == "shin.L" || _n == "thigh.L")
        {
 //            StartCoroutine(RotateLerp(90, 0.2f));
            //            circle.transform.position = transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.Find("Petra.002/hips/thigh.L/shin.L").gameObject.transform.position;
            //            transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(Vector3.up * 90), 0.2f * Time.time);                                                                                                                                                                                                              //            transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.Euler(Vector3.up * 90);
            transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.Euler(Vector3.up * 90);
            circle.transform.position = transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.Find("Petra.002/hips/thigh.L/shin.L").gameObject.transform.position;
            for (int i = 0; i < 16; i++)
            {
                float angle = i * Mathf.PI * 2f / 16;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * 0.2f, 0, Mathf.Sin(angle) * 0.2f);
                GameObject go = Instantiate(circlePrefab_shoulder, hit.collider.transform.position + newPos, Quaternion.identity);
                go.transform.parent = circle.transform;
            }
        }
        else if (_n == "shin.R" || _n == "thigh.R")
        {
            //            transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(Vector3.up * -90), 0.2f * Time.time);
            transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.Euler(Vector3.up * -90);
            circle.transform.position = transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.Find("Petra.002/hips/thigh.R/shin.R").gameObject.transform.position;
            for (int i = 0; i < 16; i++)
            {
                float angle = i * Mathf.PI * 2f / 16;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * 0.2f, 0, Mathf.Sin(angle) * 0.2f);
                GameObject go = Instantiate(circlePrefab_shoulder, hit.collider.transform.position + newPos, Quaternion.identity);
                go.transform.parent = circle.transform;
            }
        }
        else if (_n == "upper_arm.L")
        {
            for (int i = 0; i < 16; i++)
            {
                float angle = i * Mathf.PI * 2f / 16;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * 0.2f, 0, Mathf.Sin(angle) * 0.2f);
                GameObject go = Instantiate(circlePrefab_shoulder, hit.collider.transform.position + newPos, Quaternion.identity);
                go.transform.parent = circle.transform;
            }
            for (int i = 0; i < 16; i++)
            {
                float angle = i * Mathf.PI * 2f / 16;
                Vector3 newPos = new Vector3(0, Mathf.Cos(angle) * 0.2f, Mathf.Sin(angle) * 0.2f);
                GameObject go = Instantiate(circlePrefab_shoulder, hit.collider.transform.position + newPos, Quaternion.identity);
                go.transform.parent = circle.transform;
            }

            //            transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.Euler(Vector3.up * 90);
        }
        else if (_n == "upper_arm.R")
        {
            for (int i = 0; i < 16; i++)
            {
                float angle = i * Mathf.PI * 2f / 16;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * 0.2f, 0, Mathf.Sin(angle) * 0.2f);
                GameObject go = Instantiate(circlePrefab_shoulder, hit.collider.transform.position + newPos, Quaternion.identity);
                go.transform.parent = circle.transform;
            }
            for (int i = 0; i < 16; i++)
            {
                float angle = i * Mathf.PI * 2f / 16;
                Vector3 newPos = new Vector3(0, Mathf.Cos(angle) * 0.2f, Mathf.Sin(angle) * 0.2f);
                GameObject go = Instantiate(circlePrefab_shoulder, hit.collider.transform.position + newPos, Quaternion.identity);
                go.transform.parent = circle.transform;
            }

            //            transform.parent.GetComponentInChildren<DrawManager>().girl1.transform.rotation = Quaternion.Euler(Vector3.up * -90);
        }
    }

    public void DestroyHandleCircle()
    {
        transform.parent.GetComponentInChildren<DrawManager>().isEditing = false;

        if (circle != null)
        {
            Destroy(circle.gameObject);
        }
    }

    public int FindPreviousNode(int _dof)
    {
        int i = 0;
        while (i < MainParameters.Instance.joints.nodes[_dof].T.Length && transform.parent.GetComponentInChildren<DrawManager>().frameN*0.02 > MainParameters.Instance.joints.nodes[_dof].T[i])
            i++;
        return i - 1;
    }

    private void ModifyNode(int _dof)
    {
        int node = FindPreviousNode(_dof);
        MainParameters.Instance.joints.nodes[_dof].Q[node] = (float)circle.GetComponent<HandleCircle>().dof[_dof];

        transform.parent.GetComponentInChildren<DrawManager>().isEditing = false;
        transform.parent.GetComponentInChildren<DrawManager>().frameN = (int)(MainParameters.Instance.joints.nodes[_dof].T[node] / 0.02f);
        transform.parent.GetComponentInChildren<DrawManager>().PlayOneFrame();

        transform.parent.GetComponentInChildren<GameManager>().InterpolationDDL();
        transform.parent.GetComponentInChildren<GameManager>().DisplayDDL(_dof, true);
        transform.parent.GetComponentInChildren<DrawManager>().isEditing = true;
    }

    private void AddNodeInDof()
    {
        if (circle.GetComponent<HandleCircle>().target.name == "shin.L" || circle.GetComponent<HandleCircle>().target.name == "shin.R")
        {
            int node = AddNode(1);
            circle.GetComponent<HandleCircle>().node = node;
//                           ModifyNode(1);
            //                MainParameters.Instance.joints.nodes[1].Q[AddNode(1)] = (float)circle.GetComponent<HandleCircle>().dof[1];
            //                print(MainParameters.Instance.joints.nodes[1].Q[AddNode(1)]);
            //                print((-(float)circle.GetComponent<HandleCircle>().dof[1]* Mathf.Rad2Deg + 180)* Mathf.PI / 180);
            //                transform.parent.GetComponentInChildren<GameManager>().InterpolationDDL();
            //                transform.parent.GetComponentInChildren<GameManager>().DisplayDDL(1, true);
        }
        else if (circle.GetComponent<HandleCircle>().target.name == "thigh.L" || circle.GetComponent<HandleCircle>().target.name == "thigh.R")
        {
            int node = AddNode(0);
            circle.GetComponent<HandleCircle>().node = node;
//            ModifyNode(0);
            //                MainParameters.Instance.joints.nodes[0].Q[AddNode(0)] = (float)circle.GetComponent<HandleCircle>().dof[0];
            //                transform.parent.GetComponentInChildren<GameManager>().InterpolationDDL();
            //                transform.parent.GetComponentInChildren<GameManager>().DisplayDDL(0, true);
        }
        else if (circle.GetComponent<HandleCircle>().target.name == "upper_arm.L")
        {
//            circle.GetComponent<HandleCircle>().node = AddNode(2);
            //                mouseDistance.x = (float)dof[3] * 30f;
            //                mouseDistance.y = (float)dof[2] * 30f;
            //            transform.parent.GetComponentInChildren<GameManager>().InterpolationDDL();
            //            transform.parent.GetComponentInChildren<GameManager>().DisplayDDL(2, true);
        }
        else if (circle.GetComponent<HandleCircle>().target.name == "upper_arm.R")
        {
//            circle.GetComponent<HandleCircle>().node = AddNode(4);
            //                mouseDistance.x = (float)dof[5] * 30f;
            //                mouseDistance.y = (float)dof[4] * 30f;
            //            transform.parent.GetComponentInChildren<GameManager>().InterpolationDDL();
            //            transform.parent.GetComponentInChildren<GameManager>().DisplayDDL(4, true);
        }
    }

    public int AddNode(int _dof)
    {
        transform.parent.GetComponentInChildren<GameManager>().InterpolationDDL();
        transform.parent.GetComponentInChildren<GameManager>().DisplayDDL(_dof, true);

        int node = FindPreviousNode(_dof);

        if (previousFrameN == transform.parent.GetComponentInChildren<DrawManager>().frameN)
        {
            return node;
        }
        previousFrameN = transform.parent.GetComponentInChildren<DrawManager>().frameN;

        float[] T = new float[MainParameters.Instance.joints.nodes[_dof].T.Length + 1];
        float[] Q = new float[MainParameters.Instance.joints.nodes[_dof].Q.Length + 1];
        for (int i = 0; i <= node; i++)
        {
            T[i] = MainParameters.Instance.joints.nodes[_dof].T[i];
            Q[i] = MainParameters.Instance.joints.nodes[_dof].Q[i];
        }

        T[node + 1] = transform.parent.GetComponentInChildren<DrawManager>().frameN * 0.02f;
        Q[node + 1] = (float)circle.GetComponent<HandleCircle>().dof[_dof];
        for (int i = node + 1; i < MainParameters.Instance.joints.nodes[_dof].T.Length; i++)
        {
            T[i + 1] = MainParameters.Instance.joints.nodes[_dof].T[i];
            Q[i + 1] = MainParameters.Instance.joints.nodes[_dof].Q[i];
        }
        MainParameters.Instance.joints.nodes[_dof].T = MathFunc.MatrixCopy(T);
        MainParameters.Instance.joints.nodes[_dof].Q = MathFunc.MatrixCopy(Q);

        transform.parent.GetComponentInChildren<GameManager>().InterpolationDDL();
        transform.parent.GetComponentInChildren<GameManager>().DisplayDDL(_dof, true);

        return node + 1;
    }

    public int AddNode2(int _dof)
    {
        transform.parent.GetComponentInChildren<GameManager>().InterpolationDDL();
        transform.parent.GetComponentInChildren<GameManager>().DisplayDDL(_dof, true);

        int node = FindPreviousNode(_dof);

        if (previousFrameN2 == transform.parent.GetComponentInChildren<DrawManager>().frameN)
        {
            return node;
        }
        previousFrameN2 = transform.parent.GetComponentInChildren<DrawManager>().frameN;

        float[] T = new float[MainParameters.Instance.joints.nodes[_dof].T.Length + 1];
        float[] Q = new float[MainParameters.Instance.joints.nodes[_dof].Q.Length + 1];
        for (int i = 0; i <= node; i++)
        {
            T[i] = MainParameters.Instance.joints.nodes[_dof].T[i];
            Q[i] = MainParameters.Instance.joints.nodes[_dof].Q[i];
        }

        T[node + 1] = transform.parent.GetComponentInChildren<DrawManager>().frameN * 0.02f;
        Q[node + 1] = (float)circle.GetComponent<HandleCircle>().dof[_dof];
        for (int i = node + 1; i < MainParameters.Instance.joints.nodes[_dof].T.Length; i++)
        {
            T[i + 1] = MainParameters.Instance.joints.nodes[_dof].T[i];
            Q[i + 1] = MainParameters.Instance.joints.nodes[_dof].Q[i];
        }
        MainParameters.Instance.joints.nodes[_dof].T = MathFunc.MatrixCopy(T);
        MainParameters.Instance.joints.nodes[_dof].Q = MathFunc.MatrixCopy(Q);

        transform.parent.GetComponentInChildren<GameManager>().InterpolationDDL();
        transform.parent.GetComponentInChildren<GameManager>().DisplayDDL(_dof, true);

        return node + 1;
    }
}
