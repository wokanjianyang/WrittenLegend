using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TextMeshPro2Text
{
    [MenuItem("Assets/创建Texts",false,0)]
    static void CreateTextFromTMP()
    {
        CreateTextsFromTMP();
    }

    [MenuItem("GameObject/创建Texts",false,1)]
    static void CreateTextsFromTMP()
    {
        var coms = Selection.activeGameObject.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var com in coms)
        {
            CopyPropertiesFromTMP(com.gameObject);
        }
    }

    static void CopyPropertiesFromTMP(GameObject tran)
    {
        var com = tran.GetComponent<TextMeshProUGUI>();
        if (com)
        {
            var go = new GameObject(tran.name.Replace("(TMP)",""));
            go.SetActive(tran.activeSelf);
            go.AddComponent<RectTransform>();
            go.transform.SetParent(tran.transform.parent);
            go.transform.SetSiblingIndex(tran.transform.GetSiblingIndex());
            go.GetComponent<RectTransform>().anchorMax = tran.GetComponent<RectTransform>().anchorMax;
            go.GetComponent<RectTransform>().anchorMin = tran.GetComponent<RectTransform>().anchorMin;
            go.GetComponent<RectTransform>().anchoredPosition = tran.GetComponent<RectTransform>().anchoredPosition;
            go.GetComponent<RectTransform>().pivot = tran.GetComponent<RectTransform>().pivot;
            go.GetComponent<RectTransform>().rotation = tran.GetComponent<RectTransform>().rotation;
            go.GetComponent<RectTransform>().localScale = tran.GetComponent<RectTransform>().localScale;
            go.GetComponent<RectTransform>().sizeDelta = tran.GetComponent<RectTransform>().sizeDelta;
            var text = go.AddComponent<Text>();
            text.text = com.text;
            text.fontSize = (int)com.fontSize;
            text.color = com.color;
            text.alignment = TextAnchor.MiddleCenter;
            GameObject.DestroyImmediate(tran);
        }
    }
}
