namespace Assets.CBC.Editor
{
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UnityEngine;

    [CreateAssetMenu(fileName = "TagProcessor", menuName = "Tools/CBC/TagProcessor", order = 2)]
    public class TagProcessor : PostProcessorBase, IPostProcessor
    {

        public override bool CanHandle(string[] paths)
        {
            return paths.Contains("ProjectSettings/TagManager.asset");
        }

        public override void Process()
        {
            UpdateGlobalTags();
        }

        private static void UpdateGlobalTags()
        {
            Debug.Log("Generating tag file");

            using StreamWriter outfile =
                 new StreamWriter("Assets/Global/GlobalTags.cs");
            outfile.WriteLine("public static class GlobalTags");
            outfile.WriteLine("{");
            UpdateTagList(outfile);
            outfile.WriteLine("}");
        }

        private static void UpdateTagList(StreamWriter outfile)
        {
            foreach (var tag in UnityEditorInternal.InternalEditorUtility.tags)
            {
                var sanitisedTag = tag.Replace(' ', '_');
                sanitisedTag = ToPascalCase(sanitisedTag);

                outfile.WriteLine($"    public static string {sanitisedTag} = \"{tag}\";");
            }
        }

        private static string ToPascalCase(string text)
        {
            const string pattern = @"(-|_)\w{1}|^\w";
            return Regex.Replace(text, pattern, match => match.Value.Replace("-", string.Empty).ToUpper());
        }
    }
}