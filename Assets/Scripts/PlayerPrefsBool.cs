using UnityEngine;

namespace UrbanNinja
{
    public static class PlayerPrefsBool
    {
        public static bool GetBool(string key)
            => PlayerPrefs.GetInt(key) == 1;

        public static void SetBool(string key, bool value)
            => PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
}