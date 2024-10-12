using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface IItem
    {
        string GetKey();
        string GetName();
        Sprite GetIcon();
    }

    public class Item : IItem
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public Sprite Icon { get; set; }

        public Item(string key, string name, Sprite icon)
        {
            Key = key;
            Name = name;
            Icon = icon;
        }

        public string GetKey() => Key;

        public string GetName() => Name;

        public Sprite GetIcon() => Icon;
    }
}
