namespace Assets.CBC.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    [CreateAssetMenu(fileName = "TagProcessorContainer", menuName = "Tools/CBC/TagProcessorContainer", order = 1)]
    public class TagProcessorContainer : ScriptableObject
    {
        [SerializeField]
        List<PostProcessorBase> _tagProcessors = default;

        public List<PostProcessorBase> TagProcessors => _tagProcessors;
    }
}
