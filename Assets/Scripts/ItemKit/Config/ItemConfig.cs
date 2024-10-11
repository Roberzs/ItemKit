using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface IItem
    {
        string GetKey();
        string GetName();
        string GetIcon();
    }

    [CreateAssetMenu(menuName = "ItemKit/ Create ItemConfig")]
    public class ItemConfig : ScriptableObject, IItem
    {
        public string Key {  get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public string GetIcon() => Icon;

        public string GetKey() => Key;

        public string GetName() => Name;
    }
}

