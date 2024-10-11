using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class ItemKit
    {
        public static List<Item> Items = new List<Item>() 
        {
            new Item("Item_1", "物品1"),
            new Item("Item_2", "物品2"),
            new Item("Item_3", "物品3"),
            new Item("Item_4", "物品4"),
        };

        public static Dictionary<string, Item> ItemByKeyDict = new Dictionary<string, Item>()
        {
            {Items[0].Key, Items[0] },
            {Items[1].Key, Items[1] },
            {Items[2].Key, Items[2] },
            {Items[3].Key, Items[3] },
        };

        public static List<Slot> Slots = new List<Slot>()
            {
                new Slot(ItemByKeyDict[Items[0].Key], 2),
                new Slot(ItemByKeyDict[Items[1].Key], 4),
                new Slot(ItemByKeyDict[Items[2].Key], 1),
                new Slot(ItemByKeyDict[Items[3].Key], 8),
            };

        

        public static void AddItem(string key, int cnt = 1)
        {
            var solt = FindAdditiveSlotByKey(key);
            if (solt != null)
            {
                solt.Count += cnt;
            }
            else
            {
                Debug.Log("背包满了");
            }
        }

        public static bool SubItem(string key, int cnt = 1)
        {
            var solt = FindSoltByKey(key, false);
            if (solt != null)
            {
                if (solt.Count >= cnt)
                {
                    solt.Count -= cnt;
                    return true;
                }
            }
            return false;
        }

        static Slot FindSoltByKey(string key, bool isZeroResultsIncluded)
        {
            var solt = ItemKit.Slots.Find(s => s != null && s.Item != null && s.Item.Key == key);

            if (solt == null)
            {
                return null;
            }

            if (!isZeroResultsIncluded && solt.Count == 0)
            {
                return null;
            }
            return solt;
        }

        static Slot FindEmptySlot()
        {
            return ItemKit.Slots.Find(s => s != null && s.Count == 0);
        }

        static Slot FindAdditiveSlotByKey(string key)
        {
            var solt = FindSoltByKey(key, false);
            if (solt == null)
            {
                solt = FindEmptySlot();
                if (solt != null)
                {
                    solt.Item = ItemKit.ItemByKeyDict[key];
                }
            }
            return solt;
        }
    }
}

