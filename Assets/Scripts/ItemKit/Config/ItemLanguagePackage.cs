using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QFramework
{
    [CreateAssetMenu(menuName = "@ItemKit/Create Language Package File")]
    public class ItemLanguagePackage : ScriptableObject
    {
        [DisplayLabel("语言")]
        public string Language;
        [DisplayLabel("对应文件")]
        public ItemDatabase ItemDataBase;
        [Header("本地化列表")]
        public List<LocaleItem> LocaleItems = new List<LocaleItem>();

        [Serializable]
        public class LocaleItem
        {
            public string Key;
            public string Name;
            public string Description;
        }

        private void OnValidate()
        {
            if (ItemDataBase == null || ItemDataBase.ItemConfigs == null)
            {
                LocaleItems.Clear();
                return;
            }
            if (ItemDataBase.ItemConfigs.Count == 0) 
            {
                foreach (var item in ItemDataBase.ItemConfigs) 
                {
                    LocaleItems.Add(CreateLocaleItemByItemConfig(item));
                }
            }
            else
            {
                LocaleItems.RemoveAll(item => ItemDataBase.ItemConfigs.All(i => i.GetKey() != item.Key));
                var newLocaleItems = new List<LocaleItem>();
                foreach (var item in ItemDataBase.ItemConfigs)
                {
                    var localItem2Add = LocaleItems.Find(i => i.Key == item.GetKey());
                    if (localItem2Add != null)
                    {
                        newLocaleItems.Add(localItem2Add);
                    }
                    else
                    {
                        newLocaleItems.Add(CreateLocaleItemByItemConfig(item));
                    }
                }
                LocaleItems = newLocaleItems;
            }
        }

        private LocaleItem CreateLocaleItemByItemConfig(ItemConfig itemConfig)
        {
            return new LocaleItem
            {
                Key = itemConfig.Key,
                Name = itemConfig.Name,
                Description = itemConfig.Description,
            };
        }
    }
}

