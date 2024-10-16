using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface IItem
    {
        string GetKey();
        string GetName();
        string GetDescription();
        ItemLanguagePackage.LocaleItem LocaleItem { get; set; }
        Sprite GetIcon();
        bool IsStackable { get; }
        bool HasMaxStackableCount { get; }
        int MaxStackableCount { get; }

        bool GetBoolean(string propertyName);
    }

    //public class Item : IItem
    //{
    //    public string Key { get; set; }
    //    public string Name { get; set; }
    //    public Sprite Icon { get; set; }

    //    public Item(string key, string name, Sprite icon)
    //    {
    //        Key = key;
    //        Name = name;
    //        Icon = icon;
    //    }

    //    public string GetKey() => Key;

    //    public string GetName() => Name;

    //    public Sprite GetIcon() => Icon;

    //    public bool GetBoolean(string propertyName)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
}
