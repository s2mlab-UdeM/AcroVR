using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;
#if UNITY_EDITOR 
using UnityEditor;
#endif
//using System.Drawing;
//using System.Drawing.Imaging;

public class AniGraphManager : MonoBehaviour
{
    GameObject resultPrefab;
    GameObject resultCanvas;
    public GraphChart graph;
    GraphChart resultGraph;
    GameObject takeoffPrefab;
    public GameObject takeoffCanvas;
    public bool bDraw = false;

    string[] dataCategories;
    string[] nodesCategories;
    string nodesTemp1Category;
    string nodesTemp2Category;
    string dataTempCategory;
    string[] dataCurvesCategories;

    private Material data1GraphLine;
    private Material nodes1GraphPoint;

    public bool mouseTracking = false;

    public int ddlUsed = 0;
    int nodeUsed = 0;
    int numNodes = 0;
    public float axisXmin = 0;
    public float axisXmax = 0;
    public float axisYmin = 0;
    public float axisYmax = 0;
    public float axisXmaxDefault = 0;
    public float axisYminDefault = 0;
    public float axisYmaxDefault = 0;

    float factorGraphRatioX = 0;
    float factorGraphRatioY = 0;
    float q0MinCurve0;
    float q0MaxCurve0;

    double mousePosX;
    double mousePosY;
    public float mousePosSaveX;
    public float mousePosSaveY;
    bool mouseLeftButtonON = false;

    private DopeSheetEditor m_DopeSheetEditor;
    private CurveEditor m_CurveEditor1;
    private CurveEditor m_CurveEditor2;
    private CurveEditor m_CurveEditor3;
    private CurveEditor m_CurveEditor4;
    public Rect windowRect1 = new Rect(0, 0, 270, 150);
//    public Rect windowRect1 = new Rect(0, 0, 550, 330);
    public Rect windowRect2 = new Rect(270, 0, 270, 150);
    public Rect windowRect3 = new Rect(0, 150, 270, 150);
    public Rect windowRect4 = new Rect(270, 150, 270, 150);

    public class GuiListItem
    {
        public bool Selected;
        public string Name;
        public GuiListItem(bool mSelected, string mName)
        {
            Selected = mSelected;
            Name = mName;
        }
        public GuiListItem(string mName)
        {
            Selected = false;
            Name = mName;
        }
        public void enable()
        {
            Selected = true;
        }
        public void disable()
        {
            Selected = false;
        }
    }
    private Vector2 ListScrollPos;
    private List<GuiListItem> MyListOfStuff;
    private int SelectedListItem;
    private bool DropdownVisible;

    public float speed = 0.3f;
    Dictionary<int, List<Texture2D>> gifFiles = new Dictionary<int, List<Texture2D>>();

    void Start()
    {
        /*        GameObject graphCanvas = GameObject.Find("TakeOffGraph");
                graph = graphCanvas.GetComponent<GraphChart>();

                graphCanvas = GameObject.Find("ResultGraph");
                resultGraph = graphCanvas.GetComponent<GraphChart>();*/

        resultPrefab = (GameObject)Resources.Load("ResultPrefab", typeof(GameObject));
        data1GraphLine = (Material)Resources.Load("Data1GraphLine", typeof(Material));
        nodes1GraphPoint = (Material)Resources.Load("Nodes1GraphPoint", typeof(Material));
        takeoffPrefab = (GameObject)Resources.Load("TakeOffParamPrefab", typeof(GameObject));

        dataCategories = new string[2] { "Data1", "Data2" };
        //        dataCategories = new string[2] { "Player1", "Player2" };
        nodesCategories = new string[2] { "Nodes1", "Nodes2" };
        nodesTemp1Category = "NodesTemp1";
        nodesTemp2Category = "NodesTemp2";
        dataTempCategory = "DataTemp";
        dataCurvesCategories = new string[3] { "Data1", "Data2", "Data3" };

        //        CreateLineMaterial();

        m_DopeSheetEditor = new DopeSheetEditor();
        m_CurveEditor1 = new CurveEditor();
        m_CurveEditor2 = new CurveEditor();
        m_CurveEditor3 = new CurveEditor();
        m_CurveEditor4 = new CurveEditor();

        //        curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        //        curve.preWrapMode = WrapMode.PingPong;
        //        curve.postWrapMode = WrapMode.PingPong;

        //        resultCanvas = Instantiate(resultPrefab);
        //        resultCanvas.SetActive(false);

        //        takeoffCanvas = Instantiate(takeoffPrefab);
        //        takeoffCanvas.SetActive(false);
        //        graph = takeoffCanvas.GetComponentInChildren<GraphChart>();

        /*        MyListOfStuff = new List<GuiListItem>(); //Initialize our list of stuff

                MyListOfStuff.Add(new GuiListItem("Trampo_TripleAvantDemi_girl"));
                MyListOfStuff.Add(new GuiListItem("Tumbling_two_girls"));
                MyListOfStuff.Add(new GuiListItem("Trampo_TripleAvantDemi_xbot"));

                LoadGif(0, "Assets/Art/1.gif");
                LoadGif(1, "Assets/Art/2.gif");
                LoadGif(2, "Assets/Art/3.gif");*/
    }


    /*    void LoadGif(int num, string fileName)
        {
            List<Texture2D> gifFrames = new List<Texture2D>();
            var gifImage = Image.FromFile(fileName);
            var dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
            int frameCount = gifImage.GetFrameCount(dimension);
            for (int i = 0; i < frameCount; i++)
            {
                gifImage.SelectActiveFrame(dimension, i);
                var frame = new Bitmap(gifImage.Width, gifImage.Height);
                System.Drawing.Graphics.FromImage(frame).DrawImage(gifImage, Point.Empty);
                var frameTexture = new Texture2D(frame.Width, frame.Height);
                for (int x = 0; x < frame.Width; x++)
                    for (int y = 0; y < frame.Height; y++)
                    {
                        System.Drawing.Color sourceColor = frame.GetPixel(x, y);
                        //                    frameTexture.SetPixel(frame.Width - 1 - x, y, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A)); // for some reason, x is flipped
                        frameTexture.SetPixel(x, -y, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A)); // for some reason, x is flipped
                    }
                frameTexture.Apply();
                gifFrames.Add(frameTexture);
            }
            gifFiles.Add(num, gifFrames);
        }*/

    void Update()
    {
        if (MainParameters.Instance.joints.nodes == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            /*            graph.DataSource.StartBatch();
                                    graph.DataSource.ClearCategory("Player1");
                                    for (int i = 0; i < 30; i++)
                                    {
                                        graph.DataSource.AddPointToCategory("Player1", Random.value * 10f, Random.value * 10f);
                                    }
                                    graph.DataSource.EndBatch();*/
        }

//        if (graph && takeoffCanvas.activeSelf)
        if (graph && transform.parent.GetComponentInChildren<UIManager>().GetCurrentTab() == 2)
        {
                graph.MouseToClient(out mousePosX, out mousePosY);
            if (mousePosX < graph.DataSource.HorizontalViewOrigin || mousePosX > graph.DataSource.HorizontalViewOrigin + graph.DataSource.HorizontalViewSize ||
                mousePosY < graph.DataSource.VerticalViewOrigin || mousePosY > graph.DataSource.VerticalViewOrigin + graph.DataSource.VerticalViewSize)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                DisplayNodesTemp2();
                mouseLeftButtonON = true;
            }

            if (mouseLeftButtonON)
            {
                graph.DataSource.StartBatch();
                graph.DataSource.ClearCategory(nodesTemp1Category);
                if (MainParameters.Instance.joints.nodes[ddlUsed].interpolation.type == MainParameters.InterpolationType.Quintic)
                    graph.DataSource.AddPointToCategory(nodesTemp1Category, mousePosX, mousePosY);
                else
                    graph.DataSource.AddPointToCategory(nodesTemp1Category, MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed], mousePosY);
                graph.DataSource.EndBatch();
            }

            if (Input.GetMouseButtonUp(0) && mouseLeftButtonON)
            {
                RemoveNodesTemp();

                if (MainParameters.Instance.joints.nodes[ddlUsed].interpolation.type == MainParameters.InterpolationType.Quintic)
                {
                    if ((nodeUsed <= 0 && mousePosX >= MainParameters.Instance.joints.nodes[ddlUsed].T[1]) || (nodeUsed >= numNodes - 1 && mousePosX <= MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed - 1]) ||
                    (nodeUsed > 0 && nodeUsed < numNodes - 1 && (mousePosX <= MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed - 1] || mousePosX >= MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed + 1])))
                    {
                        //                    Main.Instance.EnableDisableControls(false, true);
                        //                    GraphManager.Instance.mouseTracking = false;
                        return;
                    }

                    MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed] = (float)mousePosX;
                }

                MainParameters.Instance.joints.nodes[ddlUsed].Q[nodeUsed] = (float)mousePosY / Mathf.Rad2Deg;
                transform.parent.GetComponentInChildren<GameManager>().InterpolationDDL();
                transform.parent.GetComponentInChildren<GameManager>().DisplayDDL(ddlUsed, true);
                //                transform.parent.GetComponentInChildren<GameManager>().DisplayDDL(0, true);
                //            MovementF.Instance.InterpolationAndDisplayDDL(ddlUsed, ddlUsed, (int)Mathf.Round(MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed] / MainParameters.Instance.joints.lagrangianModel.dt), false);

                int temp = transform.parent.GetComponentInChildren<DrawManager>().frameN;
                transform.parent.GetComponentInChildren<DrawManager>().ShowAvatar();
                transform.parent.GetComponentInChildren<DrawManager>().frameN = temp;
            }
        }
    }

    void RemoveNodesTemp()
    {
        graph.DataSource.StartBatch();
        graph.DataSource.ClearCategory(nodesTemp1Category);
        graph.DataSource.ClearCategory(nodesTemp2Category);
        graph.DataSource.ClearCategory(nodesCategories[0]);
        for (int i = 0; i < MainParameters.Instance.joints.nodes[ddlUsed].T.Length; i++)
        {
            float value = MainParameters.Instance.joints.nodes[ddlUsed].Q[i] * Mathf.Rad2Deg;
            if (MainParameters.Instance.joints.nodes[ddlUsed].T[i] <= axisXmax && value >= axisYmin && value <= axisYmax)
                graph.DataSource.AddPointToCategory(nodesCategories[0], MainParameters.Instance.joints.nodes[ddlUsed].T[i], value);
        }
        graph.DataSource.EndBatch();
        mouseLeftButtonON = false;
    }

    public void DisplayCurveAndNodes(int curve, int ddl, bool axisRange)
    {
        if (graph == null) return;

        graph.DataSource.StartBatch();

        graph.DataSource.ClearCategory(dataCategories[curve]);
        graph.DataSource.ClearCategory(nodesCategories[curve]);
        if (curve <= 0)
        {
            ddlUsed = ddl;
            numNodes = MainParameters.Instance.joints.nodes[ddl].T.Length;
            graph.DataSource.ClearCategory(dataCategories[1]);
            graph.DataSource.ClearCategory(nodesCategories[1]);
        }

        float q0Min = 360;
        float q0Max = -360;
        float value;
        int t0Length = MainParameters.Instance.joints.t0.Length;
        for (int i = 0; i < t0Length; i++)
        {
            value = MainParameters.Instance.joints.q0[ddl, i] * Mathf.Rad2Deg;
            if (!axisRange && value < axisYmin)
                value = axisYmin;
            if (!axisRange && value > axisYmax)
                value = axisYmax;
            if (axisRange || MainParameters.Instance.joints.t0[i] <= axisXmax)
                graph.DataSource.AddPointToCategory(dataCategories[curve], MainParameters.Instance.joints.t0[i], value);
            //            graph.DataSource.AddPointToCategory("Player1", MainParameters.Instance.joints.t0[i], value);
            if (value < q0Min) q0Min = value;
            if (value > q0Max) q0Max = value;
        }

        for (int i = 0; i < MainParameters.Instance.joints.nodes[ddl].T.Length; i++)
        {
            value = MainParameters.Instance.joints.nodes[ddl].Q[i] * Mathf.Rad2Deg;
            if (axisRange || (MainParameters.Instance.joints.nodes[ddl].T[i] <= axisXmax && value >= axisYmin && value <= axisYmax))
                graph.DataSource.AddPointToCategory(nodesCategories[curve], MainParameters.Instance.joints.nodes[ddl].T[i], value);
            //                graph.DataSource.AddPointToCategory("Player2", MainParameters.Instance.joints.nodes[ddl].T[i], value);
        }

        MaterialTiling tiling = new MaterialTiling(false, 45.5f);

        graph.DataSource.SetCategoryLine(dataCategories[curve], data1GraphLine, 2.58f, tiling);
        graph.DataSource.SetCategoryPoint(nodesCategories[curve], nodes1GraphPoint, 8);

        if (axisRange)
        {
            axisXmin = Mathf.Round(MainParameters.Instance.joints.t0[0] - 0.5f);
            axisXmax = Mathf.Round(MainParameters.Instance.joints.t0[t0Length - 1] + 0.5f);
            axisXmaxDefault = axisXmax;
            graph.DataSource.HorizontalViewOrigin = axisXmin;
            graph.DataSource.HorizontalViewSize = axisXmax - axisXmin;
            factorGraphRatioX = graph.WidthRatio / (float)graph.DataSource.HorizontalViewSize;
            if (curve <= 0)
            {
                q0MinCurve0 = q0Min;
                q0MaxCurve0 = q0Max;
            }
            else
            {
                q0Min = Mathf.Min(q0Min, q0MinCurve0);
                q0Max = Mathf.Max(q0Max, q0MaxCurve0);
            }
            axisYmin = Mathf.Round((q0Min - 30) / 10) * 10;
            axisYmax = Mathf.Round((q0Max + 30) / 10) * 10;
            axisYminDefault = axisYmin;
            axisYmaxDefault = axisYmax;
            graph.DataSource.VerticalViewOrigin = axisYmin;
            graph.DataSource.VerticalViewSize = axisYmax - axisYmin;
            factorGraphRatioY = graph.HeightRatio / (float)graph.DataSource.VerticalViewSize;
        }
        else
        {
            graph.DataSource.HorizontalViewOrigin = axisXmin;
            graph.DataSource.HorizontalViewSize = axisXmax - axisXmin;
            factorGraphRatioX = graph.WidthRatio / (float)graph.DataSource.HorizontalViewSize;
            graph.DataSource.VerticalViewOrigin = axisYmin;
            graph.DataSource.VerticalViewSize = axisYmax - axisYmin;
            factorGraphRatioY = graph.HeightRatio / (float)graph.DataSource.VerticalViewSize;
        }
        graph.DataSource.EndBatch();

        //        bDraw = true;
    }

    /*    private Material lineMaterial;

        private void CreateLineMaterial()
        {
            if (!lineMaterial)
            {
                var shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
            }
        }*/

    public void DisplayNodesTemp2()
    {
        graph.DataSource.StartBatch();

        // Effacer les noeuds précédents
        graph.DataSource.ClearCategory(nodesCategories[0]);

        // Trouver le noeud le plus près de la position de la souris (en tenant compte du ratio X vs Y du graphique), ça sera ce noeud qui sera modifié

        mousePosSaveX = (float)mousePosX;
        mousePosSaveY = (float)mousePosY;
        nodeUsed = FindNearestNode();

        // Ajouter les noeuds dans la nouvelle courbe Nodes et NodesTemp2

        for (int i = 0; i < numNodes; i++)
        {
            float value = MainParameters.Instance.joints.nodes[ddlUsed].Q[i] * Mathf.Rad2Deg;
            if (MainParameters.Instance.joints.nodes[ddlUsed].T[i] <= axisXmax && value >= axisYmin && value <= axisYmax)
            {
                if (i != nodeUsed)
                    graph.DataSource.AddPointToCategory(nodesCategories[0], MainParameters.Instance.joints.nodes[ddlUsed].T[i], value);
                else
                    graph.DataSource.AddPointToCategory(nodesTemp2Category, MainParameters.Instance.joints.nodes[ddlUsed].T[i], value);
            }
        }

        graph.DataSource.EndBatch();
    }

    public int FindNearestNode()
    {
        int node = 0;
        float minDistanceToNode = 99999;
        for (int i = 0; i < MainParameters.Instance.joints.nodes[ddlUsed].T.Length; i++)
        {
            float distanceToNode = Mathf.Pow((mousePosSaveX - MainParameters.Instance.joints.nodes[ddlUsed].T[i]) * factorGraphRatioX, 2) +
                Mathf.Pow((mousePosSaveY - MainParameters.Instance.joints.nodes[ddlUsed].Q[i] * Mathf.Rad2Deg) * factorGraphRatioY, 2);
            if (distanceToNode < minDistanceToNode)
            {
                node = i;
                minDistanceToNode = distanceToNode;
            }
        }
        return node;
    }

    public void DisplayCurves(float[] t, float[] data)
    {
        float[,] data1 = new float[data.GetUpperBound(0) + 1, 1];
        for (int i = 0; i <= data.GetUpperBound(0); i++)
            data1[i, 0] = data[i];
        DisplayCurves(t, data1);
    }

    public void DisplayCurves(float[] t, float[,] data)
    {
        if (resultGraph == null) return;
        resultGraph.DataSource.StartBatch();

        // Effacer les courbes précédentes

        for (int i = 0; i < dataCurvesCategories.Length; i++)
            resultGraph.DataSource.ClearCategory(dataCurvesCategories[i]);

        // Ajouter toutes les données dans la ou les nouvelles courbes Data
        // Calculer les valeurs minimum et maximum

        float tMin = 999999;
        float tMax = -999999;
        float dataMin = 999999;
        float dataMax = -999999;
        for (int i = 0; i <= data.GetUpperBound(1); i++)
        {
            for (int j = 0; j < t.Length; j++)
            {
                resultGraph.DataSource.AddPointToCategory(dataCurvesCategories[i], t[j], data[j, i]);
                if (i <= 0 && t[j] < tMin) tMin = t[j];
                if (i <= 0 && t[j] > tMax) tMax = t[j];
                if (data[j, i] < dataMin) dataMin = data[j, i];
                if (data[j, i] > dataMax) dataMax = data[j, i];
            }
        }

        // Définir les échelles des temps et des données

        resultGraph.DataSource.HorizontalViewOrigin = tMin;
        resultGraph.DataSource.HorizontalViewSize = tMax - tMin;
        if (tMax - tMin <= 2)
            resultGraph.GetComponent<HorizontalAxis>().MainDivisions.FractionDigits = 2;
        else
            resultGraph.GetComponent<HorizontalAxis>().MainDivisions.FractionDigits = 1;
        resultGraph.DataSource.VerticalViewOrigin = dataMin;
        resultGraph.DataSource.VerticalViewSize = dataMax - dataMin;
        if (dataMax - dataMin <= 0.2)
            resultGraph.GetComponent<VerticalAxis>().MainDivisions.FractionDigits = 3;
        else if (dataMax - dataMin <= 2)
            resultGraph.GetComponent<VerticalAxis>().MainDivisions.FractionDigits = 2;
        else
            resultGraph.GetComponent<VerticalAxis>().MainDivisions.FractionDigits = 1;

        resultGraph.DataSource.EndBatch();
    }

    public void DisplayCurves(GraphChart graphCurves, float[] t, float[] data)
    {
        float[,] data1 = new float[data.GetUpperBound(0) + 1, 1];
        for (int i = 0; i <= data.GetUpperBound(0); i++)
            data1[i, 0] = data[i];
        DisplayCurves(graphCurves, t, data1);
    }

    public void DisplayCurves(GraphChart graphCurves, float[] t, float[,] data)
    {
        if (data == null) return;

        if (graphCurves == null) return;
        graphCurves.DataSource.StartBatch();

        for (int i = 0; i < dataCurvesCategories.Length; i++)
            graphCurves.DataSource.ClearCategory(dataCurvesCategories[i]);

        float tMin = 999999;
        float tMax = -999999;
        float dataMin = 999999;
        float dataMax = -999999;
        for (int i = 0; i <= data.GetUpperBound(1); i++)
        {
            for (int j = 0; j < t.Length; j++)
            {
                graphCurves.DataSource.AddPointToCategory(dataCurvesCategories[i], t[j], data[j, i]);
                if (i <= 0 && t[j] < tMin) tMin = t[j];
                if (i <= 0 && t[j] > tMax) tMax = t[j];
                if (data[j, i] < dataMin) dataMin = data[j, i];
                if (data[j, i] > dataMax) dataMax = data[j, i];
            }
        }

        graphCurves.DataSource.HorizontalViewOrigin = tMin;
        graphCurves.DataSource.HorizontalViewSize = tMax - tMin;
        if (tMax - tMin <= 2)
            graphCurves.GetComponent<HorizontalAxis>().MainDivisions.FractionDigits = 2;
        else
            graphCurves.GetComponent<HorizontalAxis>().MainDivisions.FractionDigits = 1;
        graphCurves.DataSource.VerticalViewOrigin = dataMin;
        graphCurves.DataSource.VerticalViewSize = dataMax - dataMin;
        if (dataMax - dataMin <= 0.2)
            graphCurves.GetComponent<VerticalAxis>().MainDivisions.FractionDigits = 3;
        else if (dataMax - dataMin <= 2)
            graphCurves.GetComponent<VerticalAxis>().MainDivisions.FractionDigits = 2;
        else
            graphCurves.GetComponent<VerticalAxis>().MainDivisions.FractionDigits = 1;

        graphCurves.DataSource.EndBatch();

        bDraw = true;
    }

    float x = 50, y = 50;
    public void OnGUI()
    {
//        if (Event.current.type != EventType.Repaint)
//            return;

        //        windowRect1 = GUI.Window(0, windowRect1, Graph1, "Preview");

        //        DrawingLine.DrawLine(new Vector2(8, 100), new Vector2(12, 100), UnityEngine.Color.green, 4, false);
        //        DrawingLine.DrawLine(new Vector2(10, 100), new Vector2(200, 20), UnityEngine.Color.green, 2, false);
        //        DrawingLine.DrawLine(new Vector2(200, 20), new Vector2(300, 100), UnityEngine.Color.green, 3, false);
        //                DrawingLine.BezierLine(new Vector2(8, 100), new Vector2(8, 100), new Vector2(12, 100), new Vector2(12, 100), UnityEngine.Color.green, 4, false, 50);
        //                DrawingLine.BezierLine(new Vector2(10, 100), new Vector2(10, 100), new Vector2(200, 20), new Vector2(200, 20), UnityEngine.Color.green, 2, false, 50);
        //                DrawingLine.BezierLine(new Vector2(200, 20), new Vector2(205, 20), new Vector2(300, 100), new Vector2(295, 100), UnityEngine.Color.green, 5, false, 50);

        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.MouseDown:
                x = evt.mousePosition.x;
                y = evt.mousePosition.y;
                break;
            case EventType.MouseDrag:
                x = evt.mousePosition.x;
                y = evt.mousePosition.y;
                break;
        }

        if (bDraw)
        {
            //            DrawLines();
            /*                        int t0Length = MainParameters.Instance.joints.t0.Length;
                                    for (int i = 0; i < t0Length; i++)
                                    {
                                        if (i == t0Length - 1) break;
                                        else
                                            DrawNodeCurve(new Vector3(MainParameters.Instance.joints.t0[i], MainParameters.Instance.joints.q0[0, i] * Mathf.Rad2Deg/50, 0)/3, new Vector3(MainParameters.Instance.joints.t0[i+1], MainParameters.Instance.joints.q0[0, i+1] * Mathf.Rad2Deg/50, 0)/3);
                                    }*/

//                        windowRect1 = GUI.Window(0, windowRect1, Graph1, "Graph1");
//                        windowRect2 = GUI.Window(1, windowRect2, Graph2, "Graph2");
            //            windowRect3 = GUI.Window(2, windowRect3, Graph3, "Graph3");
            //            windowRect4 = GUI.Window(3, windowRect4, Graph4, "Graph4");

        }

        /*        if(takeoffCanvas.activeSelf)
                {
                    float value = MainParameters.Instance.joints.q0[0, 10] * Mathf.Rad2Deg;
                    //            graph.DataSource.AddPointToCategory(nodesCategories[0], MainParameters.Instance.joints.t0[10], value, 5);
                    DoubleVector3 point = graph.DataSource.GetPoint(nodesCategories[0], 10);
                   DrawingLine.DrawLine(point.ToVector2(), point.ToVector2() + new Vector2(0,10f), UnityEngine.Color.green, 4, false);
                }*/
    }

    public void TaskOffGraphOn()
    {
        takeoffCanvas = Instantiate(takeoffPrefab);
//        takeoffCanvas.SetActive(false);
        graph = takeoffCanvas.GetComponentInChildren<GraphChart>();

        transform.parent.GetComponentInChildren<DrawManager>().PlayEnd();
//        takeoffCanvas.SetActive(true);
    }

    public void TaskOffGraphOff()
    {
        Destroy(takeoffCanvas);
//        takeoffCanvas.SetActive(false);
    }

    public void GraphOn()
    {
        graph = GameObject.Find("TrainingMenu").transform.Find("Canvas/TabPanel/TabContainer/TabTwo/Content2/PanelGraph/GraphMultiple/").gameObject.GetComponent<GraphChart>();
    }

    public void ResultGraphOn()
    {
        resultCanvas = Instantiate(resultPrefab);
//        resultCanvas.SetActive(false);

        //        bDraw = true;
//        resultCanvas.SetActive(true);
    }

    public void ResultGraphOff()
    {
        Destroy(resultCanvas);
        //        resultCanvas.SetActive(false);
    }

    void Graph1(int windowID)
    {
        //        m_DopeSheetEditor.OnGUI(windowRect);
                m_CurveEditor1.OnGUI(1);
//        MakeDropDownMenu();
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    void Graph2(int windowID)
    {
        //        m_DopeSheetEditor.OnGUI(windowRect);
        m_CurveEditor2.OnGUI(2);
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    void Graph3(int windowID)
    {
        //        m_DopeSheetEditor.OnGUI(windowRect);
        m_CurveEditor3.OnGUI(3);
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    void Graph4(int windowID)
    {
        //        m_DopeSheetEditor.OnGUI(windowRect);
        m_CurveEditor4.OnGUI(4);
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    /*    private void DrawLines()
        {
            lineMaterial.SetPass(0);
            GL.LoadOrtho();
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            GL.Color(Color.green);

            float value;
            int t0Length = MainParameters.Instance.joints.t0.Length;
            for (int i = 0; i < t0Length; i++)
            {
    //            if(i == t0Length-1) DrawLine(new Vector2(MainParameters.Instance.joints.t0[i], MainParameters.Instance.joints.q0[0, i] * Mathf.Rad2Deg), new Vector2(MainParameters.Instance.joints.t0[i], MainParameters.Instance.joints.q0[0, i] * Mathf.Rad2Deg));
    //            else
    //                DrawLine(new Vector2(MainParameters.Instance.joints.t0[i], MainParameters.Instance.joints.q0[0, i] * Mathf.Rad2Deg/10), new Vector2(MainParameters.Instance.joints.t0[i+1],MainParameters.Instance.joints.q0[0, i+1] * Mathf.Rad2Deg/10));
                DrawLine(new Vector2(MainParameters.Instance.joints.t0[i], MainParameters.Instance.joints.q0[0, i] * Mathf.Rad2Deg / 50)/3, new Vector2(MainParameters.Instance.joints.t0[i]+0.01f, MainParameters.Instance.joints.q0[0, i] * Mathf.Rad2Deg / 50)/3);
            }


            //        DrawLine(new Vector2(100.0f, 100.0f), new Vector2(200.0f, 100.0f));
            //        DrawLine(new Vector2(200.0f, 100.0f), new Vector2(200.0f, 200.0f));
            //        DrawLine(new Vector2(200.0f, 200.0f), new Vector2(100.0f, 200.0f));
            //        DrawLine(new Vector2(100.0f, 200.0f), new Vector2(100.0f, 100.0f));


            GL.End();
            GL.PopMatrix();
        }

        private void DrawLine(Vector2 p1, Vector2 p2)
        {
            GL.Vertex(p1);
            GL.Vertex(p2);
        }*/

    void DrawNodeCurve(Vector3 start, Vector3 end)
    {
        //        Vector3 startTan = start + Vector3.right;
        //        Vector3 endTan = end + Vector3.left;
        Vector3 startTan = start;
        Vector3 endTan = end;

        //        Color shadowCol = new Color(0, 100, 0, 0.06f);
        //        for (int i = 0; i < 3; i++)
        //            Handles.DrawBezier(start, end, startTan, endTan, shadowCol, null, (i + 1) * 5);
        //        Handles.DrawBezier(start, end, startTan, endTan, Color.black, null, 1);
        GL.LoadOrtho();
#if UNITY_EDITOR 
        Handles.color = UnityEngine.Color.green;
        Handles.DrawLine(start, end);
#endif

        //        Handles.DrawBezier(start, end, startTan, endTan, Color.black, null, 1);
    }

    void MakeDropDownMenu()
    {
        if (DropdownVisible)
        {
            GUILayout.BeginArea(new Rect(10, 50, 220, 150));
            ListScrollPos = GUILayout.BeginScrollView(ListScrollPos, false, true);
            GUILayout.BeginVertical(GUILayout.Width(200));
            for (int i = 0; i < MyListOfStuff.Count; i++)
            {
                if (GUILayout.Button(MyListOfStuff[i].Name))
                {
                    if (SelectedListItem != -1) MyListOfStuff[SelectedListItem].disable();
                    SelectedListItem = i;
                    MyListOfStuff[SelectedListItem].enable();
                    DropdownVisible = false;
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        GUILayout.BeginArea(new Rect(10, 20, 220, 150));
        GUILayout.BeginHorizontal();
        string SelectedItemCaption = (SelectedListItem == -1) ? "Select an item..." : MyListOfStuff[SelectedListItem].Name;
        string ButtonText = (DropdownVisible) ? "<<" : ">>";
        GUILayout.TextField(SelectedItemCaption);
        DropdownVisible = GUILayout.Toggle(DropdownVisible, ButtonText, "button", GUILayout.Width(32), GUILayout.Height(20));
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        for (int i = 0; i < MyListOfStuff.Count; i++)
        {
            if (MyListOfStuff[i].Selected)
            {
                GUI.DrawTexture(new Rect(240, 50, gifFiles[i][0].width, gifFiles[i][0].height), gifFiles[i][(int)(Time.frameCount * speed) % gifFiles[i].Count]);
            }
        }
    }
}
