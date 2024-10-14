using Qframework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class ItemKit
    {
        public static QFramework.Example.UISlot CurrentSlotPointerOn = null;

        public static Dictionary<string, SlotGroup> SlotGroupByKey = new Dictionary<string, SlotGroup>();

        public static List<IItem> Items = new List<IItem>();

        public static Dictionary<string, IItem> ItemByKeyDict = new Dictionary<string, IItem>();

        //public static List<Slot> Slots = new List<Slot>();

        //public static List<Slot> BagSlots = new List<Slot>();

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


        //public static void CreateSlot(IItem item, int count)
        //{
        //    Slots.Add(new Slot(item, count));
        //}

        //public static void AddItem(string key, int cnt = 1)
        //{
        //    var solt = FindAdditiveSlotByKey(key);
        //    if (solt != null)
        //    {
        //        solt.Count += cnt;
        //    }
        //    else
        //    {
        //        Debug.Log("背包满了");
        //    }
        //}

        //public static bool SubItem(string key, int cnt = 1)
        //{
        //    var solt = FindSlotByKey(key, false);
        //    if (solt != null)
        //    {
        //        if (solt.Count >= cnt)
        //        {
        //            solt.Count -= cnt;
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //static Slot FindSlotByKey(string key, bool isZeroResultsIncluded)
        //{
        //    var solt = ItemKit.Slots.Find(s => s != null && s.Item != null && s.Item.GetKey() == key);

        //    if (solt == null)
        //    {
        //        return null;
        //    }

        //    if (!isZeroResultsIncluded && solt.Count == 0)
        //    {
        //        return null;
        //    }
        //    return solt;
        //}

        //static Slot FindEmptySlot()
        //{
        //    return ItemKit.Slots.Find(s => s != null && s.Count == 0);
        //}

        //static Slot FindAdditiveSlotByKey(string key)
        //{
        //    var solt = FindSlotByKey(key, false);
        //    if (solt == null)
        //    {
        //        solt = FindEmptySlot();
        //        if (solt != null)
        //        {
        //            solt.Item = ItemKit.ItemByKeyDict[key];
        //        }
        //    }
        //    return solt;
        //}
    }
}

