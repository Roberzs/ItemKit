using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace QFramework
{
    

    [CreateAssetMenu(menuName = "ItemKit/ Create ItemConfig")]
    public class ItemConfig : ScriptableObject, IItem
    {
        [DisplayLabel("名称")]
        public string Name = string.Empty;
        [DisplayLabel("关键字")]
        public string Key = string.Empty;
        [DisplayLabel("是武器")]
        public bool IsWeapon = false;
        [DisplayLabel("是否可堆叠")]
        public bool Stackable = true;
        [DisplayIf(nameof(Stackable), false, true)]
        [DisplayLabel("     是否有最大数量")]
        public bool HasMaxStackableCount = false;
        [DisplayIf(new string[] { nameof(Stackable), nameof(HasMaxStackableCount) }, new bool[] { false,false }, true)]
        [DisplayLabel("         最大数量")]
        public int MaxStackableCount;

        public Sprite Icon;

        public bool IsStackable => Stackable;

        bool IItem.HasMaxStackableCount => HasMaxStackableCount;

        int IItem.MaxStackableCount => MaxStackableCount;

        public Sprite GetIcon() => Icon;

        public string GetKey() => Key;

        public string GetName() => Name;

        public bool GetBoolean(string propertyName)
        {
            if (propertyName == "IsWeapon")
            {
                return IsWeapon;
            }
            return false;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ItemConfig))]
    public class ItemConfigInspector : Editor
    {
        SerializedProperty _keySp;
        SerializedProperty _iconSp;

        private void OnEnable()
        {
            _keySp = serializedObject.FindProperty("Key");
            _iconSp = serializedObject.FindProperty("Icon");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            serializedObject.DrawProperties(false, 0, "Icon");

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("图标");
            _iconSp.objectReferenceValue = EditorGUILayout.ObjectField(_iconSp.objectReferenceValue, typeof(Sprite),
                true, GUILayout.Width(48), GUILayout.Height(48));
            EditorGUILayout.EndHorizontal();

            if (_keySp.stringValue != target.name)
            {
                target.name = _keySp.stringValue;
                EditorUtility.SetDirty(target);
                //AssetDatabase.SaveAssetIfDirty(target);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}

