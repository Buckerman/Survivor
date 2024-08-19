//using QuangDM.Common;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace QuangDM.Common
//{
//    public class PlayerData
//    {
//        private static PlayerData instance;
//        public static PlayerData Instance
//        {
//            get
//            {
//                if (instance == null)
//                {
//                    instance = new PlayerData();
//                }
//                return instance;
//            }
//        }
//        public void Save()
//        {
//            string data = JsonUtility.ToJson(this);
//            DataManager.SaveData(SaveKey.PlayerData, data);
//        }
//        public void Load()
//        {
//            string data = DataManager.LoadData(SaveKey.PlayerData);
//            if(instance==null)
//            {
//                instance = new PlayerData();
//            }
//            instance = JsonUtility.FromJson<PlayerData>(data);
//        }

//        //property
//        public int AvatarID = 0;
//        public int ConversationID = 0;
//        public List<SaveFile> saveFile = new List<SaveFile>();
//    }
//    public class SaveFile
//    {
//        int currentLevel = 0;
//    }
//}
