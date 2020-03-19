using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ControlPointRenderer
{
    private class RenderChunk
    {
        public Mesh mesh;

        public List<Vector3> vertices;
        public List<Color32> colors;
        public List<Vector2> uvs;
        public List<int> indices;

        public Vector3 start;
        public Vector3 end;

        public bool isDirty = true;
    }

    private List<RenderChunk> m_RenderChunks = new List<RenderChunk>();

    private Texture2D m_Icon;

    private Rect pos;

    //  Can hold a maximum of 16250 control points.
    const int kMaxVertices = 65000;
    const string kControlPointRendererMeshName = "ControlPointRendererMesh";

    private static Material s_Material;
    public static Material material
    {
        get
        {
            if (!s_Material)
            {
                //                Shader shader = (Shader)EditorGUIUtility.LoadRequired("Editors/AnimationWindow/ControlPoint.shader");
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                s_Material = new Material(shader);
            }

            return s_Material;
        }
    }

    public ControlPointRenderer(Texture2D icon)
    {
        m_Icon = icon;
    }

    public void FlushCache()
    {
        for (int i = 0; i < m_RenderChunks.Count; ++i)
        {
            Object.DestroyImmediate(m_RenderChunks[i].mesh);
        }

        m_RenderChunks.Clear();
    }

    public void Clear()
    {
        for (int i = 0; i < m_RenderChunks.Count; ++i)
        {
            RenderChunk renderChunk = m_RenderChunks[i];

            renderChunk.mesh.Clear();

            renderChunk.vertices.Clear();
            renderChunk.colors.Clear();
            renderChunk.uvs.Clear();

            renderChunk.indices.Clear();

            renderChunk.isDirty = true;
        }
    }

    public void Render()
    {
        Material mat = material;
        mat.SetTexture("_MainTex", m_Icon);
        mat.SetPass(0);

//        for (int i = 0; i < m_RenderChunks.Count/3; ++i)
            for (int i = 0; i < m_RenderChunks.Count; ++i)
            {
                RenderChunk renderChunk = m_RenderChunks[i];

            if (renderChunk.isDirty)
            {
                renderChunk.mesh.vertices = renderChunk.vertices.ToArray();
                renderChunk.mesh.colors32 = renderChunk.colors.ToArray();
                renderChunk.mesh.uv = renderChunk.uvs.ToArray();

                renderChunk.mesh.SetIndices(renderChunk.indices.ToArray(), MeshTopology.Triangles, 0);

                renderChunk.isDirty = false;
            }

//            Graphics.DrawMeshNow(renderChunk.mesh, Handles.matrix);

            //          GL.LoadOrtho();
            //            Graphics.DrawMeshNow(renderChunk.mesh, new Vector3(i/3,0.3f,-5f), Quaternion.identity);

//            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//            cube.GetComponent<Transform>().position = renderChunk.start;
//            cube.GetComponent<Transform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);



//            GL.LoadOrtho();

//            Handles.color = Color.red;

//            Handles.DrawLine(renderChunk.start/3, renderChunk.end/3);
//            Handles.DrawSolidDisc(renderChunk.start / 3, Vector3.forward, 0.01f);

//            Handles.DrawLine(renderChunk.start*50, renderChunk.end*50);


            for (int j = 0; j < MainParameters.Instance.joints.nodes[0].T.Length; j++)
            {
                float x = MainParameters.Instance.joints.nodes[0].T[j];
                float y = MainParameters.Instance.joints.nodes[0].Q[j];
//                Handles.DrawSolidDisc(new Vector3(x+0.2f,3-y-0.3f,0)*50f, Vector3.forward, 2f);
            }
        }
    }

    public void AddPoint(Rect rect, Color color)
    {
        RenderChunk renderChunk = GetRenderChunk();

        int baseIndex = renderChunk.vertices.Count;

        renderChunk.start = new Vector3(rect.x+0.2f, 3-rect.y-0.2f, 0);
        renderChunk.end = new Vector3(rect.width+0.2f, 3-rect.height-0.2f, 0);

        renderChunk.vertices.Add(new Vector3(rect.xMin, rect.yMin, 0.0f));
        renderChunk.vertices.Add(new Vector3(rect.xMax, rect.yMin, 0.0f));
        renderChunk.vertices.Add(new Vector3(rect.xMax, rect.yMax, 0.0f));
        renderChunk.vertices.Add(new Vector3(rect.xMin, rect.yMax, 0.0f));

        renderChunk.colors.Add(color);
        renderChunk.colors.Add(color);
        renderChunk.colors.Add(color);
        renderChunk.colors.Add(color);

        renderChunk.uvs.Add(new Vector2(0.0f, 0.0f));
        renderChunk.uvs.Add(new Vector2(1.0f, 0.0f));
        renderChunk.uvs.Add(new Vector2(1.0f, 1.0f));
        renderChunk.uvs.Add(new Vector2(0.0f, 1.0f));

        renderChunk.indices.Add(baseIndex);
        renderChunk.indices.Add(baseIndex + 1);
        renderChunk.indices.Add(baseIndex + 2);

        renderChunk.indices.Add(baseIndex);
        renderChunk.indices.Add(baseIndex + 2);
        renderChunk.indices.Add(baseIndex + 3);

        renderChunk.isDirty = true;
    }

    private RenderChunk GetRenderChunk()
    {
        RenderChunk renderChunk;
        if (m_RenderChunks.Count > 0)
        {
//            renderChunk = m_RenderChunks.Last();
            // Dynamically create new render chunks when needed.
//            if ((renderChunk.vertices.Count + 4) > kMaxVertices)
//            {
                renderChunk = CreateRenderChunk();
//            }
        }
        else
        {
            renderChunk = CreateRenderChunk();
        }

        return renderChunk;
    }

    private RenderChunk CreateRenderChunk()
    {
        RenderChunk renderChunk = new RenderChunk();

        renderChunk.mesh = new Mesh();
        renderChunk.mesh.name = kControlPointRendererMeshName;
        renderChunk.mesh.hideFlags |= HideFlags.DontSave;

        renderChunk.vertices = new List<Vector3>();
        renderChunk.colors = new List<Color32>();
        renderChunk.uvs = new List<Vector2>();
        renderChunk.indices = new List<int>();

        m_RenderChunks.Add(renderChunk);

        return renderChunk;
    }
}
