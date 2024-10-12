using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace QFramework 
{
    [CreateAssetMenu(menuName = "@ItemKit/Create ItemDatabase File")]
    public class ItemDatabase : ScriptableObject
    {
        [DisplayLabel("命名空间")]
        public string NameSpace;
        public List<ItemConfig> ItemConfigs;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseInspector :Editor
    {
        class ItemEditor
        {
            public ItemConfig Item;
            public Editor Editor;
            public bool Foldout = false;
        }

        static ItemDatabase _target;
        static SerializedProperty _itemConfigsSp;

        static string _searchKey = string.Empty;

        static List<ItemEditor> _itemEditors = new List<ItemEditor> ();

        FluentGUIStyle _headStyle = FluentGUIStyle.Label().FontBold();

        public void OnEnable()
        {
            _itemEditors.Clear();

            _target = target as ItemDatabase;
            _itemConfigsSp = serializedObject.FindProperty("ItemConfigs");

            for (int i = 0; i <  _itemConfigsSp.arraySize; i++)
            {
                var itemSo = _itemConfigsSp.GetArrayElementAtIndex(i);
                var editor = CreateEditor(itemSo.objectReferenceValue);
                _itemEditors.Add(new ItemEditor
                {
                    Item = itemSo.objectReferenceValue as ItemConfig,
                    Editor = editor
                });
            }
        }

        public override void OnInspectorGUI() 
        {
            serializedObject.Update();
            GUILayout.BeginVertical("box");
            serializedObject.DrawProperties(false, 0, "ItemConfigs");
            // 
            if (GUILayout.Button("生成代码"))
            {
                GenerateCode();
            }
            GUILayout.EndVertical();

            EditorGUILayout.Separator();
            GUILayout.Label("物品列表:", _headStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label("搜索:", GUILayout.Width(40));
            _searchKey = GUILayout.TextField(_searchKey);
            GUILayout.EndHorizontal();

            for (int i = 0;  i < _itemEditors.Count; i++) 
            {
                var itemEditor = _itemEditors[i];
                if (!(itemEditor.Item.GetName().Contains(_searchKey) || itemEditor.Item.GetKey().Contains(_searchKey)))
                {
                    continue;
                }
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                itemEditor.Foldout = EditorGUILayout.Foldout(itemEditor.Foldout, itemEditor.Item.GetName());
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("-") 
                    && EditorUtility.DisplayDialog("提示", "确定要删除数据" + itemEditor.Item.GetName() + "吗?", "确定", "取消"))
                {
                    DelItemConfig(i);
                }
                GUILayout.EndHorizontal();
                if (itemEditor.Foldout)
                {
                    itemEditor.Editor.OnInspectorGUI();
                }
                GUILayout.EndVertical();
            }

            if (GUILayout.Button("添加物品"))
            {
                AddItemConfig();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private static void AddItemConfig()
        {
            var itemConfig = CreateInstance<ItemConfig>();
            itemConfig.name = "Item";
            itemConfig.Name = "新物品";
            itemConfig.Key = "Item_Key";

            AssetDatabase.AddObjectToAsset(itemConfig, _target);
            _itemConfigsSp.InsertArrayElementAtIndex(_itemConfigsSp.arraySize);
            var arrayElement = _itemConfigsSp.GetArrayElementAtIndex(_itemConfigsSp.arraySize - 1);
            arrayElement.objectReferenceValue = itemConfig;

            var itemSo = arrayElement;
            var editor = CreateEditor(itemSo.objectReferenceValue);
            _itemEditors.Add(new ItemEditor
            {
                Item = itemSo.objectReferenceValue as ItemConfig,
                Editor = editor
            });

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void DelItemConfig(int idx)
        {
            var arrayElement = _itemConfigsSp.GetArrayElementAtIndex(idx);
            var objectValue = arrayElement.objectReferenceValue;
            // 
            for (int i = 0; i < _itemEditors.Count; i++)
            {
                if (_itemEditors[i].Item == objectValue)
                {
                    _itemEditors.RemoveAt(i);
                    break;
                }
            }

            AssetDatabase.RemoveObjectFromAsset(objectValue);
            _itemConfigsSp.DeleteArrayElementAtIndex(idx);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void GenerateCode()
        {
            string filePath = AssetDatabase.GetAssetPath(_target).GetFolderPath() + "/Items.cs";

            ICodeScope rootCode = new RootCode()
                .Using("UnityEngine")
                .Using("Qframework")
                .EmptyLine()
                .Namespace(_target.NameSpace, ns => 
                {
                    ns.Class("Items", string.Empty, false, false, cs =>
                    {
                        foreach (var itemConfig in _target.ItemConfigs)
                        {
                            cs.Custom("public static string " + itemConfig.Key + " = \"" + itemConfig.Key + "\";");
                        }
                    });
                });

            using var fileWrite = File.CreateText(filePath);
            var fileCodeWrite = new FileCodeWriter(fileWrite);
            rootCode.Gen(fileCodeWrite);
        }
    }
#endif
}
