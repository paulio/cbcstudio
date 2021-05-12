namespace Assets.CBC.Editor
{
    public interface IPostProcessor
    {
        bool CanHandle(string[] paths);
        void Process();
    }
}
