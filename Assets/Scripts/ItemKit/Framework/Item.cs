using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class Item
    {
        public string Key { get; set; }
        public string Name { get; set; }

        public Item(string key, string name)
        {
            Key = key;
            Name = name;
        }
    }
}
