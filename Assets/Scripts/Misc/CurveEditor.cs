using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public interface CurveRenderer
{
    void DrawCurve(float minTime, float maxTime, Color color, Matrix4x4 transform, Color wrapColor);
    AnimationCurve GetCurve();
    float RangeStart();
    float RangeEnd();
    void SetWrap(WrapMode wrap);
    void SetWrap(WrapMode preWrap, WrapMode postWrap);
    void SetCustomRange(float start, float end);
    float EvaluateCurveSlow(float time);
    float EvaluateCurveDeltaSlow(float time);
    Bounds GetBounds();
    Bounds GetBounds(float minTime, float maxTime);
    void FlushCache();
}

public class CurveWrapper
{
    public delegate Vector2 GetAxisScalarsCallback();
    public delegate void SetAxisScalarsCallback(Vector2 newAxisScalars);

    public CurveWrapper()
    {
        id = 0;
        groupId = -1;
        regionId = -1;
        hidden = false;
        readOnly = false;
        listIndex = -1;
        getAxisUiScalarsCallback = null;
        setAxisUiScalarsCallback = null;
    }

    internal enum SelectionMode
    {
        None = 0,
        Selected = 1,
        SemiSelected = 2
    }

    private CurveRenderer m_Renderer;
    public CurveRenderer renderer { get { return m_Renderer; } set { m_Renderer = value; } }
    public AnimationCurve curve { get { return renderer.GetCurve(); } }

    // Input - should not be changed by curve editor
    public int id;
    public int groupId;
    public int regionId;                                    // Regions are defined by two curves added after each other with the same regionId.
    public Color color;
    public Color wrapColorMultiplier = Color.white;
    public bool readOnly;
    public bool hidden;
    public GetAxisScalarsCallback getAxisUiScalarsCallback; // Delegate used to fetch values that are multiplied on x and y axis ui values
    public SetAxisScalarsCallback setAxisUiScalarsCallback; // Delegate used to set values back that has been changed by this curve editor

    public int listIndex;                                       // Index into m_AnimationCurves list

    private bool m_Changed;
    public bool changed
    {
        get
        {
            return m_Changed;
        }

        set
        {
            m_Changed = value;
        }
    }

    public float vRangeMin = -Mathf.Infinity;
    public float vRangeMax = Mathf.Infinity;
}

public class CurveEditor
{
    ControlPointRenderer m_PointRenderer;
    CurveWrapper[] m_AnimationCurves;
    private List<int> m_DrawOrder = new List<int>(); // contains curveIds (last element topmost)

    AnimationCurve tCurve;
    List<float> tList = new List<float>();
    Keyframe key = new Keyframe();

    public CurveEditor()
    {
        float[] modulos = new float[] {
                0.0000001f, 0.0000005f, 0.000001f, 0.000005f, 0.00001f, 0.00005f, 0.0001f, 0.0005f,
                0.001f, 0.005f, 0.01f, 0.05f, 0.1f, 0.5f, 1, 5, 10, 50, 100, 500,
                1000, 5000, 10000, 50000, 100000, 500000, 1000000, 5000000, 10000000
            };

        m_DrawOrder.Add(1);
        m_DrawOrder.Add(2);

        m_PointRenderer = new ControlPointRenderer(null);
    }

    Dictionary<int, int> m_CurveIDToIndexMap;
    private Dictionary<int, int> curveIDToIndexMap
    {
        get
        {
            if (m_CurveIDToIndexMap == null)
                m_CurveIDToIndexMap = new Dictionary<int, int>();

            return m_CurveIDToIndexMap;
        }
    }

    public CurveWrapper GetCurveWrapperFromID(int curveID)
    {
        int index;
        if (curveIDToIndexMap.TryGetValue(curveID, out index))
            return m_AnimationCurves[index];

        return null;
    }

    public void OnGUI(int id)
    {
        GridGUI();
        CurveGUI(id);
    }

    public void CurveGUI(int id)
    {
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.KeyDown:
                Debug.Log("KeyDown");
                break;
            case EventType.ContextClick:
                Debug.Log("ContextClick");
                break;
            case EventType.MouseDown:
                key.time = evt.mousePosition.x/10;
                key.value = evt.mousePosition.y/10;
                tCurve.MoveKey(1, key);

                tList.Clear();
                for (int i = 0; i < t0Length; i++)
                {
                    tList.Add(tCurve.Evaluate(i));
                }
                DrawGraphCurve();
                break;
            case EventType.MouseDrag:
                key.time = evt.mousePosition.x / 10;
                key.value = evt.mousePosition.y / 10;
                tCurve.MoveKey(1, key);

                tList.Clear();
                for (int i = 0; i < t0Length; i++)
                {
                    tList.Add(tCurve.Evaluate(i));
                }
                DrawGraphCurve();
                break;
        }
        if (evt.type == EventType.Repaint)
        {
            DrawCurves(id);
        }
    }

    int t0Length = 0;

    void DrawCurves(int id)
    {
        if (t0Length == 0)
        {
            t0Length = MainParameters.Instance.joints.t0.Length;
            tCurve = AnimationCurve.Linear(0.0f, 0.0f, t0Length, 0.0f);

            for (int j = 0; j < MainParameters.Instance.joints.nodes[0].T.Length; j++)
            {
                float x = MainParameters.Instance.joints.nodes[0].T[j];
                float y = MainParameters.Instance.joints.nodes[0].Q[j];

                tCurve.AddKey(x*8f, y*8f);
                //                Handles.color = Color.cyan;
                //                Handles.DrawSolidDisc(new Vector3(x, y, 0)*8f, Vector3.forward, 2f);
            }

            for (int i = 0; i < t0Length; i++)
            {
                if (i == t0Length - 1) break;
                else
                {
                    Rect test = new Rect(MainParameters.Instance.joints.t0[i], MainParameters.Instance.joints.q0[0, i] * Mathf.Rad2Deg / 50, MainParameters.Instance.joints.t0[i + 1], MainParameters.Instance.joints.q0[0, i + 1] * Mathf.Rad2Deg / 50);
                    m_PointRenderer.AddPoint(test, Color.yellow);

                    tList.Add(tCurve.Evaluate(i));
                }
            }
        }

        /*        for (int i = 0; i < m_DrawOrder.Count; ++i)
                {
                    CurveWrapper cw = GetCurveWrapperFromID(m_DrawOrder[i]);
                    DrawPointsOnCurve(cw);
                }*/

//        m_PointRenderer.Render();

        if(t0Length!=0)
        {
//            DrawGraphCurve();

            if(id == 1)
                DisplayCurves(MainParameters.Instance.joints.t, MainParameters.Instance.joints.rot);
            else if(id ==2)
                DisplayCurves(MainParameters.Instance.joints.t, MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 1));
            else if(id == 3)
                DisplayCurves(MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 0), MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 1));
            else if(id == 4)
                DisplayCurves(MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 2), MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 1));
            //            DisplayCurves(MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 0), MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 1));
        }
    }

    void DrawGraphCurve()
    {
        for (int i = 0; i < tList.Count - 1; i++)
        {
            //                Handles.DrawSolidDisc(new Vector3(i, tList[i], 0)*4f, Vector3.forward, 1f);
//            Handles.color = Color.green;
//            Handles.DrawLine(new Vector3(i, 15 - tList[i], 0) * 8f, new Vector3(i + 1, 15 - tList[i + 1], 0) * 8f);
        }

//        Handles.color = Color.blue;
        for (int j = 0; j < tCurve.length; j++)
        {
//            Handles.DrawSolidDisc(new Vector3(tCurve.keys[j].time * 8f, 120 - tCurve.keys[j].value * 8f, 0), Vector3.forward, 3f);
        }
    }

    private void DisplayCurves(float[] t, float[,] data)
    {
//        for (int i = 0; i <= data.GetUpperBound(1); i++)
//        {
            for (int j = 0; j < t.Length-1; j++)
            {
//                Handles.color = Color.yellow;
//                Handles.DrawLine(new Vector3(t[j]*6f, 0.5f- data[j,0], 0)*50f, new Vector3(t[j+1]*6f, 0.5f-data[j+1,0], 0)*50f);
//                Handles.color = Color.green;
//                Handles.DrawLine(new Vector3(t[j] * 6f, 0.5f - data[j, 1], 0) * 50f, new Vector3(t[j + 1] * 6f, 0.5f - data[j + 1, 1], 0) * 50f);
//                Handles.color = Color.blue;
//                Handles.DrawLine(new Vector3(t[j] * 6f, 0.5f - data[j, 2], 0) * 50f, new Vector3(t[j + 1] * 6f, 0.5f - data[j + 1, 2], 0) * 50f);
            }
        //        }
    }

    private void DisplayCurves(float[] t, float[] data)
    {
        for (int j = 0; j < t.Length - 1; j++)
        {
//            Handles.color = Color.green;
//            Handles.DrawLine(new Vector3(t[j] * 6f, 0.5f - data[j], 0) * 50f, new Vector3(t[j + 1] * 6f, 0.5f - data[j + 1], 0) * 50f);
        }
    }

    void DrawPointsOnCurve(CurveWrapper cw)
    {
        Keyframe[] keys = cw.curve.keys;
        for (int i = 0; i < keys.Length; ++i)
        {
            Keyframe k = keys[i];
//            Handles.DrawSolidDisc(new Vector3(10,10,0), Vector3.forward, 2f);
        }
    }
    
    public void GridGUI()
    {
    }

    void DrawLine(Vector2 lhs, Vector2 rhs)
    {
//        Handles.DrawLine(new Vector3(lhs.x, lhs.y, 0), new Vector3(rhs.x, rhs.y, 0));
    }

    void DisplayCurve(AnimationCurve curve)
    {

    }
}