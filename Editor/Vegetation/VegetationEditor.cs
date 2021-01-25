using System;
using System.Linq;
using AwesomeTechnologies;
using UnityEditor;
using UnityEngine;

public class VegetationEditor : EditorWindow
{
    private VegetationMaskArea vegetationMask;
    private string targetName;
    private string targetTag;
    private string spawnerContainerName;

    [MenuItem("CBC/Vegetation")]
    private static void OpenWindow()
    {
        GetWindow<VegetationEditor>("Vegetation Helper");
    }

    private void OnGUI()
    {
        Horizontal(() =>
        {
            EditorGUILayout.PrefixLabel("Name of Parent Object to add to");
            targetName = EditorGUILayout.TextField(targetName);
        });

        Horizontal(() =>
        {
            EditorGUILayout.PrefixLabel("Tag of Parent Object to add to");
            targetTag = EditorGUILayout.TextField(targetTag);
        });

        Horizontal(() =>
        {
            EditorGUILayout.PrefixLabel("Vegetation Mask Prefab");
            vegetationMask = EditorGUILayout.ObjectField(vegetationMask, typeof(VegetationMaskArea), allowSceneObjects: true) as VegetationMaskArea;
            if (GUI.changed || string.IsNullOrEmpty(spawnerContainerName))
            {
                spawnerContainerName = vegetationMask.transform.parent.parent.name;
            }
        });

        EditorGUILayout.LabelField("Prefab is from Spawner " + spawnerContainerName);

        if (GUI.Button(new Rect(10, 90, 100, 30), "Add Masks"))
        {
            AddMasks();
        }

        if (GUI.Button(new Rect(120, 90, 100, 30), "Remove Masks"))
        {
            RemoveMasks();
        }
    }

    private void RemoveMasks()
    {
        Debug.Log($"Remove Masks from {targetName}");
        var taggedItems = GameObject.FindGameObjectsWithTag(targetTag);
        if (taggedItems != null)
        {
            var namedItems = taggedItems.Where(t => t.name == targetName);
            foreach(var item in namedItems)
            {
                var maskArea = item.GetComponentInChildren<VegetationMaskArea>();
                if (maskArea != null)
                {
                    if (maskArea.GetInstanceID() != vegetationMask.GetInstanceID())
                    {
                        GameObject.DestroyImmediate(maskArea.gameObject);
                    }
                }
            }
        }
    }

    private void AddMasks()
    {
        Debug.Log($"Add Masks to {targetName}");
        var taggedItems = GameObject.FindGameObjectsWithTag(targetTag);
        if (taggedItems != null)
        {
            var namedItems = taggedItems.Where(t => t.name == targetName && t.transform.parent.name == spawnerContainerName);
            if (namedItems.Any())
            {
                foreach (var item in namedItems)
                {
                    // Debug.Log($"Checking item {item.name}");
                    var maskArea = item.GetComponentInChildren<VegetationMaskArea>();
                    if (maskArea == null)
                    {
                        var maskContainer = new GameObject("VegetationMaskContainer");
                        var maskComponent = maskContainer.AddComponent<VegetationMaskArea>();
                        maskComponent.Nodes.Clear();
                        maskComponent.Nodes.AddRange(vegetationMask.Nodes);
                        maskContainer.transform.SetParent(item.transform, worldPositionStays: false);
                    }
                    else
                    {
                        Debug.Log($"Item {item.name} already has a VegetationMaskArea");
                    }
                }
            }
            else
            {
                Debug.Log($"No tagged items tagged with {targetTag} called {targetName}");
            }
        }
        else
        {
            Debug.Log($"No items tagged with {targetTag}");
        }
    }

    private void Horizontal(Action contents, GUIStyle style = default, params GUILayoutOption[] options)
    {
        if (style == default)
        {
            style = GUIStyle.none;
        }

        EditorGUILayout.BeginHorizontal(style, options);
        contents();
        EditorGUILayout.EndHorizontal();
    }
}
