using UnityEngine;
using UnityEditor;
using System.IO;

public class CardPrototypeAsset : ScriptableObject {

    [MenuItem("Assets/Create/Card")]
    static void CreateAsset() {
        CardPrototype asset = ScriptableObject.CreateInstance<CardPrototype>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if(path == "") {
            path = "Assets";
        } else if ( Path.GetExtension(path) != "") {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New Card.asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}