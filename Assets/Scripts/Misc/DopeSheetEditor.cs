using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class DopeSheetEditor
{
    struct DrawElement
    {
        public Rect position;
        public Color color;
        public Texture2D texture;

        public DrawElement(Rect position, Color color, Texture2D texture)
        {
            this.position = position;
            this.color = color;
            this.texture = texture;
        }
    }

    class DopeSheetControlPointRenderer
    {
        private List<DrawElement> m_UnselectedKeysDrawBuffer = new List<DrawElement>();
        private List<DrawElement> m_SelectedKeysDrawBuffer = new List<DrawElement>();
        private List<DrawElement> m_DragDropKeysDrawBuffer = new List<DrawElement>();

        private ControlPointRenderer m_UnselectedKeysRenderer;
        private ControlPointRenderer m_SelectedKeysRenderer;
        private ControlPointRenderer m_DragDropKeysRenderer;

        private Texture2D m_DefaultDopeKeyIcon;

        public void FlushCache()
        {
            m_UnselectedKeysRenderer.FlushCache();
            m_SelectedKeysRenderer.FlushCache();
            m_DragDropKeysRenderer.FlushCache();
        }

        private void DrawElements(List<DrawElement> elements)
        {
            if (elements.Count == 0)
                return;

            Color oldColor = GUI.color;

            Color color = Color.white;
            GUI.color = color;
            Texture icon = m_DefaultDopeKeyIcon;

            for (int i = 0; i < elements.Count; ++i)
            {
                DrawElement element = elements[i];

                // Change color
                if (element.color != color)
                {
                    color = GUI.enabled ? element.color : element.color * 0.8f;
                    GUI.color = color;
                }

                // Element with specific texture (sprite).
                if (element.texture != null)
                {
                    GUI.Label(element.position, element.texture, GUIStyle.none);
                }
                // Ordinary control point.
                else
                {
                    Rect rect = new Rect((element.position.center.x - icon.width / 2),
                            (element.position.center.y - icon.height / 2),
                            icon.width,
                            icon.height);
                    GUI.Label(rect, icon, GUIStyle.none);
                }
            }

            GUI.color = oldColor;
        }

        public DopeSheetControlPointRenderer()
        {
//            m_DefaultDopeKeyIcon = EditorGUIUtility.FindTexture("GameManager Icon");

            m_UnselectedKeysRenderer = new ControlPointRenderer(m_DefaultDopeKeyIcon);
            m_SelectedKeysRenderer = new ControlPointRenderer(m_DefaultDopeKeyIcon);
            m_DragDropKeysRenderer = new ControlPointRenderer(m_DefaultDopeKeyIcon);
        }

        public void Clear()
        {
            m_UnselectedKeysDrawBuffer.Clear();
            m_SelectedKeysDrawBuffer.Clear();
            m_DragDropKeysDrawBuffer.Clear();

            m_UnselectedKeysRenderer.Clear();
            m_SelectedKeysRenderer.Clear();
            m_DragDropKeysRenderer.Clear();
        }

        public void Render()
        {
            DrawElements(m_UnselectedKeysDrawBuffer);
            m_UnselectedKeysRenderer.Render();

            DrawElements(m_SelectedKeysDrawBuffer);
            m_SelectedKeysRenderer.Render();

            DrawElements(m_DragDropKeysDrawBuffer);
            m_DragDropKeysRenderer.Render();
        }

        public void AddUnselectedKey(DrawElement element)
        {
            if (element.texture != null)
            {
                m_UnselectedKeysDrawBuffer.Add(element);
            }
            else
            {
                Rect rect = element.position;
                //                rect.size = new Vector2(m_DefaultDopeKeyIcon.width, m_DefaultDopeKeyIcon.height);
                //                m_UnselectedKeysRenderer.AddPoint(rect, element.color);
                m_UnselectedKeysRenderer.AddPoint(rect, element.color);
            }
        }

        public void AddSelectedKey(DrawElement element)
        {
            // Control point has a specific texture (sprite image).
            // This will not be batched rendered and must be handled separately.
            if (element.texture != null)
            {
                m_SelectedKeysDrawBuffer.Add(element);
            }
            else
            {
                Rect rect = element.position;
                rect.size = new Vector2(m_DefaultDopeKeyIcon.width, m_DefaultDopeKeyIcon.height);
                m_SelectedKeysRenderer.AddPoint(rect, element.color);
            }
        }

        public void AddDragDropKey(DrawElement element)
        {
            // Control point has a specific texture (sprite image).
            // This will not be batched rendered and must be handled separately.
            if (element.texture != null)
            {
                m_DragDropKeysDrawBuffer.Add(element);
            }
            else
            {
                Rect rect = element.position;
                rect.size = new Vector2(m_DefaultDopeKeyIcon.width, m_DefaultDopeKeyIcon.height);
                m_DragDropKeysRenderer.AddPoint(rect, element.color);
            }
        }
    }

    private Rect FromToRect(Vector2 start, Vector2 end)
    {
        Rect r = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);
        if (r.width < 0)
        {
            r.x += r.width;
            r.width = -r.width;
        }
        if (r.height < 0)
        {
            r.y += r.height;
            r.height = -r.height;
        }
        return r;
    }

    DopeSheetControlPointRenderer m_PointRenderer;

    public void OnDisable()
    {
        if (m_PointRenderer != null)
            m_PointRenderer.FlushCache();
    }

    public void Init()
    {
        if (m_PointRenderer == null)
            m_PointRenderer = new DopeSheetControlPointRenderer();
    }

    public void OnGUI(Rect position)
    {
        Init();

        Rect localRect = new Rect(position.x, position.y, position.width, position.height);

        DopelinesGUI(localRect);


        //        Event evt = Event.current;
        //        Vector2 mousePos = evt.mousePosition;


        int id = GUIUtility.GetControlID(897560, FocusType.Passive);
        Event evt = Event.current;
        Vector2 s_StartMouseDragPosition = evt.mousePosition;

        switch (evt.GetTypeForControl(id))
        {
            case EventType.MouseDown:
                Vector2 mousePos = evt.mousePosition;
                break;
            case EventType.MouseDrag:
                Rect r = FromToRect(s_StartMouseDragPosition, evt.mousePosition);
                break;
        }


            /*        int id = s_RectSelectionID;
                    switch (evt.GetTypeForControl(id))
                    {
                        case EventType.mouseDown:
                            if (evt.button == 0 && position.Contains(mousePos))
                            {
                                GUIUtility.hotControl = id;
                                m_SelectStartPoint = mousePos;
                                m_ValidRect = false;
                                evt.Use();
                            }
                            break;

                    }*/
        }

     int t0Length = 0;

    private void DopelinesGUI(Rect position)
    {
        Rect linePosition = position;

        m_PointRenderer.Clear();

        if (t0Length == 0)
        {
            t0Length = MainParameters.Instance.joints.t0.Length;
            for (int i = 0; i < t0Length; i++)
            {
                if (i == t0Length - 1) break;
                else
                {
                    DopeLine dopeLine = new DopeLine(new Rect(MainParameters.Instance.joints.t0[i], MainParameters.Instance.joints.q0[0, i] * Mathf.Rad2Deg / 50, MainParameters.Instance.joints.t0[i + 1], MainParameters.Instance.joints.q0[0, i + 1] * Mathf.Rad2Deg / 50));

                    DopeLineRepaint(dopeLine);
                }
                //                DrawNodeCurve(new Vector3(MainParameters.Instance.joints.t0[i], MainParameters.Instance.joints.q0[0, i] * Mathf.Rad2Deg / 50, 0) / 3, new Vector3(MainParameters.Instance.joints.t0[i + 1], MainParameters.Instance.joints.q0[0, i + 1] * Mathf.Rad2Deg / 50, 0) / 3);
            }

/*            for (int i = 0; i < MainParameters.Instance.joints.nodes[ddl].T.Length; i++)
            {
                value = MainParameters.Instance.joints.nodes[ddl].Q[i] * Mathf.Rad2Deg;
                if (axisRange || (MainParameters.Instance.joints.nodes[ddl].T[i] <= axisXmax && value >= axisYmin && value <= axisYmax))
                    graph.DataSource.AddPointToCategory(nodesCategories[curve], MainParameters.Instance.joints.nodes[ddl].T[i], value);
                //                graph.DataSource.AddPointToCategory("Player2", MainParameters.Instance.joints.nodes[ddl].T[i], value);
            }*/

        }

        m_PointRenderer.Render();


        /*        List<DopeLine> dopelines = new List<DopeLine> { new DopeLine(new Rect(0, 0, 10, 10)) };
                for (int i = 0; i < dopelines.Count; ++i)
                {
                    DopeLine dopeLine = dopelines[i];
                    dopeLine.position = linePosition;

                    DopeLineRepaint(dopeLine);
                }*/
    }
    private void DopeLineRepaint(DopeLine dopeline)
    {
        int length = 1;

        for (int i = 0; i < length; i++)
        {
            Color color = Color.green;
            Texture2D texture = null;

            m_PointRenderer.AddUnselectedKey(new DrawElement(dopeline.position, color, texture));
        }
    }
}
