namespace Assets.CBC.Editor
{
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEditor.Animations;
    using UnityEngine;

    [CreateAssetMenu(fileName = "AnimationParamProcessor", menuName = "Tools/CBC/AnimParamProcessor", order = 2)]
    public class AnimationParamProcessor : PostProcessorBase, IPostProcessor
    {
        private string[] _paths;

        public override bool CanHandle(string[] paths)
        {
            foreach(var path in paths)
            {
                Debug.Log(path + Path.GetExtension(path));
                if (IsControllerPath(path))
                {
                    _paths = paths;
                    return true;
                }
            }

            return false;
        }

        private static bool IsControllerPath(string path)
        {
            return Path.GetExtension(path) == ".controller";
        }

        public override void Process()
        {
            foreach (var path in _paths)
            {
                if (IsControllerPath(path))
                {
                    var animatorAsset = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
                    if (animatorAsset != null)
                    {
                        UpdateGlobalTags(animatorAsset);
                        foreach (var param in animatorAsset.parameters)
                        {
                            Debug.Log($"{param.name} {param.type}");
                        }
                    }
                }
            }
        }

        private static void UpdateGlobalTags(AnimatorController animatorController)
        {
            var controllerName = Sanitise(animatorController.name);

            using StreamWriter outfile =
                 new StreamWriter($"Assets/Global/{controllerName}Params.cs");
            outfile.WriteLine($"public static class {controllerName}Params");
            outfile.WriteLine("{");
            UpdateParamList(animatorController, outfile);
            outfile.WriteLine("}");
        }

        private static void UpdateParamList(AnimatorController animatorController, StreamWriter outfile)
        {
            foreach (var param in animatorController.parameters)
            {
                var sanitisedParam= Sanitise(param.name);

                outfile.WriteLine($"    public static string {sanitisedParam} = \"{param.name}\";");
            }
        }

        private static string Sanitise(string tag)
        {
            var sanitisedTag = tag.Replace(' ', '_');
            sanitisedTag = ToPascalCase(sanitisedTag);
            return sanitisedTag;
        }

        private static string ToPascalCase(string text)
        {
            const string pattern = @"(-|_)\w{1}|^\w";
            return Regex.Replace(text, pattern, match => match.Value.Replace("-", string.Empty).ToUpper());
        }
    }
}