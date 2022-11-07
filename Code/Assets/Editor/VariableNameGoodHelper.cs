using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class VariableNameGoodHelper : EditorWindow
{
    public class VariableName
    {
        public string Name_CN { get; set; }
        public string Name_EN { get; set; }
    }
    
    private static VariableNameGoodHelper win;
    private List<VariableName> VariableNameCache;
    private List<VariableName> filterList;
    private Vector2 pos;

    private string filter = "";
    private string oldfilter = "";

    [MenuItem("Tools/变量名好帮手")]
    public static void Showthis()
    {
        if (win == null)
        {
            win = new VariableNameGoodHelper();
            var vars = File.ReadLines("Assets/Resources/config.txt");
            if (vars != null)
            {
                win.VariableNameCache = new List<VariableName>();
                foreach (var @var in vars)
                {
                    var names = @var.Split(Convert.ToChar(": "));
                    if(names!=null&&names.Length==2)
                    win.VariableNameCache.Add(new VariableName()
                    {
                        Name_CN = names[0],
                        Name_EN = names[1]
                    });
                }

                win.filterList = win.VariableNameCache;
            }
        }
        win.Show();
    }

    private void OnGUI()
    {
        filter = EditorGUILayout.TextField("变量名", filter);
        if (filter != oldfilter)
        {
            oldfilter = filter;
            filterList = VariableNameCache.Where(n => (n.Name_CN + n.Name_EN).Contains(filter)).ToList();
            if (filterList == null || filterList.Count() == 0)
            {
                filterList = VariableNameCache;
            }
        }
        pos = EditorGUILayout.BeginScrollView(pos);
        {
            foreach (var variableName in filterList)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label(variableName.Name_CN);
                    GUILayout.Label(variableName.Name_EN);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
    }
}
