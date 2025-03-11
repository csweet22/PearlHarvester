
    using Scripts.Utilities;
    using UnityEngine.Serialization;

    public class Settings : PersistentSingleton<Settings>
    {
        public float sensitivity = 0.5f;
        public float fov = 75f;
    }
