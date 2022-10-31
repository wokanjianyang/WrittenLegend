//#define CURVE_LINE_RENDERER_DEBUG

#undef CURVE_LINE_RENDERER_DEBUG
//#define CURVE_LINE_RENDERER_DEBUG_SHOW_NORMALS
#undef CURVE_LINE_RENDERER_DEBUG_SHOW_NORMALS

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Reflection;

[CustomEditor(typeof(CurveLineRenderer))]
public class CurveLineRendererEditor : Editor
{
    private static Material shareMaterial = null;
    private const float epsilon = 0.0001f;
    private CurveLineRenderer curveLineRenderer;
    private Transform         handleTransform;
    private Quaternion        handleRotation;
    private MeshFilter        handleMeshFilter;
    SerializedProperty        repeatProperty;

    private float vertexButtonSize     = 0.04f;
    private float vertexButtonPickSize = 0.06f;

    private int selectedIndex = -1;

    void OnEnable()
    {
        curveLineRenderer = (CurveLineRenderer) target;

        handleTransform  = curveLineRenderer.transform;
        handleRotation   = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
        handleMeshFilter = curveLineRenderer.GetComponent<MeshFilter>();

        //预生成法线处理
        for (int i = curveLineRenderer.meshNormal.Count; i < curveLineRenderer.vertices.Count; i++)
        {
            var normal = curveLineRenderer.CalcNormal(i);
            curveLineRenderer.meshNormal[i] = normal;
        }

        SceneView.onSceneGUIDelegate = OnSceneDraw;

        if (!shareMaterial)
        {
            shareMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Resource_SVN/Mat/RPGMap/Wall.mat");
        }
        
        RebuildMesh();
    }

    void OnDisable()
    {
        curveLineRenderer            =  null;
        SceneView.onSceneGUIDelegate -= OnSceneDraw;
    }

    
    void OnSceneDraw(SceneView sceneView)
    {
        List<Vector3> vertices = curveLineRenderer.vertices;
        if (vertices.Count == 0)
        {
            return;
        }

        Handles.color = Color.white;

        Vector3 prev = ShowVertex(0);
        for (int i = 1; i < vertices.Count; ++i)
        {
            Vector3 cur = ShowVertex(i);
            Handles.DrawLine(prev, cur);

            prev = cur;
        }

#if (CURVE_LINE_RENDERER_DEBUG_SHOW_NORMALS)
        Mesh mesh = handleMeshFilter.sharedMesh;
        Handles.color = Color.green;
        for (int i = 0; i < mesh.normals.Length; i += 1
            )
        {
            Vector3 startPoint = handleTransform.TransformPoint(mesh.vertices[i]);
            Vector3 normal = mesh.normals[i];
            Handles.DrawLine(startPoint, startPoint + normal);
        }
#endif

        if (GUI.changed)
        {
            if (target != null)
                EditorUtility.SetDirty(target);

            RebuildMesh();
        }

        if (curveLineRenderer.transform.hasChanged)
        {
            curveLineRenderer.isCurveConnetionProcessed = false;
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
            curveLineRenderer.Invalidate();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("meshBuildMode"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lineType"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("meshDirect"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("uvDirect"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("width"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("radius"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("roundedAngle"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maintexSize"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("reverseSideEnabled"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("meshNormal"));


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.SelectableLabel("Next curve", GUILayout.ExpandWidth(false));
        GUILayout.Space(-65);
        curveLineRenderer.nextCurve =
            (CurveLineRenderer) EditorGUILayout.ObjectField(curveLineRenderer.nextCurve, typeof(CurveLineRenderer),
                                                            true, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

#if (CURVE_LINE_RENDERER_DEBUG)
        if (GUILayout.Button("Rebuild"))
            GUI.changed = true;
#endif
        ModifiableListPropertyField.Draw(curveLineRenderer, serializedObject.FindProperty("vertices"),
                                         curveLineRenderer.meshNormal, OnAddVertexAtIndex, OnRemoveVertexAtIndex);
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            curveLineRenderer.isCurveConnetionProcessed = false;
            if (curveLineRenderer.prevCurve)
                curveLineRenderer.prevCurve.isCurveConnetionProcessed = false;
            if (target != null)
                EditorUtility.SetDirty(target);

            RebuildMesh();
        }


        //控制尺寸
        SetNativeSize();
        //设置法线

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("法线方向:");

            if (GUILayout.Button("上"))
            {
                SetNormal(new Vector3(0,1,0));
            }

            if (GUILayout.Button("下"))
            {
                SetNormal(new Vector3(0,-1,0));
            }
            if (GUILayout.Button("左"))
            {
                SetNormal(new Vector3(-1,0,0));
            }

            if (GUILayout.Button("右"))
            {
                SetNormal(new Vector3(1,0,0));
            }
        }
        GUILayout.EndHorizontal();
    }


    /// <summary>
    /// 设置法线方向
    /// </summary>
    /// <param name="v3"></param>
    private void SetNormal(Vector3 v3)
    {
        for (int i = 0; i < curveLineRenderer.meshNormal.Count; i++)
        {
            curveLineRenderer.meshNormal[i] = v3;
        }
    }
    /// <summary>
    /// 重建mesh
    /// </summary>
    void RebuildMesh()
    {
        if (handleMeshFilter == null || curveLineRenderer == null)
            return;

        Mesh mesh = handleMeshFilter.sharedMesh;
        if (mesh == null)
        {
            mesh                        = new Mesh();
            mesh.name                   = "Curve Line Mesh";
            handleMeshFilter.sharedMesh = mesh;
        }
        
        //重置材质球
        var mr = curveLineRenderer.GetComponent<MeshRenderer>();
        if (mr.sharedMaterial != shareMaterial)
        {
            mr.sharedMaterial = shareMaterial;
        }
        

        //curveLineRenderer.Rebuild(mesh);
    }

    /// <summary>
    /// 显示顶点
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    Vector3 ShowVertex(int index)
    {
        Vector3 vertex = handleTransform.TransformPoint(curveLineRenderer.vertices[index]);
        float   size   = HandleUtility.GetHandleSize(vertex);

        if (Handles.Button(vertex, handleRotation, vertexButtonSize * size, vertexButtonPickSize * size,
                           Handles.DotHandleCap))
        {
            selectedIndex = index;
            Repaint();
        }

        Handles.Label(vertex, String.Format("Vertex {0}", index));

        if (selectedIndex != index)
            return vertex;

        EditorGUI.BeginChangeCheck();
        vertex = Handles.DoPositionHandle(vertex, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curveLineRenderer, "Move vertex");
            curveLineRenderer.vertices[index]           = handleTransform.InverseTransformPoint(vertex);
            curveLineRenderer.isCurveConnetionProcessed = false;
        }

        return vertex;
    }

    /**
     * Adding new vertex in line
     */
    void AddVertexAtPosition(int position)
    {
        List<Vector3> vertices = curveLineRenderer.vertices;

        Vector3 point;
        if (position < 0)
        {
            if (vertices.Count > 0)
                point = vertices[0] - CalculateDirection(position);
            else
                point = Vector3.zero;
        }
        else if (position >= vertices.Count - 1)
        {
            point = vertices[vertices.Count - 1] + CalculateDirection(position);
        }
        else
        {
            point = (vertices[position] + vertices[position + 1]) * 0.5f;
        }

        curveLineRenderer.InsertVertex(position + 1, point);
    }

    /**
     * Calculating direction for next curve segment
     */
    Vector3 CalculateDirection(int newVertexIndex)
    {
        List<Vector3> vertices = curveLineRenderer.vertices;

        Vector3 direction;
        if (newVertexIndex < 0)
        {
            if (vertices.Count >= 2)
            {
                Vector3 firstPoint     = vertices[0];
                int     wayVertexIndex = FirstDifferentVertexIndex(1);
                if (wayVertexIndex > 0)
                {
                    direction = vertices[wayVertexIndex] - firstPoint;
                }
                else
                {
                    direction = NormalOrthogonalVector(curveLineRenderer.GetLineDirect()).normalized;
                }
            }
            else
            {
                direction = NormalOrthogonalVector(curveLineRenderer.GetLineDirect()).normalized;
            }
        }
        else if (newVertexIndex >= vertices.Count - 1)
        {
            if (vertices.Count >= 2)
            {
                Vector3 lastPoint      = vertices[vertices.Count - 1];
                int     wayVertexIndex = FirstDifferentVertexIndex(-1);
                if (wayVertexIndex >= 0)
                {
                    Vector3 pointPrevLast = vertices[wayVertexIndex];
                    direction = lastPoint - pointPrevLast;
                }
                else
                {
                    direction = NormalOrthogonalVector(curveLineRenderer.GetLineDirect()).normalized;
                }
            }
            else
            {
                direction = NormalOrthogonalVector(curveLineRenderer.GetLineDirect()).normalized;
            }
        }
        else
        {
            direction = Vector3.zero;
        }

        return direction;
    }

    /**
     * First different vertex
     */
    int FirstDifferentVertexIndex(int direction)
    {
        List<Vector3> vertices = curveLineRenderer.vertices;

        if (direction == 0)
        {
            return -1;
        }

        int     startIndex = direction > 0 ? 1 : vertices.Count - 2;
        int     endIndex   = direction > 0 ? vertices.Count : -1;
        Vector3 point      = direction > 0 ? vertices[0] : vertices[vertices.Count - 1];
        for (int i = startIndex; i != endIndex; i += direction)
        {
            if (Mathf.Abs(point.x - vertices[i].x) > epsilon || Mathf.Abs(point.y - vertices[i].y) > epsilon ||
                Mathf.Abs(point.z - vertices[i].z) > epsilon)
            {
                return i;
            }
        }

        return -1;
    }

    /**
     * Vector orthogonal to normal
     */
    Vector3 NormalOrthogonalVector(Vector3 normal)
    {
        Vector3 orthogonalVector;
        if (Mathf.Abs(normal.x) < epsilon || Mathf.Abs(normal.y) < epsilon || Mathf.Abs(normal.z) < epsilon)
        {
            orthogonalVector = new Vector3(Mathf.Abs(normal.x) < epsilon ? 1 : 0, Mathf.Abs(normal.y) < epsilon ? 1 : 0,
                                           Mathf.Abs(normal.z) < epsilon ? 1 : 0);
        }
        else
        {
            orthogonalVector = new Vector3(0, -normal.z, normal.y);
        }

        return orthogonalVector.normalized;
    }

    void OnAddVertexAtIndex(int index)
    {
        AddVertexAtPosition(index);
    }

    void OnRemoveVertexAtIndex(int index)
    {
        curveLineRenderer.RemoveVertexAt(index);
    }


    private Texture lastTex = null;

    void SetNativeSize()
    {
        var mr = curveLineRenderer.GetComponent<MeshRenderer>();
        if (mr.sharedMaterial && mr.sharedMaterial.mainTexture)
        {
            lastTex = mr.sharedMaterial.mainTexture;
            var tex = mr.sharedMaterial.mainTexture;

            var path = AssetDatabase.GetAssetPath(tex);
            var ti   = TextureImporter.GetAtPath(path);

            //反射获取
            object[] args = new object[2] {0, 0};
            MethodInfo mi =
                typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(ti, args);
            var width  = (int) args[0];
            var height = (int) args[1];

            //设置尺寸
            this.curveLineRenderer.maintexSize = new Vector2(width, height);
        }
    }
}