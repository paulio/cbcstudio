namespace Assets.CBC.Scripts.Helpers
{
    using UnityEngine;

    public static class MonoExtensions
    {
        public static T[] GetComponentsInChildren<T>(this MonoBehaviour monoBehaviour, string nullMessage)
        {
            var result = monoBehaviour.GetComponentsInChildren<T>();
            if (result == null)
            {
                Debug.LogError(nullMessage);
            }

            return result;
        }
    }
}

