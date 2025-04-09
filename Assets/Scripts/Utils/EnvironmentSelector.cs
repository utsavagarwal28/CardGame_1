using UnityEngine;

namespace Game.Utils
{
    
    public static class EnvironmentSelector
    {
#if UNITY_EDITOR
        public static string Environment => "development";
#else
        public static string Environment => "production";
#endif
    }

}