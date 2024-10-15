using Qframework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QFramework
{
    public class ItemKit
    {
        public static QFramework.Example.UISlot CurrentSlotPointerOn = null;

        public static IItemKitSaverAndLoader SaveAndLoader = new DefaultSaverAndLoader();

        public static Dictionary<string, SlotGroup> SlotGroupByKey = new Dictionary<string, SlotGroup>();

        public static List<IItem> Items = new List<IItem>();

        public static Dictionary<string, IItem> ItemByKeyDict = new Dictionary<string, IItem>();

        public static void LoadItemDatabase(string configName)
        {
            var config = Resources.Load<ItemDatabase>(configName);
            foreach (var item in config.ItemConfigs)
            {
                AddItemConfig(item);
            }
        }

        public static void AddItemConfig(IItem item)
        {
            Items.Add(item);
            ItemByKeyDict.Add(item.GetKey(), item);
        }

        public static SlotGroup CreateSlotGroup(string key)
        {
            if (SlotGroupByKey.ContainsKey(key))
            {
                return null;
            }
            SlotGroupByKey.Add(key, new SlotGroup(key));
            return SlotGroupByKey[key];
        }

        public static SlotGroup GetSlotGroupByKey(string key)
        {
            if (!SlotGroupByKey.ContainsKey(key))
            {
                return null;
            }
            return SlotGroupByKey[key];
        }

        

        public static void Save() => SaveAndLoader.Save(SlotGroupByKey);

        public static void Load() => SaveAndLoader.Load(SlotGroupByKey);
    }
}

