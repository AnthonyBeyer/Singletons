using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SoSingleton<T> : ScriptableObject where T : ScriptableObject
{

    private static string GetFilePathFromResources(System.Type t)
    {
        return t.ToString();
    }

    private static T _instance;
    public static T Instance
    {
        get{
            if(_instance == null)
            {
                _instance = Resources.Load<T>(GetFilePathFromResources(typeof(T)));
                #if UNITY_EDITOR
                if(_instance == null)
                    _instance = CreateAsset();
                #endif
            }
            return _instance;
        }
    }

    #if UNITY_EDITOR

    public static string assetPathToMainResourcesFolder = "Assets/Resources/";
    public static string extension = ".asset";
    public static string assetPath
    {
        get
        {
            return assetPathToMainResourcesFolder + GetFilePathFromResources(typeof(T)) + extension;
        }
    }

    public static T CreateAsset()
    {
        T asset = ScriptableObject.CreateInstance<T>();

        if (!System.IO.Directory.Exists(assetPathToMainResourcesFolder))
            System.IO.Directory.CreateDirectory(assetPathToMainResourcesFolder);
        
        AssetDatabase.CreateAsset(asset, assetPath);
        AssetDatabase.SaveAssets();
        return asset;
    }

    public static void PromptAsset() 
    {
        if(AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) == null)
            CreateAsset();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));
    }

    #endif
}