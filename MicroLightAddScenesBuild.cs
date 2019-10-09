
#if  UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class MicroLightAddScenesBuild : ScriptableObject
{
    static void GetFilesRecursion(List<string> FilesList, string folder, string pattern)
    {

        DirectoryInfo root = new DirectoryInfo(folder);
        foreach (FileInfo f in root.GetFiles())
        {
            if (f.FullName.Contains(pattern) && !f.FullName.Contains(".meta"))
                FilesList.Add(f.FullName);
        }

        if (FilesList.Count > 0)
        {
            //  Debug.Log("Find TREE!");
        }
        foreach (DirectoryInfo f in root.GetDirectories())
        {
            GetFilesRecursion(FilesList, f.FullName, pattern);
        }
    }


    [MenuItem("MicroLight/ClearBuildList")]
    static void ClearBuildList()
    {
        EditorBuildSettings.scenes = null;

    }

    [MenuItem("MicroLight/AddScenestoBuildAndShow")]
    static void AddScenestoBuildAndShow()
    {
        string folder = EditorUtility.OpenFolderPanel("Select the folder containing the tree", "Assets/", "");
        if (folder != "")
        {
            if (folder.IndexOf(Application.dataPath) == -1)
            {
                Debug.LogWarning("The folder must be in this project anywhere inside the Assets folder!");
                return;
            }

            List<string> FilesList = new List<string>();
            GetFilesRecursion(FilesList, folder, ".unity");

            string[] files = FilesList.ToArray();

            if (files.Length > 0)
            {
                var original = EditorBuildSettings.scenes;
                var newSettings = new EditorBuildSettingsScene[original.Length + files.Length];
                System.Array.Copy(original, newSettings, original.Length);

                for (int i = 0; i < files.Length; i++)
                {
                    int index = files[i].IndexOf("Assets\\");
                    string relativePath = files[i].Substring(index);
                    var sceneToAdd = new EditorBuildSettingsScene(relativePath, true);
                    newSettings[original.Length + i] = sceneToAdd;
                }
                EditorBuildSettings.scenes = newSettings;
            }
        }



    }

    [MenuItem("MicroLight/AddScenestoBuildAndHide")]
    static void AddScenestoBuildAndHide()
    {
        string folder = EditorUtility.OpenFolderPanel("Select the folder containing the tree", "Assets/", "");
        if (folder != "")
        {
            if (folder.IndexOf(Application.dataPath) == -1)
            {
                Debug.LogWarning("The folder must be in this project anywhere inside the Assets folder!");
                return;
            }

            List<string> FilesList = new List<string>();
            GetFilesRecursion(FilesList, folder, ".unity");

            string[] files = FilesList.ToArray();

            if (files.Length > 0)
            {
                var original = EditorBuildSettings.scenes;
                var newSettings = new EditorBuildSettingsScene[original.Length + files.Length];
                System.Array.Copy(original, newSettings, original.Length);

                for (int i = 0; i < files.Length; i++)
                {
                    int index = files[i].IndexOf("Assets\\");
                    string relativePath = files[i].Substring(index);
                    var sceneToAdd = new EditorBuildSettingsScene(relativePath, false);
                    newSettings[original.Length + i] = sceneToAdd;
                }
                EditorBuildSettings.scenes = newSettings;
            }
        }



    }

    [MenuItem("MicroLight/Combine Scenes")]
    static void Combine()
    {
        Object[] objects = Selection.objects;

        EditorApplication.SaveCurrentSceneIfUserWantsTo();
        EditorApplication.NewScene();

        foreach (Object item in objects)
        {
            EditorApplication.OpenSceneAdditive(AssetDatabase.GetAssetPath(item));
        }
    }

    [MenuItem("MicroLight/Combine Scenes", true)]
    static bool CanCombine()
    {
        if (Selection.objects.Length < 2)
        {
            return false;
        }

        foreach (Object item in Selection.objects)
        {
            if (!Path.GetExtension(AssetDatabase.GetAssetPath(item)).ToLower().Equals(".unity"))
            {
                return false;
            }
        }

        return true;
    }

    //[MenuItem("MicroLight/CombineLightMaps")]
    //static void CombineLightMaps()
    //{
    //    var original = EditorBuildSettings.scenes;
    //    if (original.Length > 0)
    //    {
    //        List<LightmapData> LightdataList = new List<LightmapData>();
    //        for (int i = 0; i < original.Length; i++)
    //        {
    //            int index = original[i].path.IndexOf("Assets\\");
    //            string relativePath = original[i].path.Substring(index);
    //            relativePath= relativePath.Replace(".unity", "\\LightingData.asset");
    //            //AssetDatabase.LoadAssetAtPath();
    //               LightmapData lightmapData =new LightmapData();
    //            LightdataList.Add(lightmapData);

    //        }
    //        LightmapSettings.lightmaps = LightdataList.ToArray();
    //    }
    //}
    //[MenuItem("MicroLight/CombineLightMaps", true)]
    //static bool CanCombineLightMaps()
    //{
    //    var original = EditorBuildSettings.scenes;
    //    if (original.Length > 0)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

}
#endif