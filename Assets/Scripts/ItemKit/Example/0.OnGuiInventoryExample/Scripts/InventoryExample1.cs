using UnityEngine;
using QFramework;
using System.Collections.Generic;
using Qframework.Example;

namespace QFramework.Example
{
    public partial class InventoryExample1 : ViewController
    {
        const string BagName = "背包";

        private void Start()
        {
            ItemKit.CreateSlotGroup(BagName)
                .CreateSlot(ItemKit.ItemByKeyDict[Items.Item_Iron], 1)
                .CreateSlot(ItemKit.ItemByKeyDict[Items.Item_GreenSword], 1)
                .CreateSlot(null, 0);

        }

        private void OnGUI()
        {
            IMGUIHelper.SetDesignResolution(640, 360);

            foreach (var slot in ItemKit.GetSlotGroupByKey(BagName).Slots)
            {
                GUILayout.BeginHorizontal("box");
                if (slot.Count > 0)
                {
                    GUILayout.Label($"{slot.Item.GetName()}x{slot.Count}");
                }
                else
                {
                    GUILayout.Label("空");
                }

                GUILayout.EndHorizontal();
            }

            for (int i = 0; i < ItemKit.GetSlotGroupByKey(BagName).Slots.Count; i++)
            {
                var idx = i + 1;
                var key = ItemKit.Items[i].GetKey();
                GUILayout.BeginHorizontal();
                GUILayout.Label("物品" + idx);
                if (GUILayout.Button("+"))
                {
                    ItemKit.GetSlotGroupByKey(BagName).AddItem(key, 1);
                }
                if (GUILayout.Button("-"))
                {
                    ItemKit.GetSlotGroupByKey(BagName).SubItem(key, 1);
                }
                GUILayout.EndHorizontal();
            }
        }

        
    }
}
