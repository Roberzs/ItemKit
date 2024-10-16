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

        public static IItemKitLoader ItemKitLoader = new DefaultItemKitLoader();

        public const string DefaultLanguagePackage = "zh_cn";
        public static string CurrentLanguagePackage = DefaultLanguagePackage;

        public static void LoadLanagePackage(string lanagePackageName)
        {
            if (lanagePackageName == CurrentLanguagePackage)
            {

            }
            else if (lanagePackageName == DefaultLanguagePackage)
            {
                foreach (var item in ItemByKeyDict.Values)
                {
                    item.LocaleItem = null;
                }
            }
            else if (lanagePackageName != CurrentLanguagePackage)
            {
                var lanagePackage = ItemKitLoader.LoadLanagePackage(lanagePackageName);// Resources.Load<ItemLanguagePackage>(lanagePackageName);
                foreach (var locale in lanagePackage.LocaleItems)
                {
                    if (ItemByKeyDict.TryGetValue(locale.Key, out var item))
                    {
                        item.LocaleItem = locale;
                    }
                }
            }
            CurrentLanguagePackage = lanagePackageName;
        }

        public static void LoadItemDatabase(string configName)
        {
            var config = ItemKitLoader.LoadItemDatabase(configName);// Resources.Load<ItemDatabase>(configName);
            foreach (var item in config.ItemConfigs)
            {
                AddItemConfig(item);
            }
        }

        public static void AddItemConfig(IItem item)
        {
            Items.Add(item);
            item.LocaleItem = null;
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

