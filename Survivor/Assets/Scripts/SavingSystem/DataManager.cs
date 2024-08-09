using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuangDM.Common
{
    public enum SaveKey
    {
        PlayerData
    }

    public class DataManager
    {
        public static void SaveData(SaveKey key, string data = "")
        {
            PlayerPrefs.SetString(key.ToString(), data);
            PlayerPrefs.Save();
        }
        public static string LoadData(SaveKey key)
        {
            string result = string.Empty;
            result = PlayerPrefs.GetString(key.ToString(), string.Empty);
            return result;
        }
    }
}