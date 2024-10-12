using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace QFramework.Example
{
    public partial class InventoryExample1 : ViewController
    {
        

        private void Start()
        {
            

        }

        private void OnGUI()
        {
            IMGUIHelper.SetDesignResolution(640, 360);

            foreach (var slot in ItemKit.Slots)
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

            for (int i = 0; i < ItemKit.Slots.Count; i++)
            {
                var idx = i + 1;
                var key = ItemKit.Items[i].GetKey();
                GUILayout.BeginHorizontal();
                GUILayout.Label("物品" + idx);
                if (GUILayout.Button("+"))
                {
                    ItemKit.AddItem(key, 1);
                }
                if (GUILayout.Button("-"))
                {
                    ItemKit.SubItem(key, 1);
                }
                GUILayout.EndHorizontal();
            }
        }

        
    }
}
