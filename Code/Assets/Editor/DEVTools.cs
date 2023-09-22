using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class DEVTools
{

    [MenuItem("开发工具/删除存档")]
    public static void DeleteArchive()
    {
        //Game.UserData.Delete();
        Debug.Log("删除成功");
    }
}
