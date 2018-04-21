using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Deck))]
public class DeckCustomEditor : Editor {

    public override void OnInspectorGUI() {
        CardList(serializedObject.FindProperty("library"));
        CardList(serializedObject.FindProperty("discard"));
    }

    public void CardList(SerializedProperty list) {
        EditorGUILayout.PropertyField(list);
        EditorGUI.indentLevel++;
        if(list.isExpanded) {
            for(var i = 0; i < list.arraySize; i++) {
                var cardRef = list.GetArrayElementAtIndex(i);
                var name = cardRef.FindPropertyRelative("name").stringValue;

                EditorGUILayout.LabelField(name);
            }
        }
        EditorGUI.indentLevel--;
    }
}
