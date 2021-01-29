using System;
using AwesomeTechnologies;
using AwesomeTechnologies.VegetationSystem;
using AwesomeTechnologies.VegetationSystem.Biomes;
using UnityEditor;
using UnityEngine;

public class BiomeHelperEditor : EditorWindow
{
    Transform _biomeParent;
    BiomeType _biomeType;
    string _startWithMask;
    GameObject _areaMaskContainer;

    [MenuItem("CBC/Area to Biome Converter")]
    private static void OpenWindow()
    {
        GetWindow<BiomeHelperEditor>("Area To Biome Converter");
    }

    private void OnGUI()
    {
        Horizontal(() =>
        {
            EditorGUILayout.PrefixLabel("Start with filter");
            _startWithMask = EditorGUILayout.TextField(_startWithMask);
        });

        Horizontal(() =>
        {
            EditorGUILayout.PrefixLabel("Biome Masks Container");
            _biomeParent = EditorGUILayout.ObjectField(_biomeParent, typeof(Transform), allowSceneObjects: true) as Transform;
        });

        Horizontal(() =>
        {
            EditorGUILayout.PrefixLabel("Biome Type");
            _biomeType = (BiomeType)EditorGUILayout.EnumPopup(_biomeType);
        });

        Horizontal(() =>
        {
            EditorGUILayout.PrefixLabel("Where to search from");
            _areaMaskContainer = EditorGUILayout.ObjectField(_areaMaskContainer, typeof(GameObject), allowSceneObjects: true) as GameObject;
        });

        if (GUI.Button(new Rect(10, 90, 200, 30), "Convert Area to Biome Masks"))
        {
            Convert();
        }
    }

    void Convert()
    {

        var defaultGameObject = new GameObject();
        var childMasks = _areaMaskContainer.GetComponentsInChildren<VegetationMaskArea>();
        foreach (var childMask in childMasks)
        {
            if (childMask.enabled && childMask.name.StartsWith(_startWithMask))
            {
                var nodes = childMask.Nodes;
                var biomeMaskGameObject = Instantiate(defaultGameObject, _biomeParent.transform);
                biomeMaskGameObject.transform.position = new Vector3(childMask.transform.position.x, childMask.transform.position.y, childMask.transform.position.z);
                biomeMaskGameObject.name = "B" + childMask.name;
                var biomeMaskArea = biomeMaskGameObject.AddComponent<BiomeMaskArea>();
                biomeMaskArea.ClearNodes();
                foreach (var node in nodes)
                {
                    biomeMaskArea.AddNode(new Vector3(node.Position.x, node.Position.y, node.Position.z));
                }

                // for some strange reason you have to reassign the positions again otherwise they all have an incorrect offset??
                for (int x = 0; x < nodes.Count; x++)
                {
                    var node = nodes[x];
                    var bNode = biomeMaskArea.Nodes[x];
                    bNode.Position = new Vector3(node.Position.x, node.Position.y, node.Position.z);
                }

                biomeMaskArea.BiomeType = _biomeType;
                childMask.enabled = false;
            }
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
