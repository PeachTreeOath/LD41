using UnityEngine;
using UnityEditor;
using System.IO;

public class SpawnAsset : ScriptableObject {

    [MenuItem("Assets/Create/Spawn")]
    static void CreateAsset() {
        Spawn asset = ScriptableObject.CreateInstance<Spawn>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if(path == "") {
            path = "Assets";
        } else if ( Path.GetExtension(path) != "") {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New Spawn.asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}