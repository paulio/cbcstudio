namespace Assets.CBC.Scripts.Helpers
{
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class MonoExtensions
    {


        public static T GetComponent<T>(this Component component, string nullMessage)
        {
            var result = component.GetComponent<T>();
            if (result.Equals(null))
            {
                var type = typeof(T);
                Debug.LogError($"{nullMessage}: Was expecting {type.Name} in {component.name}", component);
            }

            return result;
        }

        public static T[] GetComponentsInChildren<T>(this Component component, string nullMessage)
        {
            var result = component.GetComponentsInChildren<T>();
            if (result.Equals(null))
            {
                var type = typeof(T);
                Debug.LogError($"{nullMessage}: Was expecting one or more {type.Name} in the children of {component.name}", component);
            }

            return result;
        }
    }
}

