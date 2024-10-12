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
        public Sprite Icon;

        public Sprite GetIcon() => Icon;

        public string GetKey() => Key;

        public string GetName() => Name;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ItemConfig))]
    public class ItemConfigInspector : Editor
    {
        SerializedProperty _iconSp;

        private void OnEnable()
        {
            _iconSp = serializedObject.FindProperty("Icon");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //base.OnInspectorGUI();
            serializedObject.DrawProperties(false, 0, "Icon");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("图标");
            _iconSp.objectReferenceValue = EditorGUILayout.ObjectField(_iconSp.objectReferenceValue, typeof(Sprite), true, GUILayout.Width(48), GUILayout.Height(48));
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}

