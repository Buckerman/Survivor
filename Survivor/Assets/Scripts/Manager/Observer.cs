using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

namespace QuangDM.Common
{
    public static class EventName
    {
        public static readonly string PlayerOutSafeZone = "PlayerOutSafeZone";
        public static readonly string PlayerGoIntoBossZone = "PlayerGoIntoBossZone";
        public static readonly string DropHandle = "DropHandle";
        public static readonly string BossDie = "BossDie";
        public static readonly string GetEssences = "GetEssences";
        public static readonly string GetRunes = "GetRunes";
        public static readonly string PlayerEndGame = "PlayerEndGame";
        public static readonly string SpawnMap = "SpawnMap";
        public static readonly string LoadingCombatDone = "LoadingCombatDone";
        public static readonly string PlayerInChestZone = "PlayerInChestZone";
        public static readonly string PlayerOutChestZone = "PlayerOutChestZone";
        public static readonly string ReturnBase = "ReturnBase";
        public static readonly string GetBoosterPowerUp = "GetBoosterPowerUp";
        public static readonly string GetChestInGame = "GetChestInGame";
        public static readonly string LoadMapDone = "LoadMapDone";
        public static readonly string PlayerEffectUpdate = "PlayerEffectUpdate";
        public static readonly string PlayerInDoorSystem = "PlayerInDoorSystem";
        public static readonly string PlayerOutDoorSystem = "PlayerOutDoorSystem";
    }
    public class Observer : MonoBehaviour //do rozdzielenia zaleznosci miedzy skryptami, wywolywanie zmian w UI, zliczania, questy
    {

        public static Observer Instance;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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