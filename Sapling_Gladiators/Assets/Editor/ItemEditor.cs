// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(Item))]
// [CanEditMultipleObjects]
// public class ItemEditor : Editor
// {

//     Item itemInstance;

//     private void OnEnable() 
//     {
//         //Undo.RecordObject(target, "Item Script");
// 		itemInstance = (Item)target;
//     }

//     public override void OnInspectorGUI()
//     {
//         // if (itemInstance == null)
//         // {
//         //     Debug.LogError("It's Null you retard");
//         // }
//         // else
//         // {
//         //     Debug.Log("It's fine");
//         // }

//         EditorGUILayout.Space();
//         itemInstance.itemName = EditorGUILayout.TextField("Item Name", itemInstance.itemName);
//         itemInstance.numOfUses = EditorGUILayout.FloatField("Number of Uses", itemInstance.numOfUses);

//         System.Type activationScriptType = typeof(IActivatable);

//         EditorGUILayout.ObjectField((UnityEngine.Object)itemInstance.activationScript, activationScriptType);

//         serializedObject.Update();
//         serializedObject.ApplyModifiedProperties();

//         EditorUtility.SetDirty(itemInstance);
//     }
// }
