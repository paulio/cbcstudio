using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.CBC.Editor;
using UnityEngine;

public class TagPostProcessor : UnityEditor.AssetModificationProcessor
{
    private static readonly TagProcessorContainer _tagProcessorContainer = null;

    [SuppressMessage("Unity", "CA1810", Justification = "Want to see results of constructor")]
    static TagPostProcessor()
    {
        Debug.Log("TagPostProcessor ctr");
        _tagProcessorContainer = Resources.Load<TagProcessorContainer>("TagProcessorContainer");
        if (_tagProcessorContainer == null)
        {
            Debug.LogError("Tag process container not loaded. Please create a TagProcessorContainer in /Assets/Resources");
            return;
        }

        foreach(var processor in _tagProcessorContainer.TagProcessors)
        {
            Debug.Log($"TagPostProcessor Loaded {processor.name}");
        }
    }

    [SuppressMessage("CodeQuality", "IDE0051", Justification = "It's a Unity Message")]
    static string[] OnWillSaveAssets(string[] paths)
    {
        if (_tagProcessorContainer != null)
        {
            foreach (var processor in _tagProcessorContainer.TagProcessors)
            {
                if (processor.CanHandle(paths))
                {
                    processor.Process();
                }
            }
        }

        return paths;
    }
}
