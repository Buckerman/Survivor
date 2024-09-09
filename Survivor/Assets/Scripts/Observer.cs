using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

namespace QuangDM.Common
{
    public static class EventName
    {
        public static readonly string DisableAllEnemies = "DisableAllEnemies";
        public static readonly string DisableAllDamageText = "DisableAllDamageText";
        public static readonly string DisableAllLoot = "DisableAllLoot";
        public static readonly string DisableAllBloodSplash = "DisableAllBloodSplash";
        public static readonly string WaveCompleted = "WaveCompleted";
        public static readonly string TimeLeft = "TimeLeft";
        public static readonly string DamageReceived = "DamageReceived";
        public static readonly string ReactivatePlatform = "ReactivatePlatform";
        public static readonly string DropLoot = "DropLoot";
        public static readonly string UpdateWalletUI = "UpdateWalletUI";
        public static readonly string CurrentWaveLevel = "CurrentWaveLevel";
        public static readonly string Joy = "Joy";
        public static readonly string Slash = "Slash";
        public static readonly string PickUpAllLoot = "PickUpAllLoot";
        public static readonly string BloodSpawn = "BloodSpawn";
        public static readonly string PlayerLevelUp = "PlayerLevelUp";
    }
    public class Observer : MonoBehaviour //do rozdzielenia zaleznosci miedzy skryptami, wywolywanie zmian w UI, zliczania, questy
    {

        public static Observer Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public delegate void CallBackObserver(System.Object data);

        Dictionary<string, HashSet<CallBackObserver>> dictObserver = new Dictionary<string, HashSet<CallBackObserver>>();
        // Use this for initialization
        public void AddObserver(string topicName, CallBackObserver callbackObserver)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            listObserver.Add(callbackObserver);
        }

        public void RemoveObserver(string topicName, CallBackObserver callbackObserver)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            listObserver.Remove(callbackObserver);
        }

        //public void Notify<OData>(string topicName, OData Data) where OData : MonoBehaviour
        //{
        //    HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
        //    foreach (CallBackObserver observer in listObserver)
        //    {
        //        observer(Data);
        //    }
        //}
        public void Notify(string topicName, System.Object Data)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            foreach (CallBackObserver observer in listObserver)
            {
                observer(Data);
            }
        }
        public void Notify(string topicName)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            foreach (CallBackObserver observer in listObserver)
            {
                observer(null);
            }
        }

        protected HashSet<CallBackObserver> CreateListObserverForTopic(string topicName)
        {
            if (!dictObserver.ContainsKey(topicName))
                dictObserver.Add(topicName, new HashSet<CallBackObserver>());
            return dictObserver[topicName];
        }

    }
}