using Qframework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QFramework
{
    public interface IItemKitSaverAndLoader
    {
        void Load(Dictionary<string, SlotGroup> SlotGroupDict);
        void Save(Dictionary<string, SlotGroup> SlotGroupDict);
        void Delete();
    }

    public class DefaultSaverAndLoader : IItemKitSaverAndLoader
    {
        [Serializable]
        class SlotGroupSaveData
        {
            public string Key;
            public List<SlotSaveData> SlotSaveDatas;
        }

        [Serializable]
        class SlotSaveData
        {
            public string ItemKey;
            public int Count;
        }

        public void Delete()
        {
            string keysString = PlayerPrefs.GetString("slot_group_keys", "");
            if (keysString.IsNotNullAndEmpty())
            {
                string[] keys = keysString.Split('@');
                for (int i = 0; i <keys.Length; i++) 
                {
                    PlayerPrefs.DeleteKey("slot_group_"+ keys[i]);
                }
            }
            PlayerPrefs.DeleteKey("slot_group_keys");
        }

        public void Load(Dictionary<string, SlotGroup> SlotGroupDict)
        {
            string keysString = PlayerPrefs.GetString("slot_group_keys", "");
            if (keysString.IsNotNullAndEmpty())
            {
                string[] keys = keysString.Split('@');
                foreach (string key in keys) 
                {
                    var json = PlayerPrefs.GetString("slot_group_" + key , "");
                    if (json.IsNullOrEmpty())
                    {

                    }
                    else
                    {
                        var data = JsonUtility.FromJson<SlotGroupSaveData>(json);
                        var slotGroup = ItemKit.GetSlotGroupByKey(key);
                        if (slotGroup == null)
                        {
                            slotGroup = ItemKit.CreateSlotGroup(data.Key);
                        }

                        for (int i = 0; i < data.SlotSaveDatas.Count; i++)
                        {
                            var item = data.SlotSaveDatas[i].ItemKey.IsNullOrEmpty() ? null
                                : ItemKit.ItemByKeyDict[data.SlotSaveDatas[i].ItemKey];
                            if (i < slotGroup.Slots.Count)
                            {
                                // 覆盖
                                slotGroup.Slots[i].Item = item;
                                slotGroup.Slots[i].Count = data.SlotSaveDatas[i].Count;
                            }
                            else
                            {
                                slotGroup.CreateSlot(item, data.SlotSaveDatas[i].Count);
                            }
                            slotGroup.Slots[i].Changed.Trigger();
                        }
                    }
                }
            }
        }

        public void Save(Dictionary<string, SlotGroup> SlotGroupDict)
        {
            string keysString = string.Join("@", SlotGroupDict.Keys.ToList());
            PlayerPrefs.SetString("slot_group_keys", keysString);

            foreach (var data in SlotGroupDict.Values)
            {
                var slotGroupSaveData = new SlotGroupSaveData();
                slotGroupSaveData.Key = data.Key;
                slotGroupSaveData.SlotSaveDatas = data.Slots.Select(slot =>
                        new SlotSaveData()
                        {
                            ItemKey = slot.Item == null ? null : slot.Item.GetKey(),
                            Count = slot.Count
                        }
                        ).ToList();
                var saveData = JsonUtility.ToJson(slotGroupSaveData);
                PlayerPrefs.SetString("slot_group_" + slotGroupSaveData.Key, saveData);
            }
        }
    }
}
