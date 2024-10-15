using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlotGroupTemplete : MonoBehaviour
{
    [Serializable]
    public class SlotConfig
    {
        public ItemConfig Item;
        public int Count;
    }

    public string GroupKey;
    [Header("初始格子")]
    public List<SlotConfig> InitSlotConfigs;
    [DisplayLabel("关联的UISlotGroup")]
    public UISlotGroup UISlotGroup;

    private void Awake()
    {
        var group = ItemKit.CreateSlotGroup(GroupKey);
        foreach (var slotConfig in InitSlotConfigs)
        {
            group.CreateSlot(slotConfig.Item, slotConfig.Count);
        }
    }

    public void Open()
    {
        UISlotGroup.ReflushWithGroupKey(GroupKey);
        UISlotGroup.Show();
    }

    public void Close()
    {
        UISlotGroup.Hide();
    }
}
