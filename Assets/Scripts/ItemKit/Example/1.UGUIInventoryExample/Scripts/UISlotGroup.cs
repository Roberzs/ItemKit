using QFramework.Example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace QFramework
{
    

    public class UISlotGroup : MonoBehaviour
    {
        public enum UISlotGenerateType
        {
            GenerateTemplete,
            UseExistUISlot
        }

        public string Key = "Key";

        [DisplayLabel("生成类型")]
        public UISlotGenerateType GenerateType;

        [DisplayIf(nameof(GenerateType), false, UISlotGenerateType.GenerateTemplete)]
        public QFramework.Example.UISlot UISlotTemplete;
        [DisplayIf(nameof(GenerateType), false, UISlotGenerateType.GenerateTemplete)]
        public RectTransform UISlotRoot;
        //[DisplayIf(nameof(GenerateType), false, UISlotGenerateType.UseExistUISlot)]
        public List<QFramework.Example.UISlot> UISlots;

        private void Start()
        {
            if (GenerateType == UISlotGenerateType.GenerateTemplete)
            {
                UISlotTemplete.Hide();
                ReflushUISlot();
            }
            else if (GenerateType == UISlotGenerateType.UseExistUISlot)
            {
                for (int  i = 0; i < UISlots.Count; i++)
                {
                    UISlots[i].InitWithSlot(ItemKit.GetSlotGroupByKey(Key).Slots[i]);
                }
            }
        }

        public void ReflushUISlot()
        {
            UISlotRoot.DestroyChildrenWithCondition(t=>t.transform.name != "BagContainer");

            foreach (var slot in ItemKit.GetSlotGroupByKey(Key).Slots)
            {
                UISlotTemplete.InstantiateWithParent(UISlotRoot)
                    .InitWithSlot(slot)
                    .Show();
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UISlotGroup))]
    public class UISlotGroupInspector : Editor
    {
        private static UISlotGroup _target;

        private void OnEnable()
        {
            _target = target as UISlotGroup;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (_target.GenerateType != UISlotGroup.UISlotGenerateType.UseExistUISlot)
            {
                serializedObject.DrawProperties(true, 0, "UISlots");
            }
            else
            {
                serializedObject.DrawProperties(true, 0);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
