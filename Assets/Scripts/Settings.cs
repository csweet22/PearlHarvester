
    using Scripts.Utilities;
    using UnityEngine.Serialization;

    public class Settings : PersistentSingleton<Settings>
    {
        public float sensitivity = 0.2f;
        public float fov = 75f;
        public bool showTimer = false;
    }
