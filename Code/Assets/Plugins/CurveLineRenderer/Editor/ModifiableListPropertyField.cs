using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ModifiableListPropertyField
{
    private static GUILayoutOption miniButtonWidth  = GUILayout.Width(20f);
    private static GUILayoutOption miniButtonHeight = GUILayout.Height(16f);

    public delegate void OnAddAt(int index);

    public delegate void OnRemoveAt(int index);

    public static void Draw(CurveLineRenderer linerender, SerializedProperty list, List<Vector3> meshnormalList, OnAddAt onAddAtCallback,
                            OnRemoveAt         onRemoveAtCallback)
    {
        if (!list.isArray)
            return;

        EditorGUILayout.PropertyField(list);
        EditorGUI.indentLevel += 1;

        if (list.isExpanded)
        {
            EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(16);

            if (GUILayout.Button(new GUIContent("+")))
            {
                if (onAddAtCallback != null)
                    onAddAtCallback(list.arraySize - 1);

                meshnormalList.Add(Vector3.zero);
            }

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(16);
                GUILayout.Label(string.Format("Vertex {0}", i));

                if (linerender.lineType== CurveLineRenderer.LineDir.Wall && i != list.arraySize - 1)
                {
                    if (GUILayout.Button("N", GUILayout.Width(20)))
                    {
                        //翻转法线
                        var normal =  linerender.CalcNormal(i);
                        if (normal == meshnormalList[i])
                        {
                            normal *= -1f;
                        }
                        meshnormalList[i] = normal;
                    }

                }


                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
                
                if (GUILayout.Button(new GUIContent("+"), miniButtonWidth, miniButtonHeight))
                {
                    if (onAddAtCallback != null)
                        onAddAtCallback(i);
                 
                    //插入法线
                    {
                        meshnormalList.Insert(i,Vector3.zero);
                        
                        var normal =  linerender.CalcNormal(i);
                        linerender.meshNormal[i] = normal;
                    }
                   
                }

                if (GUILayout.Button(new GUIContent("-"), miniButtonWidth, miniButtonHeight))
                {
                    if (onRemoveAtCallback != null)
                        onRemoveAtCallback(i);
                    meshnormalList.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUI.indentLevel -= 1;
    }


}