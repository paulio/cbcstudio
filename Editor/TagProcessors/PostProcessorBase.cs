namespace Assets.CBC.Editor
{
    using UnityEngine;

    public class PostProcessorBase : ScriptableObject, IPostProcessor
    {
        public virtual bool CanHandle(string[] paths)
        {
            return false;
        }

        public virtual void Process()
        {
        }
    }
}
