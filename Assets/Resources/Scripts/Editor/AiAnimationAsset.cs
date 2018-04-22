using UnityEngine;
using UnityEditor;
using System.IO;

public class AiAnimationAsset : ScriptableObject {

    [MenuItem("Assets/Create/Ai Animation")]
    static void CreateAsset() {
        AiAnimation asset = ScriptableObject.CreateInstance<AiAnimation>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if(path == "") {
            path = "Assets";
        } else if ( Path.GetExtension(path) != "") {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New Animation.asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}